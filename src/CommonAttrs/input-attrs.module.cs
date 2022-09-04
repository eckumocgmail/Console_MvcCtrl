using COM;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

using ValidationAnnotationsNS;

namespace InputAttrsNS{
/******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\DataInputTypes.cs **************/
    public class DataInputTypes
    {
        public static Dictionary<string, string> DATATYPERS = new Dictionary<string, string>() {
            {"int",         "Целое число"},
            {"float",       "Вещественное число"},
            {"date",        "Дата"},
            {"datetime",    "Дата время"},
            {"varchar",     "Текстовый"},
            {"varbinary",   "Бинарный"}
        };

    
        public static string InputTypesSelectControlAttribute
        {
            get
            {
                string result = "";
                foreach(var key in INPUTTYPES.Keys)
                {
                    result += $",{key}";
                }
                return result.Substring(1);
            }
        }
        public static Dictionary<string, string> INPUTTYPES = new Dictionary<string, string>() {
            {"number",      "Числа"},
            {"text",        "Текст"},
            {"password",    "Пароль"},
            {"email",       "Электронная почта"},
            {"url",         "URL"},
            {"file",        "Файл"},
            {"color",       "Цвет"},
            {"image",       "Изображение"},
            {"icon",        "Иконка"},
        };



    
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputBoolAttribute.cs **************/
    public class InputBoolAttribute : InputTypeAttribute{
        public InputBoolAttribute() : base(InputTypes.Custom) { }     
        public override string Validate(object model, string property, object value)
        {
            return (value != null && value is bool) ? null:
                "Тип данных свойства ввода задан некорректно";
        }
        public override string GetMessage(object model, string property, object value)
        {
            return "Тип данных свойства ввода задан некорректно";
        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputColorAttribute.cs **************/
    public class InputColorAttribute : InputTypeAttribute, MyValidation
    {
        private readonly string _error;
        public InputColorAttribute() : base(InputTypes.Color) { }
        public InputColorAttribute( string error ): base(InputTypes.Color)
        {
            _error = error;
        }
        public override string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(_error))
            {
                return "Значение не удовлетворяет условию:  "+ "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
            }
            else
            {
                return _error;
            }
        }
        public override string Validate(object model, string property, object value)
        {        
            if(Regex.Match(value.ToString(), "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", RegexOptions.IgnoreCase).Success == false)
            {
                return GetMessage(model,property,value);
            }
            return null;
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputCreditCardAttribute.cs **************/
    public class InputCreditCardAttribute: InputTypeAttribute
    {
        public InputCreditCardAttribute() : base(InputTypes.CreditCard) { }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputCurrencyAttribute.cs **************/
    public class InputCurrencyAttribute : InputTypeAttribute
    {
        public InputCurrencyAttribute() : base(InputTypes.Currency) { }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputCustomAttribute.cs **************/
    public class InputCustomAttribute : InputTypeAttribute
    {
        public InputCustomAttribute() : base(InputTypes.Custom) { }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDurationAttribute.cs **************/
    public class InputDurationAttribute : InputTypeAttribute
    {
        public InputDurationAttribute() : base(InputTypes.Duration) { }

    } 
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputFileAttribute.cs **************/
    public class InputFileAttribute: InputTypeAttribute
    {
        public InputFileAttribute() : base(InputTypes.File) { }
        public InputFileAttribute(string exts) : base(InputTypes.File)
        {

        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputHiddenAttribute.cs **************/
    public class InputHiddenAttribute: Attribute
    {
        public InputHiddenAttribute() { }
        public InputHiddenAttribute(bool value)
        {
        }
    }
    

    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputIconAttribute.cs **************/
    public class InputIconAttribute : InputTypeAttribute
    {
        public static string GetIconValue( object target ){
            object val = GetValueMarkedByAttribute(target, nameof(InputIconAttribute));
            return val != null ? val.ToString(): "";
        }

        private static object GetValueMarkedByAttribute(object target, string v)
        {
            throw new NotImplementedException();
        }

        public InputIconAttribute( ) : base(InputTypes.Icon)
        {

        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputImageAttribute.cs **************/
    public class InputImageAttribute : InputTypeAttribute
    {
        public InputImageAttribute() : base(InputTypes.Image) { }

    } 
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputNumberAttribute.cs **************/
    public class InputNumberAttribute : InputTypeAttribute
    {
        public InputNumberAttribute() : base(InputTypes.Number) { }
        public InputNumberAttribute( string expression = null) : base(InputTypes.Number)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputOrderAttribute.cs **************/
        public class InputOrderAttribute: Attribute
        {
            public readonly int _Order;

            public InputOrderAttribute(int order)
            {
                this._Order = order;
            }
        }
    }

    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputPercentAttribute.cs **************/
    public class InputPercentAttribute : InputTypeAttribute, MyValidation
    {
        private string _error;

        public InputPercentAttribute( ) : base(InputTypes.Percent) { }
        public InputPercentAttribute(string error) : base(InputTypes.Percent)
        {
            _error = error;
        }

        public override string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(_error))
            {
                return "Процент задаётся действительным числом в диапазоне от 0 до 100";
            }
            else
            {
                return _error;
            }
        }

        public override string Validate(object model, string property, object value)
        {        
            int x = int.Parse(value.ToString());
            if (x < 0 || x > 100)
            {
                return GetMessage(model, property, value);
            }
            else
            {
                return null;
            }   
        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputPostalCodeAttribute.cs **************/
    public class InputPostalCodeAttribute : InputTypeAttribute
    {
        public InputPostalCodeAttribute() : base(InputTypes.PostalCode) { }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputTypeAttribute.cs **************/
    public class InputTypeAttribute : DataTypeAttribute, MyValidation
    {
        public int Order { get; set; } = 0;
        public static DataType GetDataType( string type)
        {
            switch (type)
            {                    
                case "Icon": return DataType.Text;
                case "Date": return DataType.Date;
                case "DateTime": return DataType.DateTime;
                case "Time": return DataType.Time;
                case "Duration": return DataType.Duration;
                case "Xml": return DataType.Html;
                case "Phone": return DataType.PhoneNumber;
                case "Currency": return DataType.Currency;
                case "MultilineText": return DataType.MultilineText;
                case "Email": return DataType.EmailAddress;
                case "Password": return DataType.Password;
                case "Url": return DataType.Url;
                case "Image": return DataType.Upload;
                case "CreditCard": return DataType.CreditCard;
                case "PostalCode": return DataType.PostalCode;
                case "File": return DataType.Upload;
                default: 
                    return DataType.Text;
            }
        }


        private static List<string> INPUT_TYPES = null;
        private string _InputType;

        public static List<string> GetInputTypes()
        {
            if(INPUT_TYPES == null)
            {
                INPUT_TYPES = new List<string>();
                typeof(InputTypeAttribute).Assembly.GetTypes().Where(t=>Typing.IsExtendedFrom(t,typeof(InputTypeAttribute).Name)).ToList().ForEach((Type t) => {
                    INPUT_TYPES.Add(t.Name);
                });
            }        
            return INPUT_TYPES;
        }

        public InputTypeAttribute(string InputType): base(GetDataType(InputType))
        {

            _InputType = InputType;
        }

        public string GetCSTypeName()
        {
       
            if (_InputType == InputTypes.Color)
            {
                return "string";
            }
            else if (_InputType == InputTypes.CreditCard)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Currency)
            {
                return "float";
            }
            else if (_InputType == InputTypes.Custom)
            {
                return "string";
            }
            else if (_InputType == InputTypes.PostalCode)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Text)
            {
                return "string";
            }
     
            else if (_InputType == InputTypes.Time)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Url)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Week)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Xml)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Year)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Date)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.DateTime)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Month)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Duration)
            {
                return "long";
            }
            else if (_InputType == InputTypes.Email)
            {
                return "string";
            }
            else if (_InputType == InputTypes.File)
            {
                return "byte[]";
            }
            else if (_InputType == InputTypes.Image)
            {
                return "byte[]";
            }
            else if (_InputType == InputTypes.Icon)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Percent)
            {
                return "int";
            }
            else if (_InputType == InputTypes.Number)
            {
                return "float";
            }
            else if (_InputType == InputTypes.Phone)
            {
                return "string";
            }
            else if (_InputType == InputTypes.MultilineText)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Password)
            {
                return "string";
            }
            else if (_InputType == InputTypes.Text)
            {
                return "string";
            }       
            else if (_InputType == InputTypes.Icon)
            {
                return "string";
            }
            else if (_InputType == InputTypes.PrimitiveCollection)
            {
                return "object[]";
            }
            else if (_InputType == InputTypes.StructureCollection)
            {
                return "object[]";
            }
            else
            {
                throw new Exception("Не удалось определить тип свойства CSharp "+_InputType);
            }
            throw new Exception("Не удалось определить тип свойства CSharp"+_InputType);

        }

