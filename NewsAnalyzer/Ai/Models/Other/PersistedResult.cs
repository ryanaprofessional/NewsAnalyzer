using Ai.Static;

namespace Ai.Models.Other
{
    public class PersistedResult<T> : OperationalStaus
    {
        public int Id { get; set; } = 1;
        public T Result { get; set; }
    }
}
