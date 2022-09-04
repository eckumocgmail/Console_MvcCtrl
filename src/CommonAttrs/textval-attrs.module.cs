
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using ValidationAnnotationsNS;



namespace Utils{


    /// <summary>
    /// [TextSearch("Title,Description")]
    /// </summary>
    public class TextSearchAttribute : Attribute
    {
    }



    /// <summary>
    /// Проверка уникальности значения атрибута сущности
    /// </summary>
    public class UniqValidationAttribute : Attribute, MyValidation
    {
        private readonly string _error;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ErrorMessage">сообщение после отрицательной проверки</param>    
        public UniqValidationAttribute(string ErrorMessage = null)
        {
            _error = ErrorMessage;
        }


        private object GetISuperSer(object dbContext, string ISuperSer)
        {
            Type type = dbContext.GetType();
            foreach (MethodInfo info in type.GetMethods())
            {
                if (info.Name.StartsWith("get_" + ISuperSer) == true && info.ReturnType.Name.StartsWith("ISuperSer"))
                {
                    return info.Invoke(dbContext, new object[0]);
                }
            }
            throw new Exception("Коллекция сущностей " + ISuperSer + " не найдена");
        }

        public string Validate(object model, string property, object value)
        {
            /*foreach (Type t in AssemblyReader.GetDbContexts(Assembly.GetCallingAssembly()))
            {

                using (DbContext db = ((DbContext)ReflectionService.CreateWithDefaultConstructor<DbContext>(t)))
                {
                    object ISuperSerObj = null;
                    try
                    {
                        ISuperSerObj = db.GetISuperSer( model.GetType().Name);
                    }catch(Exception ex)
                    {
                        continue;
                    }
                    int id = (int)new ReflectionService().GetValue(model, "ID");
                    bool isUniq = true;
                    foreach (var record in ((IEnumerable<dynamic>)ISuperSerObj))
                    {
                        if (record.ID != id)
                        {
                            object recordPropertyValue = new ReflectionService().GetValue(record, property);
                            if (value == null)
                            {
                                if (recordPropertyValue == null)
                                {
                                    isUniq = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (recordPropertyValue == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (recordPropertyValue.ToString() == value.ToString())
                                    {
                                        isUniq = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //bool result = (from i in ((IEnumerable<dynamic>)ISuperSerObj)
                    //where new ReflectionService().GetValue(model, property) == value && i.ID != id
                    //select i).Count() == 0;
                    return isUniq ? null : GetMessage(model,property,value);
                }
            }*/
            return null;

        }

        public string GetMessage(object model, string property, object value)
        {
            if (string.IsNullOrEmpty(_error))
            {
                return "Свойство " + property + " должно иметь уникальное значение";
            }
            else
            {
                return _error;
            }
        }


        /*
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {        
            using (DataContext db = new DataContext())
            {
                string ISuperSerPropertyName = null;
                if(_ISuperSer == null)
                {
                    string entityName = validationContext.ObjectType.Name;
                    ISuperSerPropertyName = Counting.GetMultiCountName(validationContext.ObjectType.Name);
                }
                else
                {
                    ISuperSerPropertyName = _ISuperSer;
                }
                ReflectionService reflection = new ReflectionService();
                object ISuperSerObj = GetISuperSer(db, ISuperSerPropertyName);
                string propertyName = validationContext.MemberName;            
                bool result = (from i in ((IEnumerable<dynamic>)ISuperSerObj)
                               where reflection.GetValue((object)i, propertyName) == value
                               select i).Count() == 0;
                return result? null: new ValidationResult(_error);
            }

        }*/



    }

public class EngTextAttribute: Attribute, MyValidation
{
    /// <summary>
    /// Сообщение в случае возникновения исключения
    /// </summary>
    private readonly string _message;


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="message"> сообщение в случае возникновения исключения </param>
    public EngTextAttribute( string message="" )
    {
        _message = message;
    }


    /// <summary>
    /// Получение значения по имени свойства объекта
    /// </summary>
    /// <param name="target">ссылка на целевой оъект</param>
    /// <param name="property">имф свойства</param>
    /// <returns>значение свойства</returns>
    private object GetValue(object target, string property)
    {
        PropertyInfo propertyInfo = target.GetType().GetProperty(property);
        FieldInfo fieldInfo = target.GetType().GetField(property);
        return
            fieldInfo != null ? fieldInfo.GetValue(target) :
            propertyInfo != null ? propertyInfo.GetValue(target) :
            null;

    }

    public string GetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_message))
        {
            return "Значение может содержать только буквы латинского алфавита";
        }
        else
        {
            return _message;
        }
    }

    public string Validate(object model, string property, object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }
        else
        {
          
            string alf = "qwertyuiopasdfghjklzxcvbnm" + " "+"qwertyuiopasdfghjklzxcvbnm".ToUpper();
            string text = GetValue(model, property).ToString();
            for (int i = 0; i < text.Length; i++)
            {
                if (!alf.Contains(text[i]))
                {
                    return GetMessage(model,property,value);
                }
            }
            return null;
        }
    }
}
    
public class RusTextAttribute: Attribute, MyValidation
{
    
    private readonly string _message;

    public RusTextAttribute(string message= "Значение может содержать только буквы русского алфавита")
    {
        _message = message;
    }

    public string GetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_message))
        {
            return "Значение может содержать только буквы русского алфавита";
        }
        else
        {
            return _message;
        }
    }


    public static object GetValue(object i, string v)
    {
        PropertyInfo propertyInfo = i.GetType().GetProperty(v);
        FieldInfo fieldInfo = i.GetType().GetField(v);
        return
            fieldInfo != null ? fieldInfo.GetValue(i) :
            propertyInfo != null ? propertyInfo.GetValue(i) :
            null;
    }


    /// <summary>
    /// Проверка теста на наличие инностранных литералов
    /// </summary>
    /// <param name="model"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public string Validate(object model, string property, object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }
        else
        {
            string alf = "абвгджеёжзйиклмнпорстуфхцчшщъыьэюя"+" .,1234567890"+ "абвгджеёжзйиклмнпорстуфхцчшщъыьэюя".ToUpper()+" ";
            string text = GetValue(model, property).ToString();
            for (int i=0; i<text.Length; i++)
            {
                if (!alf.Contains(text[i]))
                {
                    return GetMessage(model,property,value);
                }
            }
            return null;
        }
    }

    public static bool IsRus(string word)
    {
        return Regex.Match(word, "/^[а-яА-ЯёЁ]+$/", RegexOptions.IgnoreCase).Success;
    }
}
    
public class TextLengthAttribute: Attribute, MyValidation
{
    private readonly int _min;
    private readonly int _max;
    private readonly string _message;

    public TextLengthAttribute(int min, int max, string message)
    {
        if( min < 0 || min > max)
        {
            throw new Exception("Значения атрибута TextLengthAttribute заданы некорректно");
        }
        _min = min;
        _max = max;
        _message = message;
    }

    public string GetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(this._message))
        {
            return "Миниманое кол-во символов "+_min+" максимальное "+_max;
        }
        else
        {
            return _message;
        }
    }

    public string Validate(object model, string property, object value)
    {
        if( value == null)
        {
            return GetMessage(model,property,value);
        }
        else
        {
            if( value.ToString().Length>=_min && value.ToString().Length <= _max)
            {
                return null;
            }
            else
            {
                return GetMessage(model, property, value);
            }
        }
    }
}
    

} // end of {ns} 
