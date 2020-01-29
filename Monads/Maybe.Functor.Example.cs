using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalCSharpWithCategories.Monads.Maybe.Example

{

    public static partial class FunctionalExtensions
    {
        public static Maybe<T> FirstOrNone<T>(this List<T> @source, Func<T, bool> predicate)
        {
            var firstOrDefault = @source.FirstOrDefault(predicate);
            if (firstOrDefault != default)
                return new Some<T>(firstOrDefault);
            else
                return new None<T>();
        } 
    }

    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MockClientRepository
    {
        List<Client> clients = new List<Client>{
                    new  Client{Id=1, Name="Jim"},
                    new  Client{Id=2, Name="John"}
                };

        public Maybe<Client> GetById(int id)
        => clients.FirstOrNone(x => x.Id == id);

    }



    public abstract class Maybe<T>
    {
        public abstract Maybe<T1> Map<T1>(Func<T, T1> f);

        public abstract T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern);
    }

    public class None<T> : Maybe<T>
    {
        public None() { }

        public override Maybe<T1> Map<T1>(Func<T, T1> f)
        {
            return new None<T1>();
        }

        public override T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern)
        {
            return pattern.none();
        }
    }

    public class Some<T> : Maybe<T>
    {
        private readonly T value;
        public Some(T value) => this.value = value;

        public override Maybe<T1> Map<T1>(Func<T, T1> f)
        {
            return new Some<T1>(f(this.value));
        }
        public override T1 MatchWith<T1>((Func<T1> none, Func<T, T1> some) pattern)
        {
            return pattern.some(this.value);
        }
    }

    public class Demo
    {

        public void Run()
        {
            var repository = new MockClientRepository();

            var result =
                repository.
                GetById(6)
                  .MatchWith(pattern:(
                        none: () => "Not Found",
                        some: (client) => client.Name
                    ));
        }
    }
}

