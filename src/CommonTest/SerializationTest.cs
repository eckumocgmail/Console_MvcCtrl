using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElements
{

    public class SerializationObjectTest : TestingElement
    {
        private Func<object, object> _get = (context) => {
            return context;
        };

       
        /// <summary>
        /// Реализация метода тестирования
        /// </summary>
        protected override void OnTest()
        {

            try
            {
                object instance = _get(this);
                Messages.Add("Получен экземпляр сериализации: " + instance.GetType().Name);

                //json
                try
                {
                    instance.ToJsonOnScreen().WriteToConsole();
                    Messages.Add(instance.GetType().Name+ " успешно сериализуется в JSON");
                }
                catch (Exception ex)
                {
                    Messages.Add(instance.GetType().Name + " не может сериализоваться в JSON");
                    Info(instance.GetType().Name + " не может сериализоваться в JSON");
                    Error(ex);
                }

                //xml
                try
                {
                    instance.ToXML().WriteToConsole();
                    Messages.Add(instance.GetType().Name + " успешно сериализуется в XML");
                }
                catch (Exception ex)
                {
                    Messages.Add(instance.GetType().Name + " не может сериализоваться в XML");
                    Info(instance.GetType().Name + " не может сериализоваться в XML");
                    Error(ex);
                }



            }
            catch (Exception ex)
            {                
                this.Messages.Add("Не удалось получить экземпляр объекта тестирпования");
                Info("Не удалось получить экземпляр объекта тестирпования");
                Error(ex);
            }
        }
    }

    public class SerializationTest<T>: TestingUnit
    {
        public SerializationTest(): base()
        {
            Push(new SerializationObjectTest());
        }
    }
}
