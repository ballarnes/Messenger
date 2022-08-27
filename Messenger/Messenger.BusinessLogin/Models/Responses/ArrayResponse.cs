using System.Collections.Generic;

namespace Messenger.BusinessLogic.Models.Responses
{
    public class ArrayResponse<T>
    {
        public int TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}