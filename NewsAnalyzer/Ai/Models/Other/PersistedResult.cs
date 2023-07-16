using Ai.Static;

namespace Ai.Models.Other
{
    public class PersistedResult : OperationalStaus
    {
        public int Id { get; set; } = 1;
        public object Result { get; set; } = 0;
    }
}
