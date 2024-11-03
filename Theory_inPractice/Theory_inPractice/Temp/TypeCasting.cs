using static Theory_inPractice.Temp.TypeCasting.ImplicitCasting;

namespace Theory_inPractice.Temp
{
    internal class TypeCasting
    {
        public class ImplicitCasting
        {
            public interface IVehicle { void Move(); }
            public class Car : IVehicle { public void Move() { Console.WriteLine("moving"); } }

            public static class Program
            {
                public static void Main()
                {
                    Car myCar = new Car();
                    IVehicle obj = myCar;
                    obj.Move();
                }
            }
        }

        public class ExplicitCasting
        {
            public interface IVehicle2 { void Move(); }
            public class Car2 : IVehicle { public void Move() { Console.WriteLine("moving"); } }

            public static class Program2
            {
                public static void Main()
                {
                    Car myCar = new Car();
                    IVehicle obj = (IVehicle)myCar;
                    obj.Move();
                }
            }
        }

        public class AsKeywordCasting
        {
            public interface IVehicle3 { void Move(); }
            public class Car3 : IVehicle { public void Move() { Console.WriteLine("moving"); } }

            public static class Program3
            {
                public static void Main()
                {
                    Object obj = new Car();
                    IVehicle vehicle = obj as IVehicle; // if (obj is IVehicle vehicle)

                    if (vehicle != null)
                    {
                        vehicle.Move();
                    }
                }
            }
        }
    }
}
