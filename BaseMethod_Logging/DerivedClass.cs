using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseMethod_Logging
{
    public class DerivedClass : BaseClass
    {
        public override void Implementation()
        {
            Console.WriteLine("Logging - trying to call the base method");

            base.Implementation();

            Console.WriteLine("Logging - base method has been sucessfully run");
        }
    }
}
