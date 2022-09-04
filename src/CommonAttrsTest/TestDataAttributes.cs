using COM;

using DetailsAnnotationsNS;

using System;
using System.ComponentModel.DataAnnotations;

using ValidationAnnotationsNS;


public class TestDataAttributes : TestingElement
{
    protected override void OnTest()
    {
        canParseInputMultilineText();
        canDescriptionAttributes();
        canValidatePropertyOnChangeEvent();
    }

    private void canParseInputMultilineText()
    {
        var props = new TestAttributeInputClass();
        var inputType = AttrUtils.GetInputType(props.GetType(), "Description");
        //Writing.ToConsole(inputType);
        if(AttrUtils.GetInputType(props.GetType(), "Description") != "MultilineText")
        {
            throw new Exception("Свойства помеченные атрибутом ввода многострочный текст работают не корректно");
        }
    }

    private void canDescriptionAttributes()
    {
        try
        {
            if (COM.AttrUtils.DescriptionFor(new PersonForTest()) != "Персональная информация")
            {
                throw new Exception("Атрибут краткого описания класса не был получен");
            }
            Messages.Add("Реализован атрибут краткого описания модели");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Messages.Add("Атрибут краткого описания класса не был получен");
        }                         
    }


    private void canValidatePropertyOnChangeEvent()
    {
        try
        {
            PersonForTest person = new PersonForTest();
            person.Email = "fuck off";
            person.EnsureIsValide();
            throw new Exception("Дополнительная проверка данных с помощью атрибутов работает не корректно");
        }
        catch(ValidationException ex)
        {

            Messages.Add("Реализована дополнительная проверка данных с помощью атрибутов");
        }            
    }
  








    /// <summary>
    /// Модель для тестирования
    /// </summary>
    [Description("Персональная информация")]
    class PersonForTest : MyValidatableObject
    {
        [InputEmail( "Электронный адрес задан некорректно")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }

        

        public PersonForTest()
        {
            this.Email = "eckumoc@gmail.com";
        }
    }
}








