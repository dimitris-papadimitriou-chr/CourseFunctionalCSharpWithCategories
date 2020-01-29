using System;

namespace FunctionalCSharpWithCategories.Functors.IO
{

    public class IO<T>
    {
        Func<T> @fn;
        public IO(Func<T> fn) { this.fn = fn; }
        public T Run() { return fn(); }
        public IO<T1> Map<T1>(Func<T, T1> f)
        {
            return new IO<T1>(() => f(this.fn()));
        }
    }
    public class Demo
    {

        public void Run()
        {
            var result = new IO<int>(() => 2).Map(x => x * x);
            var lifted = result.Run();


        }
    }
}

