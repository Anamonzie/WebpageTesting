using System;

namespace examples
{
	public class BuiltInExamples()
	{
        public void examples()
        {
            Action<string> print = Console.WriteLine;
            print("Hello, QA!");

            Func<int, int, int> add = (x, y) => x + y;
            int result = add(5, 10);
            Console.WriteLine(result);  // Output: 15

            Predicate<int> isPositive = x => x > 0;
            bool check = isPositive(10);
            Console.WriteLine(check);  // Output: True

            ///Func<IPage, IHomepage> CreateHomepage2 = static (IPage page) => new Homepage(page);
        }
    }
}
