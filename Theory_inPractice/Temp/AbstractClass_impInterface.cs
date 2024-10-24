//namespace Theory_inPractice
//{
//    public interface IAnimal
//    {
//        void Eat();
//        void Sleep();
//    }

//    public abstract class Mammal : IAnimal
//    {
//        public void Sleep()
//        {
//            Console.WriteLine("Mammal is sleeping.");
//        }

//        public abstract void Eat();
//    }

//    public class Dog : Mammal
//    {
//        public override void Eat()
//        {
//            Console.WriteLine("Dog is eating.");
//        }
//    }

//    public static class Program
//    {
//        public static void Main()
//        {
//            IAnimal myDog = new Dog();
//            myDog.Sleep();
//            myDog.Eat();

//            // explicit type casting
//            Dog dog = new();

//            IAnimal animal = (IAnimal)dog;
//            animal.Eat();

//            // with as operator - calls null if cast fails.
//            Dog asDog = new();
//            IAnimal animal1 = asDog as IAnimal;

//            if (animal1 != null)
//            {
//                animal1.Eat();
//            }
//            else
//            {
//                Console.WriteLine("The object does not implement IAnimal.");
//            }
//        }
//    }
//}
