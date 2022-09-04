using COM;

 

using eckumoc_common_api.CommonCollections;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

using ValidationAnnotationsNS;

using static EcKuMoC;

/// <summary>
/// Модуль тестирования наследует данный класс.
/// </summary>
[Route("/$t/[controller]")]
[Controller]
public abstract class TestingElement: MyValidatableObject
{
    [NonAction]
    public string GetRoute() => GetRouteAttribute(GetType());

    private string GetRouteAttribute(Type type)
    {
        throw new NotImplementedException();
    }

    /*public override void Info(params object[] args)
    {

        foreach(var arg in args)
        {
            WinSysAPI.InfoDialog(GetType().Name,arg.ToString());
        } 
    }*/


  
    
    
    


    public TestingElement( )
    {
    
    }


    /// <summary>
    /// Отчёт о проведении тестирования
    /// </summary>
    public TestingReport Report;

    /// <summary>
    /// Список сообщений, полученныйв результате тестирования
    /// 
    /// </summary>
    protected List<string> Messages = new List<string>();


    /// <summary>
    /// Реализация метода тестирования
    /// </summary>
    [NonAction]
    /// 
    protected abstract void OnTest();



    protected Dictionary<string,TestingElement> Container = new Dictionary<string, TestingElement>();


    /// <summary>
    /// Добавление метода тестирования
    /// </summary>
    /// <param name="unit"> метод тестирования </param>
    [NonAction]
    /// 
    protected void Push(TestingElement unit)
    {
        //Container.Set(unit.Name, unit);
        Container[unit.ConsoleLoggerName]= unit;
    }


    /// <summary>
    /// Выполнения теста и составления отчета о тестировании
    /// </summary>
    /// <returns> отчет о тестировании </returns>
    [NonAction]
    public TestingReport DoTest( bool? interactive=false )
    {   
        
        Info($"Приступаю к выполнению задачи");        

        string cmd = null;
        if (interactive == true)
        {

            
            while (true)
            {
                Thread.Sleep(333);
                Debug("press any key...");
                
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        interactive = true;
                        break;
                    default:
                        interactive = false;
                        break;
                }
            }
            
        }
        this.Report = new TestingReport();
        TestingReport report = this.Report;
        report.messages = this.Messages;
        report.name = this.GetType().Name.Substring("Test".Length);
        try
        {
            report.started = DateTime.Now;

            this.OnTest();
        }
        catch (Exception ex)
        {


            report.failed = true;
            report.messages.Add(ex.Message);

             
        }
        finally
        {
            report.ended = DateTime.Now;
            foreach (var p in Container)
            {

                Info("Следующий..." + p.Value.GetType().GetTypeName());
                report.subreports[p.Key] = p.Value.DoTest(interactive);
                if (report.subreports[p.Key].failed)
                {
                    report.failed = true;
                }
            }

        }
        return report;

    }










    /// <summary>
    /// Текстовый идентификатор
    /// </summary>    
    [NonAction]
    /// 
    public override string ToString()
    {
        return GetType().Name;
    }

      
  
}
