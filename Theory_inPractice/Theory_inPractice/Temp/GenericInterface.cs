using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theory_inPractice.Temp
{
    public class GenericInterface
    {
        public interface IRepository<T>
        {
            void Add(T item);
            T Get(int id);
        }

        public class StringRepository : IRepository<string>
        {
            private List<string> myList = [];

            public void Add(string item)
            {
                myList.Add(item);
            }

            public string Get(int id)
            {
                return myList[id];
            }
        }
    }
}
