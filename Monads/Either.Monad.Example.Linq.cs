using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionalCSharpWithCategories.Monads.Maybe;

namespace FunctionalCSharpWithCategories.Monads.Either.Example.Linq
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

        public Either<string, Client> GetById(int id) =>
            clients.FirstOrNone(x => x.Id == id)
            .ToEither("No client Found");

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

        public Either<string, Employee> GetById(int id)
        => clients.FirstOrNone(x => x.Id == id)
            .ToEither("No employee Found");

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
                    left: (e) => "there was an issue:" + e,
                    right: (employee) => employee.Name
                ));


    }
    public class Demo
    {

        public void Run()
        {
            var assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(1);
            assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(2);
            assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(5);

        }
    }
}

