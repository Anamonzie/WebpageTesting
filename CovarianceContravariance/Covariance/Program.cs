namespace CovarianceContravariance.Covariance
{
    public static class Program
    {
        public static void Main()
        {
            // List of Cats
            IEnumerable<Cat> cats = new List<Cat>();

            // Covariance allows us to assign IEnumerable<Cat> to IEnumerable<Animal>
            IEnumerable<Animal> animals = cats;

            Console.WriteLine("Covariance example");
        }
    }
}
