using ApplicationCore.Converter.Models;

using CommonTests;
using CommonTests.CommonProcessing;
using CommonHttp.CommandLine;
using ConsoleApp.console_app_src.Streaming;

using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonModule
{

    /// <summary>
    /// Модель простого консольного приложения.
    /// 3 операции:
    /// -выбор действия
    /// -ввода параметров
    /// -результат выполнения, предоставляет выбор действия
    /// </summary>
    public class CommonProgram  
    {

        private IStreaming agent = new ConsoleStreaming();

     
        /// <summary>
        /// модель контроллера действий ( )
        /// </summary>
        private object _Item;
        private readonly MyControllerModel service;

        public CommonProgram(object Item):base()
        {
            this.service = this.CreateServiceModel(Item.GetType());
            if ((this._Item = Item) == null)
                throw new ArgumentException("Item");
            
        }


        public static EcKuMoC.Compile Run(EcKuMoC.Compile build)
        {
            return build;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            Func<string, Action> Callback = (selected) =>
            {
                IDictionary<string, object> args = new Dictionary<string, object>();
                MethodInfo methodInfo = null;
                do
                {
                    try
                    {
                        methodInfo = _Item.GetType().GetMethod(selected);

                        if (methodInfo == null)
                            Console.WriteLine("Не удалось найти: " + selected);
                    }
                    catch(Exception ex)
                    {
                        agent.WriteLine(ex.Message);
                    }
                } while (methodInfo == null);

                return Validate(methodInfo, args, (argsv) =>
                {
                    return () =>
                    {
                        string typeOfExecutor = _Item.GetType().Name;
                        _Item = _Item.Call(methodInfo.Name, args);
                    };
                },
                (errors) =>
                {

                    
                    return Input(
                        null,//this.service.Actions[methodInfo.Name],
                        methodInfo.GetParameters().Where(p => errors.ContainsKey(p.Name)).ToArray(),
                        (form) =>
                        {
                            foreach (var kv in form) args[kv.Key] = kv.Value;
                            _Item = _Item.Call(methodInfo.Name, args);

                        }
                    );
                });
            };

            Action next = Select(_Item.GetType().GetOwnPublicMethods(), Callback);
            next();
        }

 
        /// <summary>
        /// Генерация модели клиента http-контроллера
        /// </summary>
        public MyControllerModel CreateServiceModel(Type controllerType)
        {
            var model = CommandLineAssembly.GetExecuting().CreateServiceModel(controllerType);
            model.ToXML().WriteToConsole();
            return model;
        }

        /// <summary>
        /// Проверка параметров вызова
        /// </summary>
        /// <param name="info"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Action Validate(
            MethodInfo info,
            IDictionary<string, object> args,
            Func<IDictionary<string, object>, Action> validated,
            Func<IDictionary<string, string>, Action> notvalidated)
        {
            Dictionary<string, string> state = new Dictionary<string, string>();
            var set = new HashSet<string>(info.GetParameters().Select(p => p.Name));
            set.Except(args.Keys).ToList().ForEach(k => {
                state[k] = "Введите " + k;
            });
            if (state.Count() == 0)
            {
                return validated(args);
            }
            else
            {
                return notvalidated(state);
            }
        }



        /// <summary>
        /// Метод ввода параметра через из потока agent
        /// </summary>
        private Action Input(MyActionModel myActionModel, ParameterInfo[] parameters, Action<IDictionary<string, object>> handler)
        {
            return () =>
            {                 
                Dictionary<string, object> args = new Dictionary<string, object>();
                foreach (var par in parameters.Select(p => p.Name))
                {
                    var declaration = myActionModel.Parameters[par];

                    declaration.Input += (object sender, object evt) => {
                        declaration.Argv.Value = evt;
                        args[declaration.ParameterName] =
                            declaration.ParameterType.ToType().FromText(evt.GetType().GetProperty("Value").GetValue(evt).ToString());
                    };

                    while(IsValid(declaration)==false)
                    {
                        agent.WriteLine("Введите: " +
                            ToInputHtml(declaration));
                        var json = Console.ReadLine();
                        declaration.OnInput(json);
                    }
                }
                handler(args);
            };
        }
        public string ToInputHtml(MyParameterDeclarationModel declaration)
        {
            if (declaration.Argv == null)
            {
                //declaration.Init();
            }
            return $@"<div><label>{declaration.Get("Label")}</label><input type='text' value='{declaration.Argv.Value}'></div>";
        }
        private bool IsValid(MyParameterDeclarationModel declaration)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public Action InputByConsole(ParameterInfo[] parameters, Action<IDictionary<string, object>> handler)
        {

            

            return () =>
            {
                Dictionary<string, object> args = new Dictionary<string, object>();
                foreach (var par in parameters)
                {
                    Console.WriteLine("Введите: " + par.Name);
                    var text = Console.ReadLine();
                    args[par.Name] = par.ParameterType.FromText(text);
                }
                handler(args);
            };
        }

        


        /// <summary>
        /// Выбор операции, вывод списка допустимых значений в управляющий поток,
        /// затем ввод параметров запроса из управляющего потока.
        /// В случае ввода недопустимого наименования операция действие повторияется.
        /// </summary>
        /// <param name="actions">Список допустимых операций</param>
        /// <param name="selector">Делегирование управления функции валидации, которая возвращает следующую операцию для исполнения</param>
        /// <returns></returns>
        public Action Select(string[] actions, Func<string, Action> selector)
        {
            agent.Clear();
            agent.WriteLine($"[Тип]: {_Item.GetType().Name} \n{_Item.ToJsonOnScreen()}");            
            MethodInfo methodInfo = null;
            do
            {
                agent.WriteLine($"\n\n Выберите операцию: ");
                foreach (string action in actions)
                    agent.WriteLine(" -" + action);
                string input = agent.ReadLine();
                methodInfo = _Item.GetType().GetMethod(input);
                if (methodInfo == null)
                    agent.WriteLine("Не удалось найти: " + input);
            } while (methodInfo == null);
            return selector(methodInfo.Name);
        }



        /// <summary>
        /// Пример использования
        /// </summary> 
        public static int Run(string[] args)
        {
            var program = new CommonProgram(new SqlServerMigBuilder());
           
            if (Environment.UserInteractive == false)
            {
                if(program.SubmitDialog("Выполнить модульные тесты?"))
                {
                    var unit = new CommonUnit();
                    unit.DoTest().ToDocument().WriteToConsole();

                }


            }
            program.Run();


            return 0;
        }



        /// <summary>
        /// Передача управления сетевому агенту
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool SubmitDialog(string v)
        {
            agent.WriteLine(v);
            if(agent.ReadLine().ToLower()=="y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}