using eckumoc_netcore_cmd_builder.ConsoleCmdBuilder;

namespace CommonTests
{
    public class CommonBuilderUnit : TestingElement
    {
        protected override void OnTest()
        {
            Warn("Проекстируй программу формирования конфигурации системы");

            
            var cmd = new CmdBuilder(System.IO.Directory.GetCurrentDirectory());
            cmd.Init(System.IO.Directory.GetCurrentDirectory());
            cmd.Build(); 

            //JsonExecuterTest.Run();            

        }
    }
}