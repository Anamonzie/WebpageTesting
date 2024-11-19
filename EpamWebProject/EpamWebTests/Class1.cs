public class Class1
{
    static void Hello(string message)
    {
        Console.WriteLine("Hello " + message);
    }

    static void Goodbye(string message)
    {
        Console.WriteLine("Bye " + message);
    }

    delegate void CustomCallback(string message);

    public static void Examples()
    {
        CustomCallback Hi, Bye, HiBye, HiByeMinus;

        Hi = Hello;
        Bye = Goodbye;
        HiBye = Hi + Bye;
        HiByeMinus = HiBye - Hi;

        //HiBye += Hi;

        Hi("A");
        Bye("B");
        HiBye("C"); // this one is going to printb oth
        HiByeMinus("D");


        ///PrintMessage printer = (message) => Console.WriteLine(message);
    }
}
