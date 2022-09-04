using CommonTests.CommonCollections;

using eckumoc_common_api.CommonCollections;

using System;
using System.Linq;

namespace CommonTests
{
    public class CommonCollectionsUnit : TestingElement
    {
        protected override void OnTest()
        {
            Debug("Тестируем программу определения активных элементов системы");
            var dictionary = new CommonDictionary<string>();

            dictionary.OnGet += (name,value)=>
            {
                Console.WriteLine($"GET {name} => \n{value}");
            };

            foreach(var name in GetType().Assembly.GetTypes().Select(t => t.Name))
            {
                if (name.IsEng())
                {
                    dictionary.Set(name.ToString(), name);
                }
                
            }
            foreach (var name in GetType().Assembly.GetTypes().Select(t => t.Name))
            {
                if (name.IsEng())
                {
                    dictionary.Set(name.ToString(), name);
                }
            }
                
        }
    }
}