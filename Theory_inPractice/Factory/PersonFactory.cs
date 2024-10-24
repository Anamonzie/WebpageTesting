using Theory_inPractice.Models;

namespace Theory_inPractice.Factory
{
    public class PersonFactory
    {
        public Person CreatePerson(string name, int age, Address address)
        {
            return new Person { Name = name, Age = age, Address = address };
        }
    }
}
