namespace News.Models.Other
{
    public class PersistedResult : OperationalStaus
    {
        public int Id { get; set; } = 1;
        public string Code { get; set; }
        public object Result { get; set; } = 0;
    }
}
