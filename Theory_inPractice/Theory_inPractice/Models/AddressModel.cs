namespace Theory_inPractice.Models
{
    public class AddressModel
    {
        public string City { get;  set; }
        public string Country { get;  set; }

        public AddressModel(string city, string country)
        {
            City = city;
            Country = country;
        }
    }
}
