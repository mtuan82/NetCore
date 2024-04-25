namespace NetCoreWebAPI.Services.Redis.Model
{
    public class Price
    {
        public string ProviderId { get; set; }

        public string PickupZip { get; set; }

        public string DropoffZip { get; set; }

        public double TransitPrice { get; set; }

        public string LastUpdateBy { get; set; }

        public DateTime LastUpdateDate { get; set; } = DateTime.Now;
    }
}
