namespace CovarianceContravariance.Contravariance
{
    public class Cat : Animal 
    {
        public string Breed { get; set; }

        public Cat(string species, string breed) : base(species)
        {
            Breed = breed;
        }
    }
}
