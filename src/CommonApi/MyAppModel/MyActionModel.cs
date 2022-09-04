using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ApplicationCore.Converter.Models
{
    /// <summary>
    /// Модель параметров вызова удаленной процедуры
    /// </summary>
    public class MyActionModel
    {
        public string Method { get; set; } = "Get";
        public string Name { get; set; } = "Index";
        public string Path { get; set; } = "/Index";
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, MyParameterDeclarationModel> Parameters { get; set; } = new Dictionary<string, MyParameterDeclarationModel>();

        public MyActionModel()
        {
        }

    }
}