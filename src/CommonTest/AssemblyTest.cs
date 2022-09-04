using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static System.Console;



/// <summary>
/// Динамически составленный тест
/// </summary>
public class AssemblyTest: TestingUnit
{

    private Assembly _assembly;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="assembly"></param>
    public AssemblyTest(Assembly assembly )
    {
        if (assembly == null)
            throw new ArgumentException(GetType().Name+"(assembly)");
        Bind(assembly);
        
    }

    /// <summary>
    /// Считать сборку из файла и выполнить статические методы тестирования
    /// </summary>
    /// <param name="assemblyPath"></param>
    public void BindFile(string assemblyPath)
    {
        this.Bind(Assembly.LoadFile(assemblyPath));
    }


    /// <summary>
    /// Выполнитьь статические методы тестирования
    /// </summary>
    public void Bind( Assembly assembly )
    {
        this.ConsoleLoggerName = "" + assembly.GetName().Name;
        //this.Trace();


        //this.Debug(assembly.GetName().Name + "_" + assembly.ImageRuntimeVersion.ToString());
        //this.Debug(assembly.Location);
        //this.Debug(assembly.EntryPoint.DeclaringType.FullName);

        _assembly = assembly;

        foreach (var testableDelcaration in GetStaticTests())
        {
            //Info(testableDelcaration.GetTypeName());
            foreach (MethodInfo method in GetStaticTestMethods(testableDelcaration))
            {
                var operation = new TestingOperation(testableDelcaration.Name + "." + method.Name,
                () =>
                {
                    testableDelcaration.Name.ToType().New().Call(method.Name);
                    this.Messages.Add(testableDelcaration.Name + "." + method.Name + " Тест выполнен");
                });
                operation.ConsoleLoggerName = testableDelcaration.Name + "." + method.Name;
                this.Push(operation);
               
            }
        }        
    }


    /// <summary>
    /// Ищем классы реализующий статические метод с именем DoTest         
    /// </summary>
    /// <returns></returns>
    private Type[] GetStaticTests()
        => _assembly.GetTypes().Where(t => t.GetMethods().Where(m => m.Name == "OnTesting").Count() > 0).ToArray();


    /// <summary>
    /// Поиск статических методов *Test*
    /// </summary>
    private MethodInfo[] GetStaticTestMethods(params Type[] types)
    {
        var methods = types.SelectMany(t => t.GetMethods());
        return methods.Where(m => m.IsStatic && m.Name.IndexOf("Test") != -1).ToArray();
    }

    /// <summary>
    /// Выполнение и вывод в консоль
    /// </summary>
    public void Trace() => WriteLine("\n" + this.DoTest().ToDocument());

    /// <summary>
    /// ВЫполнение всех тестовых элементов, которыми считаются классы наследуемые от TestingElement
    /// </summary>
    public void RunAll()
        => _assembly.GetTypes().Where(t => t.IsExtendsFrom(typeof(TestingUnit))).ToList().ForEach((t) => {
            t.New().Call("DoTest", new Dictionary<string, object>()).Call("ToDocument", new Dictionary<string, object>());
        });
}