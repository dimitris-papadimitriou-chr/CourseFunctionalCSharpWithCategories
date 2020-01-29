using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Functors.Nullable
{
    public static partial class FunctionalExtensions
    {
        public static Nullable<T1> Map<T, T1>(this Nullable<T> @source, Func<T, T1> f)
              where T : struct
             where T1 : struct
      =>    source.HasValue ? new Nullable<T1>(f(source.Value)) : null;

    }
    public class Demo
    {

        public void Run()
        {
            int? value =5;

            if (value.HasValue)
            {
                value++;
            }

            Console.WriteLine(value.Map(x=>x++));
 

        }
    }
}

