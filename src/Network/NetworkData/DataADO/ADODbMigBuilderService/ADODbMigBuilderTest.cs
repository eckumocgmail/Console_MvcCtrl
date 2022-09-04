
using DataCommon.DatabaseMetadata;

using statistics_endpoint;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class ADODbMigBuilderTest: TestingElement
    {

        class Account
        {
            public int ID { get; set; }
            public string Email { get; set; }
        }
        class Person
        {
            public int ID { get; set; }
            public string Email { get; set; }
        }
        class AppUser
        {
            public int ID { get; set; }
            public int AccountID { get; set; }
            public int PersonID { get; set; }
            public Account Account { get; set; }
            public Person Person { get; set; }
        }



        public static List<string> Run()
        {
            SqlServerMigBuilder builder = new SqlServerMigBuilder();
            Type[] Entities = new Type[] 
            {                
                typeof(Account),
                typeof(Person),
                typeof(AppUser) 
            };
            var messages = new List<string>();
            foreach (var Entity in Entities)
            {
                builder.AddEntityType(Entity);
                foreach(var mig in builder.DropAndCreate())
                {
                    string message = (mig.Up
                            .ReplaceAll("  ", " ")
                            .ReplaceAll("  ", " ") );
                    builder.Info(message.Split("/n"));
                    messages.AddRange(new List<string>(message.Split("/n")));
                }
            }
            return messages;
        }

        protected override void OnTest()
        {
            Run();
        }
    }
}
