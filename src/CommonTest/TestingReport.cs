using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AttributeEntityNS;
using InputAttrsNS;
using DetailsAnnotationsNS;

/// <summary>
/// Отчет о тестировании
/// </summary>
public class TestingReport 
{
    [SystemEntity]
    [InputText]
    [Label("Наименование теста")]
    public string name { get; set; }

    [SystemEntity]
    [InputBool]
    [Label("Результат выполнения")]
    public bool failed { get; set; }

    [SystemEntity]
    [InputDateTime]
    [Label("Начало выполнения")]
    public DateTime started { get; set; }

    [SystemEntity]
    [InputDateTime]
    [Label("Завершение выполнения")]
    public DateTime ended { get; set; }

    [NotInput]
    [NotMapped]
    [JsonIgnore]
    [Label("Утверждения полученные у ходе проверки")]
    public List<string> messages { get; set; }
    
    [NotInput]
    [NotMapped]
    [JsonIgnore]
    [Label("Подъотчёты")]
    public Dictionary<string, TestingReport> subreports { get; set; }
        = new Dictionary<string, TestingReport>();


   

    /// <summary>
    /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
    /// </summary>
    /// <returns></returns>
    public virtual int GetVersion()
    {            
        return (from r in this.subreports.Values where r.failed == false select r).Count();
    }


    /// <summary>
    /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
    /// </summary>
    /// <returns></returns>
    public virtual Version GetRealisticVersion()
    {
        if(this.subreports.Count == 0)
        {
            return new Version(1, this.failed ? 0 : 1);
        }                
        return new Version(
            (from r in this.subreports.Values where r.failed==false select r).Count(), 
            this.subreports.Count);
    }

    /// <summary>
    /// Количественный номер версии, показывает кол-во выполненых проверок
    /// </summary>
    /// <returns></returns>
    public virtual Version GetMaximalisticVersion()
    {
        if (this.subreports.Count == 0)
        {
            return new Version(1, this.failed ? 0 : 1);
        }
        return new Version((from r in this.subreports.Values where r.failed == false select r).Count(), this.subreports.Count);
    }


    /// <summary>
    /// Метод получчения числовой информации о результатх тестирования 
    /// </summary>
    /// <returns> числовая информация о результатах тестирования </returns>
    public virtual string GetStat()
    {            
        if( this.subreports.Count() == 0)
        {
            return this.failed ? "0" : "1";
        }
        else
        {
            int inc = 0;
            foreach (var p in this.subreports)
            {
                if (p.Value.failed)
                {
                    break;
                }
                else
                {
                    inc++;
                }
            }
            return $"{this.subreports.Count}-{inc}";
        }            
    }


    /// <summary>
    /// Составление текстового документа, содержащего информацию о результатах тестирования
    /// </summary>
    /// <param name="isTopReport"> true, если отчет составлен на верхнем уровне </param>
    /// <returns> теккстовый документ </returns>
    public virtual string ToDocument(int level=0)
    {        
        string document = "";
        foreach (string message in messages)
        {
            for(int i=0; i<=level; i++)
            {
                document += "    ";
            }
            document += message + "\n";
        }
        int number = 1;
        foreach( var pair in this.subreports)
        {
            for (int i = 0; i <= level; i++)
            {
                document += "    ";
            }
                
            document += //$"{GetRealisticVersion().ToString()}: "+pair.Key + "\n";
                $"{number}/{this.subreports.Count()}: " + pair.Key + "\n";
            document += pair.Value.ToDocument(level+1);
            number++;
        }
        return document;
    }


    /// <summary>
    /// Преобразование в текстовый формат
    /// </summary>
    /// <returns> текстовые данные </returns>
    public override string ToString()
    {
        return JObject.FromObject(this).ToString();
    }


    public CommonNode<TestingReport> ToNode()
    {
        var pnode = new CommonNode<TestingReport>( this.name, this, null);
        foreach (var pchild in this.subreports)
        {
            pnode.Append(pchild.Value.ToNode());
        }
        return pnode;
    }
}
