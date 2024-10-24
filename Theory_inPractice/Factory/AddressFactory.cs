using Theory_inPractice.Models;

namespace Theory_inPractice.Factory
{
    public class AddressFactory
    {
        public Address CreateAddress(string city, string country)
        {
            return new Address { City = city, Country = country };
        }
    }
}
