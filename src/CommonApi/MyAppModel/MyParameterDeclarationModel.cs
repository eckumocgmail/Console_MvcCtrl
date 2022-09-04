using DetailsAnnotationsNS;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



/// <summary>
/// Модель параметра вызова метода или процедуры или функции.
/// ПО этой модели приложение-клиент создаёт поле для ввода информации
/// на форме выполнения операции.
/// </summary>
public class MyParameterDeclarationModel 
{
    public string ParameterName { get; set; }
    public string ParameterType { get; set; }
    public bool ParameterIsOptional { get; set; }
    public int ParameterPosition { get; set; }
    public object ParameterDefValue { get; set; }

    [JsonIgnore]
    [NotMapped]
    public MyParameterArgumentModel Argv { get; set; }

    [JsonIgnore]
    [NotMapped]    
    public Action<object,object> Input = (sender, evt) => { };






    public string Name { get => ParameterName; set => ParameterName = value; }
    public string Type { get => ParameterType; set => ParameterType = value; }
    public bool IsOptional { get => ParameterIsOptional; set => ParameterIsOptional = value; }
    public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string,string>();



    public string Label
    {
        get
        {
            var attrs = Attributes;
            return attrs==null? "":
                attrs.ContainsKey(nameof(EntityLabelAttribute)) ? attrs[nameof(EntityLabelAttribute)] :
                attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : Name;
        }
    }
    public string Description
    {
        get => COM.AttrUtils.DescriptionFor(Attributes);
    }
    public string Icon
    {
        get => COM.AttrUtils.IconFor(Attributes);
    }
    public string Help
    {
        get => COM.AttrUtils.HelpFor(Attributes);
    }
    public string[] Keys
    {
        get => COM.AttrUtils.KeysFor(Attributes);
    }

    public void OnInput(string json)
    {
        this.Input.Invoke(this, new Dictionary<string, string>()
        {
            {"Value", json }
        });
    }
}
