using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Functors.FuncTT1
{
    public static partial class FunctionalExtensions
    {
        public static Func<T1> Map<T, T1>(this Func<T> @source, Func<T, T1> f)
        {
            return () => f(source());
        }

        public static Func<V, T1> Compose<V, T, T1>(this Func<V, T> f1, Func<T, T1> f2)
        {
            return ((v) =>
            {
                return f2(f1(v));

            });
        }
    }
    public class Demo
    {
        public void Run()
        {
            Func<int> factory = () => 2;
            Func<int> factoryAdd1 = factory.Map(x => x + 1);
            var result = factoryAdd1();

        }
    }
}

