using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Laws
{

    public class Demo
    {
        public static void Run()
        {
            Id<int> result = new Id<int>(4).Bind(x => new Id<int>(x + 4)); 
        }
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
        public Id<T1> Bind<T1>(Func<T, Id<T1>> f) => f(Value);

        public TM Fold<TM>(TM state, Func<T, TM, TM> reducer) => reducer(Value, state);

    }

}