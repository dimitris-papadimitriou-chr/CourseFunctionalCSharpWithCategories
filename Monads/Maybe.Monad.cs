using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalCSharpWithCategories.Monads.Maybe
{


    public abstract class Maybe<T>
    {
        public abstract Maybe<T1> Map<T1>(Func<T, T1> f);

        public abstract T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern);

        public Maybe<T1> Bind<T1>(Func<T, Maybe<T1>> f) =>
            this.MatchWith((
                    none: () => new None<T1>(),
                    some: (v) => f(v)
                ));

        #region Linq support Aliases
        public Maybe<T1> Select<T1>(Func<T, T1> f) => Map(f);
        public Maybe<T2> SelectMany<T1, T2>(Func<T, Maybe<T1>> f, Func<T, T1, T2> selector) =>
            MatchWith((
                    none: () => new None<T2>(),
                    some: (v) => f(v).Map(x => selector(v, x))
                ));

        #endregion

    }

    public class None<T> : Maybe<T>
    {
        public None() { }

        public override Maybe<T1> Map<T1>(Func<T, T1> f) => new None<T1>();


        public override T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern) => pattern.none();

    }

    public class Some<T> : Maybe<T>
    {
        private readonly T value;
        public Some(T value) => this.value = value;

        public override Maybe<T1> Map<T1>(Func<T, T1> f) => new Some<T1>(f(this.value));
        public override T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern) => pattern.some(this.value);

    }

    public class Demo
    {

        public void Run()
        {
            Maybe<int> v = new None<int>();

            var result = v
                .Map(x => x + 1)
                .Bind(x => new Some<int>(x + 3))
                .MatchWith((
                    none: () => 0,
                    some: (x) => x
                ));

            //new List<int> { 2 }.SelectMany()
            var r = from x1 in new Some<int>(2)
                    from x2 in new Some<int>(2)
                    select x1 + x2;


        }
    }
}

