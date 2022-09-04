

using COM;

using eckumoc_common_api.CommonCollections;

using System;

namespace ApplicationUnit.CommonUnit
{
    public class TestExpressions : TestingElement
    {

        protected override void OnTest()
        {
            try
            {
                string exprerssion = Expressions.GetUniqTextExpressionFor(
                    new CommonDictionary<string>().GetType(), "DataSet.");
                string r1 = Expression.Interpolate(exprerssion, new CommonDictionary<string>());
            }
            catch (Exception ex)
            {
                Messages.Add("В случае некорректной компиляции выражения "+
                    "Получаем исключение");
            }



 

            if (GetType().Name != Expression.Compile("GetType().Name", this).ToString())
            {
                throw new Exception("Выражения компилируются с ошщибкой");
            }

            if (GetType().Name != Expression.Interpolate("{{GetType().Name}}", this))
            {
                throw new Exception("Выражения компилируются с ошщибкой");
            }

        }
    }
}