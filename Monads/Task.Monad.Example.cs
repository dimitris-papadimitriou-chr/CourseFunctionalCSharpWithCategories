using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FunctionalCSharpWithCategories.Monads.Either;
using FunctionalCSharpWithCategories.Monads.Maybe;

namespace FunctionalCSharpWithCategories.Task.Monad.Example
{
    public static partial class FunctionalExtensions
    {
        public static Task<T1> Map<T, T1>(this Task<T> @source, Func<T, T1> f) =>
              source.ContinueWith(task => f(task.Result));
        public static Task<T1> Bind<T, T1>(this Task<T> @source, Func<T, Task<T1>> f) =>
           source.ContinueWith(task => f(task.Result).Result);

        public static Task<Either<TLeft, T1>> Bind<TLeft, T, T1>(this Task<Either<TLeft, T>> @source, Func<T, Task<T1>> f)
        {
            var result = source.ContinueWith(task =>
            task.Result.MatchWith<Either<TLeft, T1>>((
                    left: (e) => new Left<TLeft, T1>(e),
                    right: (v) => new Right<TLeft, T1>(f(v).Result)
                )));
            return result;
        }

        public static Task<Either<TLeft, T1>> Map<TLeft, T, T1>(this Task<Either<TLeft, T>> @source, Func<T, T1> f) =>
          source.ContinueWith(task => task.Result.MatchWith<Either<TLeft, T1>>((
                        left: (e) => new Left<TLeft, T1>(e),
                        right: (v) => new Right<TLeft, T1>(f(v))
                    ))
          );

        public static Task<Either<TLeft, T1>> Bind<TLeft, T, T1>(this Task<Either<TLeft, T>> @source, Func<T, Task<Either<TLeft, T1>>> f)
        {
            var result = source.ContinueWith(task =>
            task.Result.MatchWith((
                    left: (e) => new Left<TLeft, T1>(e),
                    right: (v) => f(v).Result
                )));
            return result;
        }
    }

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
        public static Either<TLeft, T> ToEither<TLeft, T>(this Maybe<T> @source, TLeft defaultLeft)
        {
            return @source.MatchWith<Either<TLeft, T>>((
                    none: () => new Left<TLeft, T>(defaultLeft),
                    some: (v) => new Right<TLeft, T>(v)
                ));
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

        public Task<Either<string, Client>> GetById(int id) =>
            Task<Either<string, Client>>.Run(() => clients.FirstOrNone(x => x.Id == id)
            .ToEither("No client Found"));

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
        public Task<Either<string, Employee>> GetById(int id)
        => Task<Either<string, Client>>.Run(() => clients.FirstOrNone(x => x.Id == id)
            .ToEither("No employee Found"));

    }

    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();
        public Task<Either<string, Employee>> GetAssignedEmployeeNameById(int clientId)
        {
            var result = clients
                .GetById(clientId)
                .Bind(clientTask => employees.GetById(clientTask.EmployeeId));
            return result;
        }

    }
    public class Demo
    {

        public async System.Threading.Tasks.Task RunAsync()
        {
            var assignedEmployeeName = await new Controller().GetAssignedEmployeeNameById(1);
            assignedEmployeeName = await new Controller().GetAssignedEmployeeNameById(2);
            assignedEmployeeName = await new Controller().GetAssignedEmployeeNameById(4);
        }
    }

}