        public string GetOracleDataType()
        {
            return GetSqlServerDataType();
        }

        public string GetPostgreDataType()
        {
            return GetSqlServerDataType();
        }

        public string GetMySQLDataType()
        {
            return GetSqlServerDataType();
        }

        public string GetSqlServerDataType()
        {
            if (_InputType == InputTypes.Color)
            {
                return "varchar(20)";
            }
            else if (_InputType == InputTypes.CreditCard)
            {
                return "varchar(40)";
            }
            else if (_InputType == InputTypes.Currency)
            {
                return "float";
            }
            else if (_InputType == InputTypes.Custom)
            {
                return "varchar(max)";
            }
            else if (_InputType == InputTypes.PostalCode)
            {
                return "varchar(80)";
            }
            else if (_InputType == InputTypes.Text)
            {
                return "varchar(max)";
            }

            else if (_InputType == InputTypes.Time)
            {
                return "Time";
            }
            else if (_InputType == InputTypes.Url)
            {
                return "nvarchar(255)";
            }
            else if (_InputType == InputTypes.Week)
            {
                return "Date";
            }
            else if (_InputType == InputTypes.Xml)
            {
                return "nvarchar(max)";
            }
            else if (_InputType == InputTypes.Year)
            {
                return "Date";
            }
            else if (_InputType == InputTypes.Date)
            {
                return "Date";
            }
            else if (_InputType == InputTypes.DateTime)
            {
                return "DateTime";
            }
            else if (_InputType == InputTypes.Month)
            {
                return "Date";
            }
            else if (_InputType == InputTypes.Duration)
            {
                return "long";
            }
            else if (_InputType == InputTypes.Email)
            {
                return "nvarchar(40)";
            }
            else if (_InputType == InputTypes.File)
            {
                return "varbinary(max)";
            }
            else if (_InputType == InputTypes.Image)
            {
                return "varbinary(max)";
            }
            else if (_InputType == InputTypes.Icon)
            {
                return "nvarchar(40)";
            }
            else if (_InputType == InputTypes.Percent)
            {
                return "int";
            }
            else if (_InputType == InputTypes.Number)
            {
                return "float";
            }
            else if (_InputType == InputTypes.Phone)
            {
                return "nvarchar(20)";
            }
            else if (_InputType == InputTypes.MultilineText)
            {
                return "nvarchar(max)";
            }
            else if (_InputType == InputTypes.Text)
            {
                return "nvarchar(80)";
            }
            else    if (_InputType == InputTypes.Password)
            {
                return "nvarchar(80)";
            }
            else if (_InputType == InputTypes.Icon)
            {
                return "nvarchar(40)";
            }
            else if (_InputType == InputTypes.PrimitiveCollection)
            {
                return "blob";
            }
            else if (_InputType == InputTypes.StructureCollection)
            {
                return "blob";
            }
            throw new Exception("Не удалось определить тип данных SQL Server "+_InputType        );
        }

