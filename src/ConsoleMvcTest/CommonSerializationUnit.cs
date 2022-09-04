using CommonTests.CommonCollections;

using eckumoc_common_api.CommonCollections;

using eckumoc_netcore_cmd_builder.ConsoleCmdBuilder;

using Newtonsoft.Json;

using System;

namespace CommonTests
{
    public class CommonSerializationUnit : TestingElement
    {
        protected override void OnTest()
        {
            //Debug("Протекстируй программу сериализации объектов");

            var services = new CommonDictionary<string>();
            services.Set("Задача1", "Протекстируй программу сериализации объектов");
            var exec1 = new JsonExecuter(services);

            var exec2 = new JsonExecuter(services);
            var settings = new JsonSerializerSettings();

            settings.Formatting = Formatting.Indented;
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    exec2.GetPrototype( ),
                settings)
            );
        }
    }
}