namespace Core.Model.MissingTime
{
    public class MissingTime
    {
        public string Name { get; set; }
        public decimal Expected { get; set; }
        public List<Day> Days { get; set; }
    }
}
