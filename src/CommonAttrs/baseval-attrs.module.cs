using COM;
using DetailsAnnotationsNS;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ValidationAnnotationsNS{

    public class SocketOptions : ConsoleLogger 
    {
        [Key]
        public int ID { get; set; }
        public string host { get; set; } = "127.0.0.1";
        public int port { get; set; } = 8888;
    }


    /// <summary>
    /// ��
    /// </summary>
    public interface ObjectWithID
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }



    /// <summary>
    /// �������� ������������
    /// </summary>
    public class MatchAttribute : Attribute, MyValidation
    {
        private string _expression;
        private readonly string _message;

        public MatchAttribute(string expression, string message)
        {
            _expression = expression;
            _message = message;

        }

        public static object GetValue(object key, string value)
        {
            PropertyInfo propertyInfo = key.GetType().GetProperty(value);
            FieldInfo fieldInfo = key.GetType().GetField(value);
            return
                fieldInfo != null ? fieldInfo.GetValue(key) :
                propertyInfo != null ? propertyInfo.GetValue(key) :
                null;

        }

        public string Validate(object model, string property, object value)
        {
            if (!Regex.Match(GetValue(model, property).ToString(), _expression, RegexOptions.IgnoreCase).Success)
            {
                return GetMessage(model, property, value);
            }
            else
            {
                return null;
            }
        }

        public string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(_message))
            {
                return "�������� �� ����������� ��������� " + this._expression;
            }
            else
            {
                return _message;
            }
        }
    }


    [EntityLabel("�������� ����������� ����������")]
    [DetailsAnnotationsNS.Description("��� ��������� ����������� �������� ������������� ����������," +
        "������� ������� � ����. ������������� ����������� �������� ������� ������ � �����������.")]
    public class DictionaryIsValidated : MyValidatableObject, MyValidation
    {

        public string Dictionary { get; set; }


        public string Property { get; set; }

        public DictionaryIsValidated()
        {

        }


        public string GetMessage(object model, string property, object value)
        {
            throw new NotImplementedException();
        }

        public string Validate(object model, string property, object value)
        {
            throw new NotImplementedException();
        }
    }



    public class NeedCompareAttribute : ValidationAttribute, MyValidation
    {
        private readonly string _property;
        private readonly string _message;

        public NeedCompareAttribute(string property, string message)
        {
            _property = property;
            _message = message;
        }

        public string GetMessage(object model, string property, object value)
        {
            return _message;
        }

        public string Validate(object model, string property, object value)
        {
            object value2 = model.GetType().GetProperty(_property).GetValue(model);
            if (value != null && value2 != null)
            {
                if (value.ToString() != value2.ToString())
                    return GetMessage(model, property, value);
            }
            return null;
        }
    }
    public class NotNullNotEmptyAttribute : RequiredAttribute, MyValidation
    {
        private readonly string _errorMessage;

        public NotNullNotEmptyAttribute() : base()
        {
            _errorMessage = "�� ����������� ������������ �������� ";
        }
        public NotNullNotEmptyAttribute(string errorMessage) : base()
        {
            ErrorMessage = _errorMessage = errorMessage;
        }
        public NotNullNotEmptyAttribute(string errorMessage, string b) : base()
        {
            ErrorMessage = _errorMessage = errorMessage;
        }

        public string Validate(object model, string property, object value)
        {
            if (value != null && value is byte[])
            {
                return null;
            }
            

            if (value is DateTime)
            {
                if (((DateTime)value).Year == 1 && ((DateTime)value).Month == 1 && ((DateTime)value).Day == 1)
                {
                    return _errorMessage;
                }
            }
            if (value == null || value.ToString().Trim() == "")
                return _errorMessage;
            return null;
        }

        public string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return "�������� " + property + " ���������� ���������� ��� �������������� ��������";
            }
            else
            {
                return ErrorMessage;
            }
        }
    }

   

    public class RemoteValidationAttribute : MyValidation
    {
        private readonly string _uri;

        private static string Parse(string uri, int number)
        {
            try
            {
                string[] ids = uri.Split("/");
                return ids[number];
            }
            catch (Exception ex)
            {
                Writing.ToConsole("������ ��� ������ URI=" + uri + ": " + ex.Message);
                throw new Exception("������ ��� ������ URI=" + uri + ": " + ex.Message, ex);
            }
        }


        public RemoteValidationAttribute(string uri)
        {
            string controller = Parse(uri, 1);
            string action = Parse(uri, 2);
            _uri = uri;
        }

        public string Validate(object model, string property, object value)
        {
            var http = new HttpClient();
            var resp = http.GetAsync(this._uri).Result;
            resp.EnsureSuccessStatusCode();
            string responseText = resp.Content.ReadAsStringAsync().Result;
            var dictiopnary = FromJson<Dictionary<string, object>>(responseText);
            if (dictiopnary.Count() > 0)
            {
                string error = "";
                foreach (var p in dictiopnary)
                {
                    error += p.Value + "\n";
                }
                return error;
            }
            else
            {
                return null;
            }



        }

        private T FromJson<T>(string responseText)
        {
            return JsonConvert.DeserializeObject<T>(responseText);
        }

        public string GetMessage(object model, string property, object value)
        {
            return $"�� ��������� ������� ��������� ������ {model.GetType().Name} ��� �������� {property}";
        }
    }

    public abstract class BaseValidationAttribute : ValidationAttribute, MyValidation
    {
        public BaseValidationAttribute() : base()
        {

        }
        public abstract string Validate(object model, string property, object value);
        public abstract string GetMessage(object model, string property, object value);
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string result = this.Validate(validationContext.ObjectInstance, validationContext.MemberName, value);
            if (result == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(result);
            }

        }
    }


    public interface MyValidation
    {
        public string Validate(object model, string property, object value);
        public string GetMessage(object model, string property, object value);
    }



    public abstract class ThrowableSource: SocketOptions, IDisposable
    {
        private static List<ThrowableSource> Stack = new List<ThrowableSource>();
        public static List<ThrowableSource> GetInvokationStack() => Stack.ToList();

        protected ThrowableSource()
        {
            Stack.Add(this);
            base.ConsoleLoggerName = 1 + "";
        }

        public IDisposable[] GetStack()
        {
            return Stack.Select(item => (IDisposable)item).ToArray();
        }

        public abstract IDictionary GetData();
        public void ThrowData(string message)
        {
            throw new Exception(message)
            {
                HelpLink = "www.openid.com"
            };
        }
         

        public virtual void Dispose()
        {
            Stack.Remove(this);
        }
    }

    


    public class MyValidatableObject : ThrowableSource, IValidatableObject
    {

        [JsonIgnore]
        [NotMapped()]
        [NotInput()]
        public Dictionary<string, List<string>> ModelState { get; set; }


        public MyValidatableObject()
        {
             
        }

        /// <summary>
        /// true, ���� ��� ������� ����������� �� ���������
        /// </summary> 
        public bool IsExtendedFrom(string baseType)
        {
            Type typeOfObject = new object().GetType();
            Type p = base.GetType();
            while (p != typeOfObject)
            {
                if (p.Name == baseType)
                {
                    return true;
                }
                p = p.BaseType;
            }
            return false;
        }


        /// <summary>
        /// ���������� �������� �������
        /// </summary>   
        public Dictionary<string, List<string>> ValidateProperties(string[] keys)
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            foreach (string key in keys)
            {
                List<string> errors = ValidateProperty(key);
                if (errors.Count > 0)
                {
                    results[key] = errors;
                }
            }

            return results;
        }

        /// <summary>
        /// ���������� �������� ������ �������
        /// </summary>   
        public List<string> ValidateProperty(string key)
        {
            
            List<string> errors = new List<string>();
            var attributes = GetType().GetProperty(key).CustomAttributes;

            foreach (var data in this.GetType().GetProperty(key).GetCustomAttributesData())
            {
                if (data.AttributeType.GetInterfaces().Contains(typeof(MyValidation)))
                {
                    List<object> args = new List<object>();
                    foreach (var a in data.ConstructorArguments)
                    {
                        args.Add(a.Value);
                    }
                    MyValidation validation =
                        ReflectionService.Create<MyValidation>(data.AttributeType, args.ToArray());
                    object value = new ReflectionService().GetValue(this, key);
                    string validationResult =
                        validation.Validate(this, key, value);
                    if (validationResult != null)
                    {

                        errors.Add(validationResult);
                    }
                }
            }
            return errors;
        }

        private object ForProperty(Type type, string key)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// �������� ������ ��������� ���������� ��� �� ���������� �����������
        /// </summary>
        public void EnsureIsValide()
        {
            var r = Validate();
            if (r.Count() > 0)
            {
                string message = "";
                foreach (var p in r)
                {
                    string propertyErrorsText = "";
                    p.Value.ForEach((e) => { propertyErrorsText += e + ", "; });
                    message += $"\t{p.Key}={propertyErrorsText}\n";
                }
                throw new ValidationException($"������ " + base.GetType().Name +
                    " �������� �� ���������� ������: \n" +
                    message);
            }
        }


        /// <summary>
        /// ��������� � ��������� MVC
        /// </summary> 
        public IEnumerable<ValidationResult> OnValidating(ValidationContext validationContext)
        {            
            List<ValidationResult> results = new List<ValidationResult>();
            Dictionary<string, List<string>> errors = Validate();
            foreach (var errorEntry in errors)
            {
                string propertyName = errorEntry.Key;
                List<string> propertyErrors = errorEntry.Value;
                foreach (string propertyError in propertyErrors)
                {
                    ValidationResult result = new ValidationResult(propertyError, new List<string>() { propertyName });
                    results.Add(result);
                }
            }
            return results;
        }

        public virtual object GetValue(string key)
        {
            return ReflectionService.GetValueFor(this, key);
        }

        public virtual void SetValue(string key, object value)
        {
            PropertyInfo prop = this.GetType().GetProperty(key);
            if (prop != null)
            {
                prop.SetValue(this, value);
            }
            FieldInfo field = this.GetType().GetField(key);
            if (field != null)
            {
                field.SetValue(this, value);
            }
        }





        public string GetValidationState(string Property)
        {
            return ModelState == null || ModelState.ContainsKey(Property) == false ? "valid" : "invalid";
        }

        

        /// <summary>
        /// ��������� ������ �� �������� ����������� ����� ��������
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, List<string>> Validate()
        {
            object target = this;
            ModelState = new Dictionary<string, List<string>>();
            foreach (var property in target.GetType().GetProperties())
            {
                string key = property.Name;

                if (Typing.IsPrimitive(property.PropertyType))
                {
                    List<string> errors = ValidateProperty(key);
                    if (errors.Count > 0)
                    {
                        ModelState[key] = errors;
                    }

                }
            }
            var optional = ValidateOptional();
            foreach (var p in optional)
            {
                if (ModelState.ContainsKey(p.Key))
                {
                    ModelState[p.Key].AddRange(optional[p.Key]);
                }
                else
                {
                    ModelState[p.Key] = optional[p.Key];
                }
            }


            return ModelState;
        }

        [JsonIgnore]
        [NotInput]
        protected Dictionary<string, List<string>> CustomValidations = new Dictionary<string, List<string>>();



        

    

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, List<string>> ValidateOptional()
        {

            foreach(var kv in CustomValidations)
            {
                object instance = this;
                string name = kv.Key;
                foreach(var validation in kv.Value)
                {
                  

                    
                    var context = new Dictionary<string, object>() 
                    {
                        //this.GetProperty(name);
                    };
                     
                     
                }
           
                
                
            }
            return new Dictionary<string, List<string>>();
        }


        /// <summary>
        /// 
        /// </summary>       
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            try
            {                                
                return OnValidating(validationContext);                
            }
            catch(Exception ex)
            {
                return new List<ValidationResult>() 
                { 
                    new ValidationResult(ex.Message)
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDictionary GetData()
        {
            return this.ToDictionaryOfText(); 
        }
    }



} // end of {ns} 
