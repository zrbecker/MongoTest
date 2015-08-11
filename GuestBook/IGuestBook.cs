using System.Collections.Generic;

namespace GuestBook
{
    public interface IGuestBook
    {
        int Count { get; }

        void Post(GuestBookEntry entry);

        IEnumerable<GuestBookEntry> Read(int index, int count);
    }
}
