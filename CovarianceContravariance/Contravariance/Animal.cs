namespace CovarianceContravariance.Contravariance
{
    public abstract class Animal 
    { 
        public string Species { get; set; }

        protected Animal(string species)
        {
            Species = species;
        }
    }
}