using FluentAssertions;
using Theory_inPractice.Factory;

namespace TheoryInPractice.Tests
{
    public class PersonTests
    {
        private PersonFactory personFactory;
        private AddressFactory addressFactory;

        [SetUp]
        public void Setup()
        {
            personFactory = new PersonFactory();
            addressFactory = new AddressFactory();
        }

        [Test]
        public void CompareDifferentCitySameCountry_ShouldBeEquivalent()
        {
            //Arrange
            var address1 = addressFactory.CreateAddress("Tbilisi", "Georgia");
            var address2 = addressFactory.CreateAddress("Batumi", "Georgia");  // Different City but same Country
            var person1 = personFactory.CreatePerson("John Doe", 30, address1);
            var person2 = personFactory.CreatePerson("John Doe", 30, address2);

            //Act

            //Assert
            person1.Should().BeEquivalentTo(person2, options => options
                .Excluding(p => p.Address.City));
        }
    }
}
