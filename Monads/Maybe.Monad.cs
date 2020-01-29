using System;

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

        public override Maybe<T1> Map<T1>(Func<T, T1> f)=> new Some<T1>(f(this.value)); 
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

        }
    }
}

