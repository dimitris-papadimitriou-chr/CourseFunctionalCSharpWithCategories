using System;
using System.Threading.Tasks;

namespace FunctionalCSharpWithCategories.Task.Monad
{
    public static partial class FunctionalExtensions
    {
        public static Task<T1> Map<T, T1>(this Task<T> @source, Func<T, T1> f) =>
              source.ContinueWith(task => f(task.Result));

        public static async Task<T1> BindAsync<T, T1>(this Task<T> @source, Func<T, Task<T1>> f)
        {
            var value = await @source;
            var continuation = await f(value);
            return continuation;
        }


        public static void MatchWith<T>(this Task<T> @this,
            (Action<T> ok, Action<Exception> error) pattern) =>
        @this.ContinueWith(t =>
        {
            if (t.IsFaulted)
                pattern.error(t.Exception);
            else
                pattern.ok(t.Result);
        });

        public static async Task<T1> MatchWithAsync<T, T1>(this Task<T> @this,
            (Func<Exception, T1> left, Func<T, T1> right) pattern) =>
         await @this.ContinueWith(t =>
         {
             if (t.IsFaulted)
                 return pattern.left(t.Exception);
             else
                 return pattern.right(t.Result);
         });
         
    }
    public class Demo
    {

        public async System.Threading.Tasks.Task RunAsync()
        {
            Func<int> raiseException = () => throw new Exception("Exception ");

            var result = await Task<int>.Run(() => 4)
                .Map(x => x * x)
                .BindAsync(x => Task<int>.Run(raiseException))
                .MatchWithAsync((
                left: (e) =>
                {
                    return e.Message;
                },
                right: (v) =>
                {
                    return "result:" + v;
                }
            ));


        }
    }
}

