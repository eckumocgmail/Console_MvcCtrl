using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SummaryOfType: FromAttributes
{


    public class SummaryOfProperty: FromAttributes
    {
        public string Name { get; set; }
        public string Icon { get; set; } = "home";
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        public string HelpMessage { get; set; } = "";

        public SummaryOfProperty(Type t, string prop)
        {
            Init(t, prop);
        }
    }



    public string Name { get; set; }
    public string EntityIcon { get; set; } = "home";
    public string EntityLabel { get; set; } = "";
    public string HelpMessage { get; set; } = "";
    public string ClassDescription { get; set; } = "";


    public Dictionary<string, SummaryOfProperty> SummaryOfProperties { get; set; } = new Dictionary<string, SummaryOfProperty>();


    public SummaryOfType( Type type )
    {
        Init(type);
        Name = type.Name;
        type.GetOwnPropertyNames().ToList().ForEach(name =>
        {
            SummaryOfProperties[name] = new SummaryOfProperty(type, name);
        });
    }
}