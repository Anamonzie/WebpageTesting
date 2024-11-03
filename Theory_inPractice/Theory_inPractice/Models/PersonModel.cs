namespace Theory_inPractice.Models
{
    public class PersonModel
    {
        public string Name { get;  set; }
        public int Age { get;  set; }
        public AddressModel Address { get;  set; }

        internal PersonModel(string name, int age, AddressModel address)
        {
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
