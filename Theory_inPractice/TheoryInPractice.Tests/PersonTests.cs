using FluentAssertions;
using Theory_inPractice;

namespace TheoryInPractice.Tests
{
    public class PersonTests
    {

        [Test]
        public void CompareDifferentCitySameCountry_ShouldBeEquivalent()
        {
            //Arrange
            var person1 = new Builder()
                .SetName("Ana")
                .SetAge(24)
                .SetAddress("Tbilisi", "Georgia")
                .Build();

            var person2 = new Builder()
                .SetName("Anna")
                .SetAge(24)
                .SetAddress("Batumi", "Georgia")
                .Build();

            //Act

            //Assert
            person1.Should().BeEquivalentTo(person2, options => options
            .Excluding(p => p.Name)
            .Excluding(p => p.Address.City));
        }
    }
}
