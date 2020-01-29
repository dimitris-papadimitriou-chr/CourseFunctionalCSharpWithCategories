using System;
 

namespace FunctionalCSharpWithCategories.Functors.FuncT_T1
{
    public static partial class FunctionalExtensions
    {
        public static Func<V, T1> Map<V, T, T1>(this Func<V, T> @source, Func<T, T1> f)
        {
            return (v) => f(source(v));
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
            Func<int,int> factory = (x) => x+2;
            Func<int,int> factoryAdd3 = factory.Map(x => x + 1);
            var result = factoryAdd3(2);
			Console.WriteLine(result);

            Func<int, int> add2 = (x) => 2 + x;
            Func<int, int> add1 = (x) => 2 + 1;
            //var add3 = add1.Compose(add2);
            var add3 = add1.Map(add2);
   

        }
    }
}

