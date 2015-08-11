using MongoDB.Bson;

namespace GuestBook
{
    public class GuestBookEntry
    {
        public string Name { get; set; }

        public string Message { get; set; }
    }
}
