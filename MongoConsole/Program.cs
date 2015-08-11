using System;
using GuestBook;
using MongoDB.Driver;

namespace MongoConsole
{
    class Program
    {
        static void MakeEntry(IGuestBook guestbook)
        {
            var entry = new GuestBookEntry();
            Console.Write("enter name: ");
            entry.Name = Console.ReadLine();

            Console.Write("enter message: ");
            entry.Message = Console.ReadLine();

            guestbook.Post(entry);

            Console.Clear();
        }

        static void Main(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("GuestBook");
            var guestbook = new MongoGuestBook(db);

            var index = 0;
            var count = 5;
            var done = false;

            while (!done)
            {
                try
                {
                    var posts = guestbook.Read(index, count);
                    Console.WriteLine("index - {0}, count - {1}", index, count);
                    Console.WriteLine();
                    foreach (var post in posts)
                    {
                        Console.WriteLine("name: {0}", post.Name);
                        Console.WriteLine("message: {0}", post.Message);
                        Console.WriteLine();
                    }
                }
                catch (GuestbookException ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("n - next page, p - previous page, e - make entry, r - refresh, q - quit");
                var input = Console.ReadKey(true).KeyChar.ToString().ToLower();
                Console.Clear();

                var lastPageIndex = count * (guestbook.Count / count);
                switch (input)
                {
                    case "n":
                        index = Math.Min(index + count, lastPageIndex);
                        break;
                    case "p":
                        index = Math.Max(index - count, 0);
                        break;
                    case "e":
                        MakeEntry(guestbook);
                        index = lastPageIndex;
                        break;
                    case "r":
                        break;
                    case "q":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("error: unknown command");
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
