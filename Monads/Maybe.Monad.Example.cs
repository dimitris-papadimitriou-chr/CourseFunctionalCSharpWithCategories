using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalCSharpWithCategories.Maybe.Monad.Example
{
    public static partial class FunctionalExtensions
    {
        public static Maybe<T> FirstOrNone<T>(this List<T> @source, Func<T, bool> predicate)
        {
            var firstOrDefault = @source.FirstOrDefault(predicate);
            return firstOrDefault != null ?
                new Some<T>(firstOrDefault) :
                (Maybe<T>)new None<T>();
        }
    }

    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
    }
    public class MockClientRepository
    {
        List<Client> clients = new List<Client>{
                    new  Client{Id=1, Name="Jim",  EmployeeId=1},
                    new  Client{Id=2, Name="John", EmployeeId=4}
                };

        public Maybe<Client> GetById(int id)
        => clients.FirstOrNone(x => x.Id == id);

    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MockEmployeeRepository
    {
        List<Employee> clients = new List<Employee>{
                    new  Employee{Id=1, Name="Jim"},
                    new  Employee{Id=2, Name="John"}
                };

        public Maybe<Employee> GetById(int id)
        => clients.FirstOrNone(x => x.Id == id);

    }



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




    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();
        public string GetAssignedEmployeeNameById(int clientId) =>
          clients
                .GetById(clientId)
                .Map(client => client.EmployeeId)
                .Bind(employees.GetById)
                .MatchWith((
                    none: () => "there is non employee assigned",
                    some: (employee) => employee.Name
                ));

    }
    public class Demo
    {

        public void Run()
        {


            var assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(1);


        }
    }
}