        public virtual string Validate(object model, string property, object value)
        {
            return null;
        }

        public virtual string GetMessage(object model, string property, object value)
        {
            throw new NotImplementedException();
        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputTypes.cs **************/
    public class InputTypes
    {
    
        public static string PrimitiveCollection = "PrimitiveCollection";
        public static string StructureCollection = "StructureCollection";
        public static string Percent = "Percent";
        public static string Number = "Number";
        public static string Custom = "Custom";
        public static string Date = "Date";
        public static string DateTime = "DateTime";
        public static string Time = "Time";
        public static string Duration = "Duration";
        public static string Xml = "Xml";
        public static string Image = "Image";
        public static string PostalCode = "PostalCode";
        public static string CreditCard = "CreditCard";
        public static string Currency = "Currency";
        public static string Icon = "Icon";
        public static string Color = "Color";
        public static string Email = "Email";
        public static string File = "File";
        public static string Month = "Month";
        public static string Password = "Password";    
        public static string Phone = "Phone";
        public static string Url = "Url";
        public static string Week = "Week";
        public static string Year = "Year";
        public static string Text = "Text";
        public static string MultilineText = "MultilineText";
   
    
   
        public static string GetSqlDataType(string _InputType)
        {
            if (_InputType.ToLower() == InputTypes.Color.ToLower())
            {
                return "varchar(20)";
            }
            else if (_InputType.ToLower() == InputTypes.CreditCard.ToLower())
            {
                return "varchar(40)";
            }
            else if (_InputType.ToLower() == InputTypes.Currency.ToLower())
            {
                return "float";
            }
            else if (_InputType.ToLower() == InputTypes.Custom.ToLower())
            {
                return "varchar(max)";
            }
            else if (_InputType.ToLower() == InputTypes.PostalCode.ToLower())
            {
                return "varchar(80)";
            }
            else if (_InputType.ToLower() == InputTypes.Text.ToLower())
            {
                return "varchar(max)";
            }

            else if (_InputType.ToLower() == InputTypes.Time.ToLower())
            {
                return "Time";
            }
            else if (_InputType.ToLower() == InputTypes.Url.ToLower())
            {
                return "nvarchar(255)";
            }
            else if (_InputType.ToLower() == InputTypes.Week.ToLower())
            {
                return "Date";
            }
            else if (_InputType.ToLower() == InputTypes.Xml.ToLower())
            {
                return "nvarchar(max)";
            }
            else if (_InputType.ToLower() == InputTypes.Year.ToLower())
            {
                return "Date";
            }
            else if (_InputType.ToLower() == InputTypes.Date.ToLower())
            {
                return "Date";
            }
            else if (_InputType.ToLower() == InputTypes.DateTime.ToLower())
            {
                return "DateTime";
            }
            else if (_InputType.ToLower() == InputTypes.Month.ToLower())
            {
                return "Date";
            }
            else if (_InputType.ToLower() == InputTypes.Duration.ToLower())
            {
                return "long";
            }
            else if (_InputType.ToLower() == InputTypes.Email.ToLower())
            {
                return "nvarchar(40)";
            }
            else if (_InputType.ToLower() == InputTypes.File.ToLower())
            {
                return "varbinary(max)";
            }
            else if (_InputType.ToLower() == InputTypes.Image.ToLower())
            {
                return "varbinary(max)";
            }
            else if (_InputType.ToLower() == InputTypes.Icon.ToLower())
            {
                return "nvarchar(40)";
            }
            else if (_InputType.ToLower() == InputTypes.Percent.ToLower())
            {
                return "int";
            }
            else if (_InputType.ToLower() == InputTypes.Number.ToLower())
            {
                return "float";
            }
            else if (_InputType.ToLower() == InputTypes.Phone.ToLower())
            {
                return "nvarchar(20)";
            }
            else if (_InputType.ToLower() == InputTypes.MultilineText.ToLower())
            {
                return "nvarchar(max)";
            }
            else if (_InputType.ToLower() == InputTypes.Text.ToLower())
            {
                return "nvarchar(80)";
            }
            else if (_InputType.ToLower() == InputTypes.Password.ToLower())
            {
                return "nvarchar(80)";
            }
            else if (_InputType.ToLower() == InputTypes.Icon.ToLower())
            {
                return "nvarchar(40)";
            }
            else if (_InputType.ToLower() == InputTypes.PrimitiveCollection.ToLower())
            {
                return "blob";
            }
            else if (_InputType.ToLower() == InputTypes.StructureCollection.ToLower())
            {
                return "blob";
            }
            throw new Exception("Не удалось определить тип данных SQL Server " + _InputType);
        }
        }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\NotInputAttribute.cs **************/
    public class NotInputAttribute : Attribute
    {
        public NotInputAttribute(string message=null)
        {

        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputCollection\InputPrimitiveCollectionAttribute.cs **************/
    public class InputPrimitiveCollectionAttribute : InputTypeAttribute
    {


        public InputPrimitiveCollectionAttribute(): base(InputTypes.PrimitiveCollection){
        
        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputCollection\InputStructureCollectionAttribute.cs **************/
    public class InputStructureCollectionAttribute : InputTypeAttribute
    {

        public Type ItemType { get; }

        public InputStructureCollectionAttribute(): base(InputTypes.StructureCollection){        
        }

        public InputStructureCollectionAttribute(string type) : base(InputTypes.StructureCollection)
        {

            this.ItemType = ReflectionService.TypeForName(type);
            if( ItemType == null)
            {
                throw new Exception("Тип элемент коллекции задан неверно");
            }

        }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputDateAttribute.cs **************/
    public class InputDateAttribute : InputTypeAttribute
    {
        public InputDateAttribute( ) : base(InputTypes.Date)
        {
        
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputDateTimeAttribute.cs **************/
    public class InputDateTimeAttribute : InputTypeAttribute
    {
        public InputDateTimeAttribute( ) : base(InputTypes.DateTime)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputMonthAttribute.cs **************/
    public class InputMonthAttribute : InputTypeAttribute
    {
        public InputMonthAttribute( ) : base(InputTypes.Month)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputTimeAttribute.cs **************/
    public class InputTimeAttribute : InputTypeAttribute
    {
        public InputTimeAttribute( ) : base(InputTypes.Time)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputWeekAttribute.cs **************/
    public class InputWeekAttribute: InputTypeAttribute
    {
        public InputWeekAttribute( ) : base(InputTypes.Week) { }
        public InputWeekAttribute(string exts) : base(InputTypes.Week)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputDate\InputYearAttribute.cs **************/
    public class InputYearAttribute : InputTypeAttribute
    {
        public InputYearAttribute() : base(InputTypes.Year)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputEmailAttribute.cs **************/
    public class InputEmailAttribute : InputTypeAttribute, MyValidation
    {
        private readonly string _message;

        public InputEmailAttribute() : base(InputTypes.Email) { }
        public InputEmailAttribute(string message) : base(InputTypes.Email)
        {
            _message = message;
        }

        public override string Validate(object model, string property, object value)
        {            
            try
            {
                if (Regex.IsMatch(value.ToString(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                    {
                        return null;
                    }
                    else
                    {
                        return GetMessage(model, property, value);
                    }
                }
            catch (RegexMatchTimeoutException)
            {
                return GetMessage(model, property, value);
            }
        }

 
        public override string GetMessage(object model, string property, object value)
        {
            if (_message == null)
            {
                return "Не правильно укаазан адрес электронной почты";
            }
            else
            {
                return _message;
            }
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputMultilineTextAttribute.cs **************/
    public class InputMultilineTextAttribute : InputTypeAttribute
    {
        public InputMultilineTextAttribute() : base(InputTypes.MultilineText) { }
        public InputMultilineTextAttribute(string expression =null): base(InputTypes.MultilineText)
        {
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputPasswordAttribute.cs **************/
    public class InputPasswordAttribute : InputTypeAttribute
    {
        public static string GetPasswordValue( object target ){
            object val = GetValueMarkedByAttribute(target, nameof(InputPasswordAttribute));
            return val != null ? val.ToString(): "";
        }

        private static object GetValueMarkedByAttribute(object target, string v)
        {
            return target.GetType().GetAttrs();
        }

        public InputPasswordAttribute(  ) : base(InputTypes.Password)
        {

        
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputPhoneAttribute.cs **************/
    public class InputPhoneAttribute : InputTypeAttribute, MyValidation
    {
        private string _message;

        public InputPhoneAttribute() : base(InputTypes.Phone) { }
        public InputPhoneAttribute(string message) : base(InputTypes.Phone)
        {
            _message = message;
        }

        private bool isNumber( char ch )
        {
            return "0123456789".IndexOf(ch) != -1;
        }

        public override string Validate(object model, string property, object value)
        {
        
            if (value==null)
            {
                return GetMessage(model,property,value);
            }
            else
            {
                string message = value.ToString();
                if (message.Length != "7-904-334-1124".Length)
                {
                    return GetMessage(model, property, value);
                }
                else
                {
                
                    if (message[1] != '-' || message[5] != '-' || message[9] != '-')
                    {
                        return GetMessage(model, property, value);
                    }
                    else
                    {
                        if (isNumber(message[0]) == false ||
                            isNumber(message[2]) == false || isNumber(message[3]) == false || isNumber(message[4]) == false ||
                            isNumber(message[6]) == false || isNumber(message[7]) == false || isNumber(message[8]) == false ||
                            isNumber(message[10]) == false || isNumber(message[11]) == false ||
                            isNumber(message[12]) == false || isNumber(message[13]) == false)
                        {
                            return GetMessage(model, property, value);
                        }
                    }
                }
            }
            return null;
        }

        public override string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(this._message))
            {
                return "Номер телефона задаётся в формате X-XXX-XXX-XXXX";
            }
            else
            {
                return _message;
            }
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputTextAttribute.cs **************/
    public class InputTextAttribute : InputTypeAttribute, MyValidation
    {
        
        private string _message;

        public InputTextAttribute() : base(InputTypes.MultilineText) { }
        public InputTextAttribute(string message =null): base(InputTypes.MultilineText)
        {
            _message = message;
        }

        public override string GetMessage(object model, string property, object value)
        {
            if(string.IsNullOrEmpty(_message))
            {
                return "Значение свойство не является тестовым значением";
            }
            else
            {
                return _message;
            }

        }

        public override string Validate(object model, string property, object value)
        {
            if( value != null)
            {
                if(value is String == false)
                {
                    return GetMessage(model, property, value);
                }
            }
            return null;
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputUrlAttribute.cs **************/
    public class InputUrlAttribute : InputTypeAttribute, MyValidation
    {
    

        public InputUrlAttribute( string message=null ) : base(InputTypes.Url)
        {
            base.ErrorMessage = message;
        }

        public override string GetMessage(object model, string property, object value)
        {
            return "Некорректно задан URL адрес";
        }

        public override string Validate(object model, string property, object value)
        {
            if (value == null)
            {
                return GetMessage(model, property,value);
            }
            else
            {
                string textValue = value.ToString();

                if(Validation.IsValidUrl(textValue) == false)
                {
                    return GetMessage(model, property, value);
                }
                else
                {
                    return null;
                }            
            }        
        }
    }


    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeInput\InputText\InputXmlAttribute.cs **************/
    public class InputXmlAttribute : InputTypeAttribute
    {
        public InputXmlAttribute() : base(InputTypes.Xml) 
        { 
    }

        

} // end of {ns} 
