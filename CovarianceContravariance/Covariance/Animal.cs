namespace CovarianceContravariance.Covariance
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