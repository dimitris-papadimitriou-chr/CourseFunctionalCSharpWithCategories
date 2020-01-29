using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Functors.Task
{
    public static partial class FunctionalExtensions
    {
        public static Task<T1> Map<T, T1>(this Task<T> @source, Func<T, T1> f) =>
              source.ContinueWith(task => f(task.Result));

    }
    public class Demo
    { 
        public void Run()
        {
            var result = Task<int>.Run(() => 2).Map(x => x * x);

            var lifted = result.Result;


        }
    }
}

