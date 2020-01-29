using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        { 
           await new FunctionalCSharpWithCategories.Task.Monad.Example.Demo().RunAsync();

        }
    }
}
