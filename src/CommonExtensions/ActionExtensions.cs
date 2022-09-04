using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

using COM;


/// <summary>
/// Расширения методов выполнения делегатов
/// </summary>
public static class ActionExtensions
{

    public static object InvokeStatic(this Type source, string name, params object[] args)
        => source.GetMethod(name).Invoke(null, args);


    public static object InvokeStatic( this MethodInfo action, params object[] args )
    {
        return action.Invoke(null, args);
    }

    public static object Call(this object actor, string method, IDictionary<string, object> valeusMap=null)
    {
        var args = new List<object>();
        var type = actor.GetType();
        while (type != null)
        {
            try
            {
                var action = type.GetMethod(method);
                actor.GetType().BaseType.GetMethods().Select(m => m.Name).ToJsonOnScreen().WriteToConsole();
                if (action == null)
                    throw new ArgumentException(new { var = "method", tag = "select", options = type.GetMethods().Select(m => m.Name) }.ToJsonOnScreen());
                if (valeusMap != null)
                {
                    foreach (var p in action.GetParameters().ToList())
                        args.Add(valeusMap[p.Name]);
                }
                try
                {
                    var result = action.Invoke(actor, args.ToArray());
                    return result;
                }
                catch (System.Reflection.TargetException ex)
                {
                    Exception t = ex;
                    Console.WriteLine("Ошибка при выполнении метода через рефлексию");
                    while (t.InnerException != null)

                    {
                        t = t.InnerException;
                        Console.WriteLine(" \t " + t.Message);
                    }
                    t.StackTrace.WriteToConsole();
                    Console.WriteLine("Чтобы продолжить выполнение нажмите enter...");
                    Console.ReadLine();
                    return actor;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    Exception t = ex;
                    Console.WriteLine("Ошибка при выполнении метода через рефлексию");
                    while (t.InnerException != null)

                    {
                        t = t.InnerException;
                        Console.WriteLine(" \t " + t.Message);
                    }
                    t.StackTrace.WriteToConsole();
                    Console.WriteLine("Чтобы продолжить выполнение нажмите enter...");
                    Console.ReadLine();
                    return actor;
                }
                catch (System.Reflection.TargetParameterCountException ex)
                {
                    Exception t = ex;
                    Console.WriteLine("Ошибка при выполнении метода через рефлексию");
                    while (t.InnerException != null)

                    {
                        t = t.InnerException;
                        Console.WriteLine(" \t " + t.Message);
                    }
                    t.StackTrace.WriteToConsole();
                    Console.WriteLine("Чтобы продолжить выполнение нажмите enter...");
                    Console.ReadLine();
                    return actor;
                }
                catch (InvalidOperationException ex)
                {
                    Exception t = ex;
                    Console.WriteLine("Ошибка при выполнении метода через рефлексию");
                    while (t.InnerException != null)

                    {
                        t = t.InnerException;
                        Console.WriteLine(" \t " + t.Message);
                    }
                    t.StackTrace.WriteToConsole();
                    Console.WriteLine("Чтобы продолжить выполнение нажмите enter...");
                    Console.ReadLine();
                    return actor;
                }
                catch (ArgumentException ex)
                {
                    Exception t = ex;
                    Console.WriteLine("Ошибка при выполнении метода через рефлексию");
                    while (t.InnerException != null)

                    {
                        t = t.InnerException;
                        Console.WriteLine(" \t " + t.Message);
                    }
                    t.StackTrace.WriteToConsole();
                    Console.WriteLine("Чтобы продолжить выполнение нажмите enter...");
                    Console.ReadLine();
                    return actor;
                }
            }
            catch(ArgumentException ar)
            {
                type = type.BaseType;
                if (type != null)
                    continue;
                else break;
            }
        }
        return null;
     

    }

    /// <summary>
    /// Выполнение делегата по истечению заданного в милисекундах промежутка времени
    /// </summary>
    /// <param name="action"> делегат </param>
    /// <param name="ms"> кол-во миллисекунд </param>
    public static void SetTimeout( this Object context,  Action action, long ms)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(ms);
        aTimer.Elapsed += (Object source, ElapsedEventArgs e) => {
            action();
            aTimer.Enabled = false;
        };
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }



    /// <summary>
    /// Выполнение с заданным интервалом определённое кол-во раз
    /// </summary>
    /// <param name="actionsCount"></param>
    /// <param name="actionTimeout"></param>
    /// <param name="p"></param>
    public static void Simulate(this Object context, Action todo, int actionTimeout, int actionsCount)
    {
        for(int i=1; i<=actionsCount; i++)
        {
            context.SetTimeout(todo, actionTimeout*i);
        }
    }

 
    /// <summary>
    /// Перехват выполнения исключений
    /// </summary> 
    public static Action Catch(this Object context, Action todo, Action<Exception> catcher)
    {
        return () =>
        {            
            lock (context)
            {
                try
                {
                    todo();
                }
                catch (Exception ex)
                {
                    catcher(ex);
                }
            }
        };
    }




    public static Action Catch(this Action todo)
    {
        return () =>
        {
             
            try
            {
                todo();
            }
            catch (Exception ex)
            {
                throw new Exception($"[{ex.TargetSite}][{ex.Source}][{ex.Message}]");
            }
            
        };
    }


 


    /// <summary>
    /// Выполнение делегата с заданным интервалом в милисекундах
    /// </summary>
    /// <param name="action"> делегат </param>
    /// <param name="ms"> кол-во миллисекунд </param>
    public static void SetInterval(this Object context, Action action, long ms)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(ms);
        aTimer.Elapsed += (Object source, ElapsedEventArgs e) => {
            action();
              
        };
        aTimer.AutoReset = true;
        aTimer.Enabled = true;            
    }
}
