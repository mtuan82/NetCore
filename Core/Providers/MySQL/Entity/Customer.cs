namespace Core.Providers.MySQL.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string country { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set;}
    }
}
