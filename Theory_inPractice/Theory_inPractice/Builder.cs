using Theory_inPractice.Models;

namespace Theory_inPractice
{
    public class Builder
    {
        private string name;
        private int age;
        private AddressModel address;

        public Builder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public Builder SetAge(int age)
        {
            this.age = age;
            return this;
        }

        public Builder SetAddress(string city, string country)
        {
            address = new AddressModel(city, country);
            return this;
        }

        public PersonModel Build()
        {
            return new PersonModel(name, age, address);
        }
    }
}
