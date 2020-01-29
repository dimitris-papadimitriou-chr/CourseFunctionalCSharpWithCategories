using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Functors.Lazy
{
    public static partial class FunctionalExtensions
    {
        public static Lazy<T1> Map<T, T1>(this Lazy<T> @source, Func<T, T1> f)
        {
            return new Lazy<T1>(() =>
            {
                return f(source.Value);

            });
        }
    }
    public class Demo
    {

        public void Run()
        {
            var result = new Lazy<int>(() => 2).Map(x => x * x);

            var lifted = result.Value;


        }
    }
}

