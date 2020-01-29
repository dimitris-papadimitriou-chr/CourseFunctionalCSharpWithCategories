using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Monads.Either
{ 
   public abstract class Either<TLeft, T>
    {
        public abstract Either<TLeft, T1> Map<T1>(Func<T, T1> f);

        public abstract  T1 MatchWith<T1>((Func<TLeft, T1> left, Func<T, T1> right) pattern);

        public Either<TLeft, T1> Bind<T1>(Func<T, Either<TLeft, T1>> f) =>
            MatchWith((
                left: (e) => new Left<TLeft, T1>(e),
                right: v => f(v) 
            ));

    }

    public class Left<TLeft, T> : Either<TLeft, T>
    {
        private readonly TLeft value;
        public Left(TLeft value) => this.value = value;

        public override Either<TLeft, T1> Map<T1>(Func<T, T1> f)
        {
            return new Left<TLeft, T1>(this.value);
        }

        public override T1 MatchWith<T1>((Func<TLeft, T1> left, Func<T, T1> right) pattern)
        {
            return pattern.left(this.value);
        }
    }

    public class Right<TLeft, T> : Either<TLeft, T>
    {
        private readonly T value;
        public Right(T value) => this.value = value;

        public override Either<TLeft, T1> Map<T1>(Func<T, T1> f)
        {
            return new Right<TLeft, T1>(f(this.value));
        }
        public override T1 MatchWith<T1>((Func<TLeft, T1> left, Func<T, T1> right) pattern)
        {
            return pattern.right(this.value);
        }
    }

    public class Demo
    {

        public void Run()
        {
            Either<string, int> v = new Right<string, int>(5);
            try
            {
                v.Map(x => x + 1);

                throw new Exception("some error");
            }
            catch (Exception e)
            {
                v = new Left<string, int>(e.Message);
            }

            var result = v
                .MatchWith((
                    left: (error) => "Error:" + error,
                    right: (x) => "result:" + x
                ));



        }
    }
}

