namespace COM

{
    using AttributeEntityNS;


    using DetailsAnnotationsNS;
     

    using InputAttrsNS;
 
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using global::Utils;

    using ValidationAnnotationsNS;
    using CommonModule;
    using eckumoc_common_api.CommonCollections;
    using ApplicationCore.Converter.Models;


    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Converter.cs **************/
    public class Converter
    {
        public static string ReplaceAll(string text, string s1, string s2)
        {
            while (text.IndexOf(s1) != -1)
            {
                text = text.Replace(s1, s2);
            }
            return text;
        }

        public static string ToHtmlText(string text)
        {
            string result = "";
            foreach (string line in text.Split("\n"))
            {
                result += $"<div>{line}</div>";
            }
            return result;
        }

        public static string TransliteToLatine(string rus)
        {
            string latine = "";
            for (int i = 0; i < rus.Length; i++)
            {
                latine += map(rus[i]);
            }
            return latine;
        }

        private static string map(char v)
        {
            switch ((v + "").ToLower())
            {
                case "а": return "a";
                case "б": return "b";
                case "в": return "v";
                case "г": return "g";
                case "д": return "d";
                case "е": return "e";
                case "ё": return "yo";
                case "ж": return "j";
                case "з": return "z";
                case "и": return "i";
                case "й": return "yi";
                case "к": return "k";
                case "л": return "l";
                case "м": return "m";
                case "н": return "n";
                case "о": return "o";
                case "п": return "p";
                case "р": return "r";
                case "с": return "s";
                case "т": return "t";
                case "у": return "u";
                case "ф": return "f";
                case "х": return "h";
                case "ц": return "c";
                case "ш": return "sh";
                case "щ": return "sh";
                case "ъ": return "'";
                case "ы": return "u";
                case "ь": return "'";
                case "э": return "a";
                case "ю": return "y";
                case "я": return "ya";
                default: throw new Exception("Не удалось транслировать сообщение");
            }
        }
    }



    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Expression.cs **************/
    public class Expression
    {

        /// <summary>
        /// Разбор тектового выражения
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static List<string> Parse(string exp)
        {
            List<string> expressions = new List<string>();
            while (exp.IndexOf("{{") >= 0)
            {
                int x2 = exp.IndexOf("}}");
                int x1 = exp.IndexOf("{{");
                expressions.Add(exp.Substring(x1 + 2, x2 - x1 - 2));
                exp = exp.Substring(x2 + 2);
            }
            expressions.ForEach((p) => { System.Console.WriteLine(p); });
            return expressions;
        }



        /// <summary>
        /// Преобразование строки форму пригодную для функции string.Format
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string ToFormatable(string exp)
        {
            string formatableString = "";
            List<string> expressions = new List<string>();
            int ctn = 0;
            while (exp.IndexOf("{{") >= 0)
            {
                int x2 = exp.IndexOf("}}");
                int x1 = exp.IndexOf("{{");
                expressions.Add(exp.Substring(x1 + 2, x2 - x1 - 2));
                formatableString += exp.Substring(0, x1) + "{" + ctn + "}";
                ctn++;
                exp = exp.Substring(x2 + 2);
            }
            formatableString += exp;
            return formatableString;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string Interpolate(string exp, object p)
        {
            List<object> paramsList = new List<object>();
            List<string> expressions = Parse(exp);
            foreach (string s in expressions)
            {
                object value = Compile(s, p);
                paramsList.Add(value);
            }
            //while (exp.IndexOf("{{") > 0) exp = exp.Replace("{{", "{");
            //while (exp.IndexOf("}}") > 0) exp = exp.Replace("}}", "}");
            string formatingString = ToFormatable(exp);
            return string.Format(formatingString, paramsList.ToArray());


        }

        public static object Compile(string exp, object p)
        {
            var value = p;
            if (value.GetType().IsExtendsFrom("BaseEntity"))
            {
                value.GetType().GetMethod("JoinAll").Invoke(value, new object[0]);
            }
            foreach (string operation in exp.Trim().Split("."))
            {
                if (operation.IndexOf("(") == -1)
                {
                    string ssexpression = operation.Trim();
                    if (IsLiteral(ssexpression))
                    {
                        value = GetLiteral(ssexpression);
                    }
                    else
                    {
                        value = GetPropertyOrFieldValue(value, ssexpression);
                    }
                }
                else
                {
                    string actionName = operation.Substring(0, operation.IndexOf("("));
                    int x1 = operation.IndexOf("(");
                    int x2 = operation.LastIndexOf(")");
                    string paramsStr = operation.Substring(x1 + 1, x2 - x1 - 1).Trim();
                    List<object> args = new List<object>();
                    if (paramsStr.Length > 0)
                    {
                        var arr = new List<string>();

                        string temp = "";
                        foreach (string s in paramsStr.Split(","))
                        {
                            if (s.Trim().StartsWith("'") && !s.Trim().EndsWith("'"))
                            {
                                temp += s.Substring(1);
                            }
                            else if (!s.Trim().StartsWith("'") && s.Trim().EndsWith("'"))
                            {
                                temp += ',' + s.Substring(0, s.Length - 1);
                                arr.Add("'" + temp + "'");
                                temp = "";
                            }
                            else if (s.Trim().StartsWith('"') && !s.Trim().EndsWith('"'))
                            {
                                temp += s.Substring(1);
                            }
                            else if (!s.Trim().StartsWith('"') && s.Trim().EndsWith('"'))
                            {
                                temp += ',' + s.Substring(0, s.Length - 1);
                                arr.Add("'" + temp + "'");
                                temp = "";
                            }
                            else
                            {
                                
                                arr.Add(s);
                            }
                        }

                        foreach (string s in arr)
                        {
                            string sarg = s.Trim();
                            if (string.IsNullOrEmpty(sarg))
                            {
                                throw new Exception("Строка параметров считана с ошибками. параметр не может иметь имя длиной 0 символов");
                            }
                            args.Add(Compile(sarg, value));
                        }
                    }

                    value = Execute(value, actionName, args);

                }

            }


            return value;

        }

        private static object GetLiteral(string ssexpression)
        {
            if (Validation.IsNumber(ssexpression[0] + ""))
            {
                return int.Parse(ssexpression);
            }
            else
            {
                if (ssexpression[0] == '"')
                {
                    int i2 = ssexpression.Substring(1).IndexOf('"');
                    return ssexpression.Substring(1, i2);
                }
                else if ((ssexpression[0] + "") == "'")
                {
                    int i2 = ssexpression.Substring(1).IndexOf("'");
                    return ssexpression.Substring(1, i2);
                }
                else
                {
                    throw new Exception("Компиляция выражения: " + ssexpression + " на настоящий момент не возможна");
                }
            }
            throw new Exception("Компиляция выражения: " + ssexpression + " на настоящий момент не возможна");
        }

        private static bool IsLiteral(string ssexpression)
        {
            if (string.IsNullOrEmpty(ssexpression))
            {
                throw new Exception("Текст выражения пуст");
            }
            if (Validation.IsNumber(ssexpression[0] + "") || ssexpression[0] == '"' || (ssexpression[0] + "") == "'")
            {
                return true;
            }
            //TODO: дописать проверку является ли выражение литералом
            return false;

        }

        private static object Execute(object value, string actionName, List<object> args)
        {
            var method = value.GetType().GetMethod(actionName);
            if (method == null)
            {
                throw new Exception("Не удалось найти метод с именем: " + actionName + " для типа " + value.GetType().Name);
            }
            else
            {

                return method.Invoke(value, args.ToArray());
                //return InvokeHelper.Do(value, actionName, args);
            }
        }

        private static object GetPropertyOrFieldValue(object value, string operation)
        {
            var property = value.GetType().GetProperty(operation);
            if (property != null)
            {
                return property.GetValue(value);
            }
            var field = value.GetType().GetField(operation);
            if (field != null)
            {
                return field.GetValue(value);
            }
            throw new Exception("Обьект типа" + value.GetType().Name + " не имеет свойства " + operation);
        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Expressions.cs **************/
    public class Expressions
    {


        public static HashSet<string> GetKeywords(IEnumerable<object> items, string entity, string query)
        {
            HashSet<string> keywords = new HashSet<string>();
            List<string> terms = AttrUtils.GetSearchTerms(entity);
            foreach (var p in items.ToList())
            {
                foreach (string term in terms)
                {
                    object val = ReflectionService.GetValueFor(p, term);
                    if (val != null)
                    {
                        foreach (string s in val.ToString().Split(" "))
                        {
                            keywords.Add(s);
                        }
                    }
                }
            }
            return keywords;
        }

        public static HashSet<object> Search(IEnumerable<object> items, string entity, string query)
        {
            HashSet<object> results = new HashSet<object>();
            List<string> terms = AttrUtils.GetSearchTerms(entity);
            Func<object, bool> verify = Expressions.ArePropertiesContainsText(terms, query);

            foreach (var p in items.ToList())
            {
                if (verify(p))
                {
                    results.Add(p);
                }
            }
            return results;
        }

        public static string GetUniqTextExpressionFor(Type type)
        {
            return GetUniqTextExpressionFor(type, "");
        }


        public static string GetUniqTextExpressionFor(Type type, string prefix)
        {
            string expression = "";
            var attributes = AttrUtils.ForAllPropertiesInType(type);
            string uniqProperty = AttrUtils.GetUniqProperty(attributes);

            if (uniqProperty == null)
            {
                foreach (string propertyName in ReflectionService.GetPropertyNames(type))
                {
                    if (Typing.IsPrimitive(type, propertyName))
                    {
                        expression += "{{" + prefix + propertyName + "}} ";
                    }
                }

                if (expression.Length > 0)
                {
                    expression = expression.Substring(0, expression.Length - 1);
                }
            }
            else
            {
                return "{{" + prefix + uniqProperty + "}}";
            }
            return "" + expression + "";
        }


        /// <summary>
        /// Проверка наличия значений свойств в тексте
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Func<object, bool> ArePropertiesContainsText(List<string> properties, string text)
        {
            return (p) =>
            {
                if (string.IsNullOrEmpty(text))
                {
                    return true;
                }
                foreach (var prop in properties)
                {

                    object val = p.GetType().GetProperty(prop).GetValue(p);
                    if (val != null)
                    {
                        bool validation = val.ToString().ToLower().IndexOf(text) != -1;
                        if (validation) return true;
                    }
                }
                return false;
            };
        }

        public static object GetDefaultBindingsFor(string entity)
        {
            return new Dictionary<string, string>() {
            { "Title", GetUniqTextExpressionFor(ReflectionService.TypeForName(entity)) }
        };

        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Files.cs **************/
    public class Files
    {
        public static void CopyCatalog(string from, string to)
        {
            if (System.IO.Directory.Exists(to) == false)
                System.IO.Directory.CreateDirectory(to);
            foreach (string file in System.IO.Directory.GetFiles(from))
            {
                System.IO.File.Copy(file, file.Replace(from, to), true);
            }
            foreach (string dir in System.IO.Directory.GetDirectories(from))
            {
                string path = dir.Replace(from, to);
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);
                Files.CopyCatalog(dir, path);
            }
        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Http.cs **************/
    public static class Http
    {
        /// <summary>
        /// Скачивание избражения с ресурса доступого по URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> DownloadImage(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] ms = await response.Content.ReadAsByteArrayAsync();


            return ms;
        }
    }


    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Random.cs **************/
    public class Randomizing
    {
        private static Random R = new Random();
        public static int GetRandomInt(int from, int to)
        {
            R.NextDouble();
            R.NextDouble();
            return (int)(Math.Floor(R.NextDouble() * (to - from)) + from);
        }

        public static dynamic GetRandomDate(DateTime now, DateTime dateTime)
        {
            TimeSpan uTimeSpan = (now - dateTime);
            return now.AddSeconds(Randomizing.GetRandomInt(0, uTimeSpan.Seconds - 1));
        }
    }






    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\ReflectionService.cs **************/
    public class ReflectionService
    {
        private static HashSet<string> PrimitiveTypeNames = Typing.PRIMITIVE_TYPES;
        public static ConcurrentDictionary<string, Type> SHORT_NAME_TYPE_DICTIONARY = new ConcurrentDictionary<string, Type>();

        public static List<System.Reflection.PropertyInfo> GetPropertiesList(Type target)
        {
            return new List<System.Reflection.PropertyInfo>(target.GetProperties());
        }

        public static List<string> GetOwnMethodNames(Type type)
        {
            return (from p in new List<MethodInfo>((type).GetMethods()) where p.DeclaringType == type select p.Name).ToList();
        }

        public static List<string> GetOwnPublicMethodNames(Type type)
        {
            return (from p in new List<MethodInfo>((type).GetMethods()) where p.IsStatic==false && p.IsPublic==true && p.DeclaringType == type select p.Name).ToList();
        }

        public static bool IsPrimitive(Type type)
        {
            return IsPrimitive(Typing.ParsePropertyType(type));
        }

        public static bool IsPrimitive(string typeName)
        {
            return PrimitiveTypeNames.Contains(typeName);
        }

        public static List<string> GetPublicStaticFieldNames(Type type)
        {
            List<string> fieldNames = new List<string>();
            foreach (var field in type.GetFields())
            {
                if (field.IsPublic && field.IsStatic)
                {
                    fieldNames.Add(field.Name);
                }
            }
            return fieldNames;
        }

        public static object GetOwnProperties(object p, string type = "")
        {
            IDictionary<string, object> options = new Dictionary<string, object>();

            GetOwnPropertyNames("ViewOptions").ForEach(n => {
                options[n] = p.GetType().GetProperty(n).GetValue(p);
            });
            return options.ToJson();
        }

        public static object CopyValuesFromDictionary(object searchRequest, IDictionary<string, object> dictionary)
        {
            ReflectionService.GetOwnPropertyNames(searchRequest).ForEach(p => {
                if (dictionary.ContainsKey(p))
                    Setter.SetValue(searchRequest, p, dictionary[p]);

            });
            return searchRequest;
        }

        private static HashSet<string> ObjectMethods = new HashSet<string>() {
            "GetHashCode", "Equals", "ToString", "GetType", "ReferenceEquals" };

        public static List<object> Values(dynamic item, List<string> columns)
        {
            int ctn = 0;
            object[] values = new object[columns.Count()];
            foreach (string col in columns)
            {
                values[ctn++] = new ReflectionService().GetValue(item, col);
            }
            return new List<object>(values);
        }


        /// <summary>
        /// Копирование свойств обьекта
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        public void Copy(object item, object target)
        {
            Type type = target.GetType();
            while (type != null)
            {
                foreach (FieldInfo field in type.GetFields())
                {
                    if (field.GetValue(item) !=
                        target.GetType().GetField(field.Name).GetValue(target))
                    {
                        object current,
                                prev = target.GetType().GetField(field.Name);
                        target.GetType().GetField(field.Name).SetValue(target, current = field.GetValue(item));
                        object evt = new
                        {
                            prev = prev,
                            current = current

                        };
                    }
                }
                type = type.BaseType;
            }
        }

        public static List<string> GetOwnPropertyNames(object type)
        {
            if (type is Type)
                return (from p in new List<PropertyInfo>(((Type)type).GetProperties()) where p.DeclaringType == ((Type)type) select p.Name).ToList();
            else
                return (from p in new List<PropertyInfo>(type.GetType().GetProperties()) where p.DeclaringType == type.GetType() select p.Name).ToList();
        }

        public static List<string> GetPropertyNames(Type type)
        {
            var list = (from p in new List<PropertyInfo>(type.GetProperties()) select p.Name).ToList();
            list.Reverse();
            return list;
        }
        public static List<string> GetFieldNames(Type type)
        {
            return (from p in new List<FieldInfo>(type.GetFields()) select p.Name).ToList();
        }

        public static void CopyValues(object item, object target)
        {

            foreach (string propertyName in GetPropertyNames(target.GetType()))
            {
                var itemProperty = item.GetType().GetProperty(propertyName);
                if (itemProperty != null)
                {
                    object value = itemProperty.GetValue(item);
                    target.GetType().GetProperty(propertyName).SetValue(target, value);
                }
            }
            foreach (string fieldName in GetFieldNames(target.GetType()))
            {
                var itemField = item.GetType().GetField(fieldName);
                if (itemField != null)
                {
                    object value = itemField.GetValue(item);
                    target.GetType().GetField(fieldName).SetValue(target, value);
                }
            }
        }

        public ReflectionService( )
        {

        }


        /// <summary>
        /// Список аргументов вызова метода
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static List<string> GetArguments(MethodInfo method)
        {
            List<string> args = new List<string>();
            foreach (ParameterInfo pinfo in method.GetParameters())
            {
                args.Add(pinfo.Name);
            }
            return args;
        }

        public static Type TypeForName(string typeName)
        {
            if (typeName == "string") return typeof(String);
            if (typeName == "String") return typeof(String);
            if (typeName == "Int32") return typeof(int);
            if (typeName == "Int64") return typeof(long);
            if (typeName == "Boolean") return typeof(Boolean);
            if (typeName == "DateTime") return typeof(DateTime);
            if (typeName == "Decimal") return typeof(float);
            if (typeName == "String[]") return typeof(String[]);
            if (typeName == "Int32[]") return typeof(int[]);
            if (typeName == "Int64[]") return typeof(long[]);
            if (typeName == "Boolean[]") return typeof(Boolean[]);
            if (typeName == "DateTime[]") return typeof(DateTime[]);
            if (typeName == "Decimal[]") return typeof(float[]);
            if (typeName == "JObject") return typeof(JObject);
            if (typeName == "JArray") return typeof(JArray);
            if (typeName == "JToken") return typeof(JToken);
            if (typeName == "JValue") return typeof(JValue);
            if (typeName.Contains(".") == false)
            {
                return TypeForShortName(typeName);
            }
            Type t = (from p in Assembly.GetExecutingAssembly().GetTypes() where p.FullName == typeName select p).SingleOrDefault();
            if (t == null)
            {
                t = (from p in Assembly.GetCallingAssembly().GetTypes() where p.FullName == typeName select p).SingleOrDefault();
            }
            if (t == null)
            {
                throw new Exception("Не найден тип " + typeName);
            }
            return t;
        }

        public static Type TypeForShortName(string type)
        {
            if (SHORT_NAME_TYPE_DICTIONARY.ContainsKey(type))
            {
                return SHORT_NAME_TYPE_DICTIONARY[type];
            }
            else
            {
                Type t;
                try
                {
                    t = (from p in Assembly.GetExecutingAssembly().GetTypes() where p.Name == type select p).SingleOrDefault();
                    if (t == null)
                    {
                        t = (from p in Assembly.GetCallingAssembly().GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    /*if (t == null)
                    {
                        t = (from p in typeof(IAuthorizationService).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }


                    if (t == null)
                    {
                        t = (from p in typeof(IAuthenticationService).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }

                    if (t == null)
                    {
                        t = (from p in typeof(System.Buffers.IPinnable).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }

                    if (t == null)
                    {
                        t = (from p in typeof(LinkGenerator).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(ObjectPoolProvider).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(IInlineConstraintResolver).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(IServer).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(IConnectionListenerFactory).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }

                    if (t == null)
                    {
                        t = (from p in typeof(IHttpContextFactory).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(DiagnosticListener).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }

                    if (t == null)
                    {
                        t = (from p in typeof(ILoggerFactory).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }

                    if (t == null)
                    {
                        t = (from p in typeof(IConfiguration).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }
                    if (t == null)
                    {
                        t = (from p in typeof(Microsoft.Extensions.Options.OptionsValidationException).Assembly.GetTypes() where p.Name == type select p).SingleOrDefault();
                    }*/
                }
                catch (Exception)
                {
                    Writing.ToConsole($"Обнаружено несколько типов с именем {type}");
                    Writing.ToConsole(Assembly.GetExecutingAssembly().GetTypes().Where(p => p.Name == type).Select(t => t.FullName));
                    throw new Exception($"Обнаружено несколько типов с именем {type}");
                }
                if (t == null)
                {
                    throw new Exception("Не удалось найти тип " + type);
                }
                SHORT_NAME_TYPE_DICTIONARY[type] = t;
                return t;
            }
        }

        public void copyFromDictionary(object model, IDictionary<string, object> dictionaries)
        {
            foreach (var prop in model.GetType().GetProperties())
            {
                if (dictionaries.ContainsKey(prop.Name))
                {
                    prop.SetValue(model, dictionaries[prop.Name]);
                }
            }
        }

        public static T Create<T>(string typeName, object[] vs)
        {
            Type type = null;
            if (typeName.Contains("."))
            {
                type = ReflectionService.TypeForName(typeName);
            }
            else
            {
                type = ReflectionService.TypeForShortName(typeName);
            }
            return Create<T>(type, vs);
        }


        public static T Create<T>(Type type, object[] vs)
        {
            ConstructorInfo constructor = (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == vs.Length select c).FirstOrDefault();
            return (T)constructor.Invoke(vs);
        }

        /// <summary>
        /// Создание новоги экземпляра класса конструктором по-умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T CreateWithDefaultConstructor<T>(string typeName)
        {
            Type type = null;
            if (typeName.Contains("."))
            {
                type = ReflectionService.TypeForName(typeName);
            }
            else
            {
                type = ReflectionService.TypeForShortName(typeName);
            }
            return CreateWithDefaultConstructor<T>(type);
        }


        /// <summary>
        /// Создание новоги экземпляра класса конструктором по-умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T CreateWithDefaultConstructor<T>(Type type)
        {
            ConstructorInfo constructor = GetDefaultConstructor(type);
            if (constructor == null)
            {
                throw new Exception($"Тип {type.Name} не обьявляет контруктор по-умолчанию");
            }
            return (T)constructor.Invoke(new object[0]);
        }


        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>s
        /// <returns></returns>
        public List<MessageAttribute > GetProperties( object item )
        {
            List<MessageAttribute > props = new List<MessageAttribute >();
            {             
                foreach (var prop in db.GetEntityProperties(item.GetType()))
                {                 
                    string type = null;
                    object val = new ReflectionService().GetValue(item, prop.Name);

                    if(val!=null)
                    switch (val.GetType().Name.ToLower())
                    {
                        case "bool":
                        case "boolean":
                            type = "checkbox";
                            break;
                        case "string":
                        case "text":
                            type = "text";
                            break;
                        case "int":
                        case "float":
                        case "double":
                        case "decimal":
                        case "int32":
                        case "int64":
                            type = "text";
                            break;
                        case "date":
                            type = "date";
                            break;
                        case "datetime":
                            type = "datetime";
                            break;
                    }

                    var attributes = Utils.GetForProperty(item.GetType(), prop.Name);
                    var dataType = Utils.GetDataType(attributes);
                    if (dataType != null)
                    {
                        type = dataType.ToString().ToLower();
                    }

                    props.Add(new MessageAttribute (attributes)
                    {

                        Label = db.GetDisplayName(item.GetType(), prop.Name),
                        Name = prop.Name,
                        Value = new ReflectionService().GetValue(item, prop.Name),
                        State = "valid",
                        Type = type
                    });

                }
                return props;
            }
        }*/


        /// <summary>
        /// Копирование свойств обьекта
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        public void copy(object item, object target)
        {
            foreach (FieldInfo field in target.GetType().GetFields())
            {
                if (field.GetValue(item) !=
                    target.GetType().GetField(field.Name).GetValue(target))
                {
                    object current,
                            prev = target.GetType().GetField(field.Name);
                    target.GetType().GetField(field.Name).SetValue(target, current = field.GetValue(item));
                    object evt = new
                    {
                        prev = prev,
                        current = current

                    };
                }
            }
            foreach (PropertyInfo field in target.GetType().GetProperties())
            {
                if (field.GetValue(item) !=
                    target.GetType().GetProperty(field.Name).GetValue(target))
                {
                    object current,
                            prev = target.GetType().GetField(field.Name);
                    target.GetType().GetProperty(field.Name).SetValue(target, current = field.GetValue(item));
                    object evt = new
                    {
                        prev = prev,
                        current = current

                    };
                }
            }
        }

        public static object GetValueFor(object i, string v)
        {

            try
            {
                PropertyInfo propertyInfo = i.GetType().GetProperty(v);
                FieldInfo fieldInfo = i.GetType().GetField(v);
                return
                    fieldInfo != null ? fieldInfo.GetValue(i) :
                    propertyInfo != null ? propertyInfo.GetValue(i) :
                    null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении значения свойства {v} из объекта типа {i.GetType().Name} ", ex);
            }
        }

        public object GetValue(object i, string v)
        {
            try
            {
                PropertyInfo propertyInfo = i.GetType().GetProperty(v);
                FieldInfo fieldInfo = i.GetType().GetField(v);
                return
                    fieldInfo != null ? fieldInfo.GetValue(i) :
                    propertyInfo != null ? propertyInfo.GetValue(i) :
                    null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении значения свойства {v} из объекта типа {i.GetType().Name} ", ex);
            }
        }

        public static IDictionary<string, object> GetSkeleton(object api)
        {
            return GetSkeleton(api, new List<string>());
        }

        /**
         * Метод получения семантики public-методов обьекта
         */
        public static IDictionary<string, object> GetSkeleton(object subject, List<string> path)
        {

            IDictionary<string, object> actionMetadata = new Dictionary<string, object>();
            if (subject == null || subject.GetType().IsPrimitive || PrimitiveTypeNames.Contains(subject.GetType().Name))
            {
                return actionMetadata;
            }
            else
            {
                if (subject is IDictionary<string, object>)
                {
                    foreach (var kv in ((Dictionary<string, object>)subject))
                    {
                        actionMetadata[kv.Key] = kv.Value;
                        if (!kv.Value.GetType().IsPrimitive && !PrimitiveTypeNames.Contains(kv.Value.GetType().Name))
                        {

                            List<string> childPath = new List<string>(path);
                            childPath.Add(kv.Key);
                            actionMetadata[kv.Key] = GetSkeleton(kv.Value, childPath);
                        }
                    };
                }
                else
                {
                    //Debug.WriteLine(JObject.FromObject(subject));
                    Type type = subject.GetType();
                    //Debug.WriteLine(type.Name, path);
                    foreach (MethodInfo info in type.GetMethods())
                    {
                        if (info.IsPublic && !ObjectMethods.Contains(info.Name))
                        {
                            IDictionary<string, object> args = new Dictionary<string, object>();
                            foreach (ParameterInfo pinfo in info.GetParameters())
                            {
                                args[pinfo.Name] = new
                                {
                                    type = pinfo.ParameterType.Name,
                                    optional = pinfo.IsOptional,
                                    name = pinfo.Name
                                };
                            }
                            List<string> actionPath = new List<string>(path);
                            actionPath.Add(info.Name);
                            actionMetadata[info.Name] = new
                            {
                                type = "method",
                                path = actionPath,
                                args = args
                            };
                        }
                    }
                    foreach (FieldInfo info in type.GetFields())
                    {
                        if (info.IsPublic)
                        {
                            if (!info.GetType().IsPrimitive && !PrimitiveTypeNames.Contains(info.GetType().Name))
                            {
                                List<string> childPath = new List<string>(path);
                                childPath.Add(info.Name);
                                actionMetadata[info.Name] = GetSkeleton(info.GetValue(subject), childPath);
                            }
                        }
                    }
                }
            }

            return actionMetadata;
        }


        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            return (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == 0 select c).SingleOrDefault();
        }


        public IDictionary<string, object> GetStaticMethods(Type type)
        {
            IDictionary<string, object> actionMetadata = new Dictionary<string, object>();
            foreach (MethodInfo info in type.GetMethods())
            {
                if (info.IsPublic && info.IsStatic)
                {
                    IDictionary<string, object> args = new Dictionary<string, object>();
                    foreach (ParameterInfo pinfo in info.GetParameters())
                    {
                        args[pinfo.Name] = new
                        {
                            type = pinfo.ParameterType.Name,
                            optional = pinfo.IsOptional,
                            name = pinfo.Name
                        };
                    }
                }
            }
            return actionMetadata;
        }

        /// <summary>
        /// <button>ok</button>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<string> GetOwnPublicMethodsNames(Type type)
        {
            return (from m in new List<MethodInfo>(type.GetMethods())
                    where m.IsPublic &&
                            !m.IsStatic &&
                            m.DeclaringType.FullName == type.FullName
                    select m.Name).ToList<string>();
        }


        /// <summary>
        /// <button>ok</button>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<MethodInfo> GetOwnPublicMethods(Type type)
        {
            return (from m in new List<MethodInfo>(type.GetMethods())
                    where m.IsPublic &&
                            !m.IsStatic &&
                            m.DeclaringType.FullName == type.FullName
                    select m).ToList<MethodInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetMethodParameters(MethodInfo method)
        {
            IDictionary<string, object> args = new Dictionary<string, object>();
            foreach (ParameterInfo pinfo in method.GetParameters())
            {
                args[pinfo.Name] = new
                {
                    type = pinfo.ParameterType.Name,
                    optional = pinfo.IsOptional,
                    name = pinfo.Name
                };
            }
            return args;
        }


        public static object Invoke(MethodInfo method, object target, JObject args)
        {
            IDictionary<string, string> pars = JsonConvert.DeserializeObject<Dictionary<string, string>>(args.ToString());
            return Execute(method, target, pars);
        }

        public static object Execute(MethodInfo method, object target, IDictionary<string, string> pars)
        {
            string state = "Поиск обьекта: ";
            List<object> invArgs = null;
            try
            {
                invArgs = new List<object>();
                foreach (ParameterInfo pinfo in method.GetParameters())
                {
                    if (pinfo.IsOptional == false && pars.ContainsKey(pinfo.Name) == false)
                    {
                        throw new Exception("require argument " + pinfo.Name);
                    }
                    string parameterName = pinfo.ParameterType.Name;

                    if (parameterName.StartsWith("Dictionary"))
                    {
                        IDictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(pars[pinfo.Name].ToString());
                        invArgs.Add(dictionary);
                    }
                    else
                    {
                        invArgs.Add(pars[pinfo.Name]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("ArgumentsException: " + ex.Message, ex);
            }


            try
            {

                object result = method.Invoke(target, invArgs.ToArray());
                state = state.Substring(0, state.Length - 7) + "успех;";
                return result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error in controller function: " + ex.Message);
                throw;
            }
        }




        /// <summary>
        /// Поиск метода 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IDictionary<string, Object> Find(object subject, string path)
        {
            object p = subject;
            string[] ids = path.Split('.');
            for (int i = 0; i < (ids.Length - 1); i++)
            {
                string id = ids[i];
                if (p is IDictionary<string, object>)
                {
                    p = ((Dictionary<string, object>)p)[id];
                }
                else if (p is ConcurrentDictionary<string, object>)
                {
                    p = ((ConcurrentDictionary<string, object>)p)[id];
                }
                else
                {
                    p = p.GetType().GetField(id).GetValue(p);
                }
            }

            MethodInfo info = null;
            string methodName = ids[ids.Length - 1];

            foreach (var method in p.GetType().GetMethods())
            {
                if (String.Equals(methodName, method.Name))
                {
                    info = method;
                    break;
                }
            }
            IDictionary<string, Object> res = new Dictionary<string, Object>();
            res["method"] = info;
            res["target"] = p;
            res["path"] = path;


            return res;
        }





        public string GetMethodParametersBlock(MethodInfo method)
        {
            string s = "{";
            bool needTrim = false;
            foreach (var pair in GetMethodParameters(method))
            {
                needTrim = true;
                s += pair.Key + ':' + pair.Key + ",";
            }
            if (needTrim == true)
                return s.Substring(0, s.Length - 1) + "}";
            else
            {
                return s + "}";
            }
        }


        public string GetMethodParametersString(MethodInfo method)
        {
            bool needTrim = false;
            string s = "";
            foreach (var p in GetMethodParameters(method))
            {
                needTrim = true;
                s += p.Key + ",";// +":"+ p.Value + ",";
            }
            return needTrim == true ? s.Substring(0, s.Length - 1) : s;
        }


    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Setter.cs **************/
    public class Setter
    {
        public static object FromText(object value, string propertyType)
        {
            switch (propertyType)
            {
                case "String": { return value.ToString(); }
                case "System.String": { return value.ToString(); }
                case "Single": { return System.Single.Parse(value.ToString()); }
                case "System.Single": { return System.Single.Parse(value.ToString()); }
                case "Double": { return System.Double.Parse(value.ToString()); }
                case "System.Double": { return System.Double.Parse(value.ToString()); }
                case "Decimal": { return System.Decimal.Parse(value.ToString()); }
                case "System.Decimal": { return System.Decimal.Parse(value.ToString()); }
                case "Int16": { return System.Int16.Parse(value.ToString()); }
                case "System.Int16": { return System.Int16.Parse(value.ToString()); }
                case "Int32": { return System.Int32.Parse(value.ToString()); }
                case "System.Int32": { return System.Int32.Parse(value.ToString()); }
                case "Nullable<Int32>": { return System.Int32.Parse(value.ToString()); }
                case "Nullable<System.Int32>": { return System.Int32.Parse(value.ToString()); }
                case "Int64": { return System.Int64.Parse(value.ToString()); }
                case "System.Int64": { return System.Int64.Parse(value.ToString()); }
                case "UInt16": { return System.UInt16.Parse(value.ToString()); }
                case "System.UInt16": { return System.UInt16.Parse(value.ToString()); }
                case "UInt32": { return System.UInt32.Parse(value.ToString()); }
                case "System.UInt32": { return System.UInt32.Parse(value.ToString()); }
                case "UInt64": { return System.UInt64.Parse(value.ToString()); }
                case "System.UInt64": { return System.UInt64.Parse(value.ToString()); }
                default:
                    throw new Exception($"Тип  {propertyType} неподдрживается");
            }
        }
        public static void SetValue(object target, string property, object value)
        {
            var p = target.GetType().GetProperty(property);
            if (Typing.IsDateTime(p))
            {
                DateTime? date = value != null ? (DateTime?)value.ToString().ToDate() : null;
                p.SetValue(target, date);
            }
            else if (Typing.IsNumber(p))
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    if (Typing.IsNullable(p) && Typing.IsPrimitive(p.PropertyType) == false)
                    {
                        p.SetValue(target, null);
                    }
                    else
                    {
                        throw new Exception($"Свойство {property} не может хранить ссылку на null");
                    }
                }
                else
                {
                    string propertyType = Typing.ParsePropertyType(p.PropertyType);
                    switch (propertyType)
                    {
                        case "Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                        case "System.Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                        case "Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                        case "System.Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                        case "Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                        case "System.Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                        case "Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                        case "System.Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                        case "Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                        case "System.Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                        case "Nullable<Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                        case "Nullable<System.Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                        case "Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                        case "System.Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                        case "UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                        case "System.UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                        case "UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                        case "System.UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                        case "UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                        case "System.UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                        default:
                            throw new Exception($"Тип свойства {property} {propertyType} неподдрживается");
                    }


                }
                /*if (value != null && (value.GetType().Name == "Int64" || propertyTypeName == "Int32"))
                {
                    value = Int32.Parse(value.ToString());
                }*/
            }
            else if (Typing.IsText(p))
            {
                p.SetValue(target, value.ToString());
            }
            else
            {

                p.SetValue(target, value);
            }

        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Timing.cs **************/
    public class Timing
    {

        private static long timestamp = (long)((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);

        public static long check()
        {
            long now = GetTime();
            long longness = now - timestamp;
            timestamp = now;
            return longness;
        }


        /// <summary>
        /// Получение текущего времени в милисекундах
        /// </summary>
        /// <returns></returns>
        public static long GetTime()
        {
            TimeSpan uTimeSpan = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)uTimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Получение текущего времени в милисекундах
        /// </summary>
        /// <returns></returns>
        public static long GetTime(DateTime date)
        {
            TimeSpan uTimeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)uTimeSpan.TotalMilliseconds;
        }



        /// <summary>
        /// Время начала сегодняшнего дня в миличекундах
        /// </summary>
        /// <returns></returns>
        public static long GetTodayBeginTime()
        {

            TimeSpan uTimeSpan = (DateTime.Today - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)uTimeSpan.TotalMilliseconds;
        }


        /// <summary>
        /// Метод получения наименования месяца в дательном падеже
        /// </summary>
        /// <param name="month">номер месяца</param>
        /// <returns></returns>
        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "января";
                case 2: return "февраля";
                case 3: return "марта";
                case 4: return "апреля";
                case 5: return "мая";
                case 6: return "июня";
                case 7: return "июля";
                case 8: return "августа";
                case 9: return "сентября";
                case 10: return "октября";
                case 11: return "ноября";
                case 12: return "декабря";
                default:
                    {
                        throw new Exception("Месяц задан неверно.");

                    }
            }
        }
    }


    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Typing.cs **************/
    public static class TypieExtes
    {


        public static string GetName(this Type propertyType)
        {
            string name = propertyType.Name.IndexOf("`") == -1 ? propertyType.Name : propertyType.Name.Substring(0, propertyType.Name.IndexOf("`"));
            if (propertyType.GenericTypeArguments != null && propertyType.GenericTypeArguments.Length > 0)
            {
                string suffix = "";
                foreach (var parType in propertyType.GenericTypeArguments)
                {
                    suffix += "," + parType.GetName();
                }
                suffix = suffix.Replace(",", "<") + ">";
                name += suffix;
            }
            return name;
        }
        public static IDictionary<string, string> GetUtils(this Type type)
        {
            return AttrUtils.ForType(type);
        }
    }

    /// <summary>
    /// Реализует методы работы с типами
    /// </summary>
    public class Typing
    {

        public static HashSet<string> PRIMITIVE_TYPES = new HashSet<string>() {
            "Byte[]", "System.Byte[]", "String", "Boolean", "System.String", "string", "int","long","float",
        "Nullable<System.Boolean>", "Double", "Nullable<System.Double>",
        "Int16", "Nullable<Int16>", "Int32", "Nullable<System.Int32>",
        "Int64", "Nullable<System.Int64>", "UInt16", "UInt32", "UInt64",
        "DateTime", "Nullable<System.DateTime>" };
        public static readonly IEnumerable<string> INPUT_TYPES = new HashSet<string>(ReflectionService.GetPublicStaticFieldNames(typeof(InputTypes)));

        public static readonly IEnumerable<string> NUBMER_TYPES = new HashSet<string>() {
              "System.Decimal",  "Decimal", "Nullable<System.Decimal>", "System.Float",
        "Float", "Nullable<System.Float>", "System.Double",  "Double", "Nullable<System.Double>",
        "Int16", "System.Int16", "Nullable<System.Int16>",
        "Int32", "System.Int32", "Nullable<System.Int32>",
        "Int64", "System.Int64", "Nullable<System.Int64>",
        "UInt16", "System.UInt16", "Nullable<System.UInt16>",
        "UInt32", "System.UInt32", "Nullable<System.UInt32>",
        "UInt64", "System.UInt64", "Nullable<System.UInt64>"  };
        public static readonly IEnumerable<string> TEXT_TYPES = new HashSet<string>() {
            "String,System.String" };
        public static readonly IEnumerable<string> LOGICAL_TYPES = new HashSet<string>() {
            "Boolean","System.Boolean","Nullable<System.Boolean>", };
        public static bool IsExtendedFrom(Type targetType, string baseType)
        {
            Type typeOfObject = new object().GetType();
            Type p = targetType;
            while (p != typeOfObject && p != null)
            {
                if (p.Name == baseType)
                {
                    return true;
                }
                p = p.BaseType;
            }
            return false;
        }


        public static bool IsActiveObject(Type type)
        {
            return IsExtendedFrom(type, "ActiveObject");
        }

        public static bool IsDailyStatsTable(Type type)
        {
            return IsExtendedFrom(type, "DailyStatsTable");
        }

        public static bool IsDictionaryTable(Type type)
        {
            return IsExtendedFrom(type, "DictionaryTable");
        }

        public static bool IsDimensionTable(Type type)
        {
            return IsExtendedFrom(type, "DimensionTable");
        }

        public static bool IsFactsTable(Type type)
        {
            return IsExtendedFrom(type, "EventsTable");
        }

        public static bool IsPublicEntity(Type type)
        {
            return IsExtendedFrom(type, "PublicEntity");
        }

        public static bool IsStatsTable(Type type)
        {
            return IsExtendedFrom(type, "StatsTable");
        }

        public static bool IsWeeklyStatsTable(Type type)
        {
            return IsExtendedFrom(type, "WeeklyStatsTable");
        }

        public static bool IsYearlyStatsTable(Type type)
        {
            return IsExtendedFrom(type, "YearlyStatsTable");
        }



        public static bool IsHierDictinary(Type entityType)
        {
            bool isHier = false;
            Type p = entityType;
            while (p != typeof(Object) && p != null)
            {
                if (p.Name.StartsWith("HierDictionaryTable"))
                {
                    isHier = true;
                    break;
                }
                p = p.BaseType;
            }

            return isHier;
        }
        public static string ParseCollectionType(Type type)
        {
            string text = type.AssemblyQualifiedName;
            text = text.Substring(text.IndexOf("[[") + 2);
            text = text.Substring(0, text.IndexOf(","));
            return text.Substring(text.LastIndexOf(".") + 1);
        }
        public static string ParseCollectionType(Type model, string propertyName)
        {
            return ParseProperty(model, model.GetProperty(propertyName)).Type;
        }


        public static bool HasBaseType(Type targetType, Type baseType)
        {
            if (targetType == null)
                throw new Exception("Тип не определён");
            Type p = targetType.BaseType;
            while (p != typeof(Object) && p != null)
            {
                if (p.Name == baseType.Name)
                {
                    return true;
                }
                p = p.BaseType;
            }
            return false;
        }



        public static bool IsDateTime(PropertyInfo property)
        {
            string propertyType = ParsePropertyType(property.PropertyType);
            if (propertyType == "System.DateTime" || propertyType == "DateTime" || propertyType == "Nullable<DateTime>" || propertyType == "Nullable<System.DateTime>")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullable(PropertyInfo property)
        {
            string propertyType = ParsePropertyType(property.PropertyType);
            return propertyType.StartsWith("Nullable");
        }


        public static List<MyParameterDeclarationModel> ParseActions(Type type)
        {
            List<MyParameterDeclarationModel> props = new List<MyParameterDeclarationModel>();
            foreach (var property in type.GetProperties())
            {
                MyParameterDeclarationModel prop = ParseProperty(type, property);
                props.Add(prop);
            }
            return props;
        }

        public static List<MyParameterDeclarationModel> ParseProperties(Type type)
        {
            List<MyParameterDeclarationModel> props = new List<MyParameterDeclarationModel>();
            foreach (var property in type.GetProperties())
            {
                MyParameterDeclarationModel prop = ParseProperty(type, property);
                props.Add(prop);
            }
            return props;
        }
        public static bool IsCollectionType(Type type)
        {
            Type p = type;
            while (p != typeof(Object) && p != null)
            {
                if ((from pinterface in new List<Type>(p.GetInterfaces()) where pinterface.Name.StartsWith("ICollection") select p).Count() > 0)
                {
                    return true;
                }
                p = p.BaseType;
            }
            return false;
        }
        public static MyParameterDeclarationModel ParseProperty(Type type, PropertyInfo property)
        {
            string TypeName = property.PropertyType.Name;
     
            if (property.PropertyType.Name.StartsWith("List"))
            {
               //IsCollection = true;
                string text = property.PropertyType.AssemblyQualifiedName;
                text = text.Substring(text.IndexOf("[[") + 2);
                text = text.Substring(0, text.IndexOf(","));
                TypeName = text.Substring(text.LastIndexOf(".") + 1);
                Writing.ToConsole(property.Name + " " + text);
            }
            MyParameterDeclarationModel prop = new MyParameterDeclarationModel
            {
                Name = property.Name,
                
             Type = TypeName,
                Attributes = AttrUtils.ForProperty(type, property.Name)
            };
            return prop;
        }

        public static string ParsePropertyType(Type propertyType)
        {
            string name = propertyType.Name;

            if (name.Contains("`"))
            {
                string text = propertyType.AssemblyQualifiedName;
                text = text.Substring(text.IndexOf("[[") + 1);
                text = text.Substring(0, text.IndexOf(","));
                name = name.Substring(0, name.IndexOf("`")) + "<" + text + ">";
            }
            return name;
        }



        /// <summary>
        /// Метод получения описателя вызова статических методов 
        /// </summary>
        /// <param name="type"> тип </param>
        /// <returns> описание статических методов </returns>
        public static IDictionary<string, object> GetStaticMethods(Type type)
        {
            IDictionary<string, object> actionMetadata = new Dictionary<string, object>();
            foreach (MethodInfo info in type.GetMethods())
            {
                if (info.IsPublic && info.IsStatic)
                {
                    IDictionary<string, object> args = new Dictionary<string, object>();
                    foreach (ParameterInfo pinfo in info.GetParameters())
                    {
                        args[pinfo.Name] = new
                        {
                            type = pinfo.ParameterType.Name,
                            optional = pinfo.IsOptional,
                            name = pinfo.Name
                        };
                    }
                }
            }
            return actionMetadata;
        }
        public List<string> GetEventListeners()
        {
            List<string> listeners = new List<string>();
            foreach (EventInfo evt in GetType().GetEvents())
            {
                listeners.Add(evt.Name.ToLower());
            }
            return listeners;
        }
        public static bool IsNumber(PropertyInfo propertyInfo)
        {
            return NUBMER_TYPES.Contains(ParsePropertyType(propertyInfo.PropertyType));
        }

        public static bool IsText(PropertyInfo propertyInfo)
        {
            return TEXT_TYPES.Contains(ParsePropertyType(propertyInfo.PropertyType));
        }

        public static bool IsPrimitive(string propertyType)
        {
            Type type = ReflectionService.TypeForName(propertyType);

            return PRIMITIVE_TYPES.Contains(ParsePropertyType(type));
        }

        public static bool IsPrimitive(Type propertyType)
        {
            return PRIMITIVE_TYPES.Contains(ParsePropertyType(propertyType));
        }

        public static bool IsPrimitive(Type modelType, string property)
        {
            return PRIMITIVE_TYPES.Contains(ParsePropertyType(modelType.GetProperty(property).PropertyType));
        }

        public static bool IsBoolean(PropertyInfo propertyInfo)
        {
            return LOGICAL_TYPES.Contains(ParsePropertyType(propertyInfo.PropertyType));
        }

        public static bool ReferenceIsDictionary(object properties)
        {
            return properties.GetType().Name.Contains("Dictionary");
        }
    }




    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Utils.cs **************/
    public class AttrUtils
    {

        public static IDictionary<string, IDictionary<string, string>> AttrsByType = new Dictionary<string, IDictionary<string, string>>();
        public static IDictionary<string, IDictionary<string, IDictionary<string, string>>> AttrsByMembers = new Dictionary<string, IDictionary<string, IDictionary<string, string>>>();

        public static string IconFor(string type)
        {
            return IconFor(ReflectionService.TypeForName(type));
        }

        /// <summary>
        /// Проверка флага отображением
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsVisible(Type type, string property)
        {
            string hidden = ForPropertyValue(type, typeof(InputHiddenAttribute), property);
            return "True" == hidden ? false : true;
        }




        /// <summary>
        /// Получить значения атрибуf заданного для свойства
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string ForPropertyValue(Type type, Type attr, String property)
        {
            if (type == null)
            {
                throw new Exception("Аргумент " + type + " содержить ссылку на null");
            }
            IDictionary<string, string> attrs = new Dictionary<string, string>();
            if (type == null || type.GetProperty(property) == null)
            {
                throw new Exception("Свойство не найдено либо не задан тип");
            }
            foreach (var data in type.GetProperty(property).CustomAttributes)
            {
                string key = data.AttributeType.Name;
                if (key == attr.Name)
                {
                    foreach (var arg in data.ConstructorArguments)
                    {
                        string value = arg.Value.ToString();
                        return value;
                    }
                }


            }
            return null;
        }


        /// <summary>
        /// Получение атрибута типа поля ввода
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        public static string GetInputType(IDictionary<string, string> attrs)
        {
            if (attrs.ContainsKey("Key") || attrs.ContainsKey("KeyAttribute"))
            {
                return "hidden";
            }
            string key = null;
            List<string> keys = new List<string>(attrs.Keys);
            InputTypeAttribute.GetInputTypes().ForEach((string name) =>
            {
                if (keys.Contains(name))
                {
                    key = name;
                }
            });
            if (key != null)
            {
                return key.Replace("Attribute", "").Replace("Input", "");
            }
            else
            {

                return null;
            }
        }




        /// <summary>
        /// Подпись элемента визуализации ассоциированного со заданным свойством 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetInputType(Type type, string name)
        {
            IDictionary<string, string> attrs = ForProperty(type, name);
            return GetInputType(attrs);
        }

        public static string IconFor(Type type)
        {
            IDictionary<string, string> attrs = ForType(type);
            return attrs.ContainsKey(nameof(IconAttribute)) ? attrs[nameof(IconAttribute)] :
                attrs.ContainsKey(nameof(EntityIconAttribute)) ? attrs[nameof(EntityIconAttribute)] :
                null;
        }
        public static IDictionary<string, string> ForMethod(Type controllerType, string name)
        {
            IDictionary<string, string> attrs = new Dictionary<string, string>();
            foreach (var method in controllerType.GetMethods())
            {
                foreach (var data in method.GetCustomAttributesData())
                {
                    string key = data.AttributeType.Name;
                    foreach (var arg in data.ConstructorArguments)
                    {
                        string value = arg.Value.ToString();
                        attrs[key] = value;
                    }

                }
            }
            return attrs;
        }

        public static IDictionary<string, IDictionary<string, string>> GetAttributesByMemberForType(Type type)
        {
            type.GetProperties().ToList().Select(p => p.Name).ToList().ForEach((name) => {
                ForProperty(type, name).ToJsonOnScreen().WriteToConsole();
            });
            return AttrsByMembers[type.Name];
        }

        public static IEnumerable<string> GetRefTypPropertiesNames(Type type)
        {
            return type.GetProperties().ToList().Where(p => Typing.IsPrimitive(p.PropertyType) == false).Select(p => p.Name);
        }

        /// <summary>
        /// Возвращаент имя свойства помеченного заданным атриюбутом
        /// </summary>
        /// <param name="target"></param>
        /// <param name="nameOfInputTypeAttribute"></param>
        /// <returns></returns>
        public static object GetValueMarkedByAttribute(object target, string nameOfInputTypeAttribute)
        {
            return ForAllPropertiesInType(target.GetType()).Where(p => p.Value.ContainsKey(nameOfInputTypeAttribute)).Select(p => p.Key).Single();

        }


        public static IDictionary<string, IDictionary<string, string>> ForAllPropertiesInType(Type type)
        {
            IDictionary<string, IDictionary<string, string>> result = new Dictionary<string, IDictionary<string, string>>();
            foreach (var prop in type.GetProperties())
            {
                IDictionary<string, string> forProperty = ForProperty(type, prop.Name);
                result[prop.Name] = forProperty;
            }
            return result;
        }

        public static Attribute[] ForPropertyLikeAttrubtes(Type type, string property)
        {
            var attrs = new List<Attribute>();
            if (type == null)
            {
                throw new ArgumentNullException();
            }



            PropertyInfo info = type.GetProperty(property);
            if (info == null)
            {
                throw new Exception($"Свойство {property} не найдено в обьекте типа {type.Name}");
            }
            foreach (var data in info.GetCustomAttributesData())
            {
                string key = data.AttributeType.Name;

                if (data.ConstructorArguments == null || data.ConstructorArguments.Count == 0)
                {
                    Attribute attr = ReflectionService.Create<Attribute>(key, new object[0]);
                    attrs.Add(attr);
                }
                else
                {
                    List<object> parameters = new List<object>();
                    foreach (CustomAttributeTypedArgument arg in data.ConstructorArguments)
                    {
                        parameters.Add(arg.Value);
                    }
                    Attribute attr = ReflectionService.Create<Attribute>(key, parameters.ToArray());
                    attrs.Add(attr);

                }

                //model.Attributes[data.AttributeType] = null;

            }
            return attrs.ToArray();
        }

        public static List<string> SearchTermsForType(Type p)
        {
            List<string> terms = new List<string>();
            IDictionary<string, string> attrs = ForType(p);
            if (attrs.ContainsKey(nameof(SearchTermsAttribute)))
            {
                terms.AddRange(attrs[nameof(SearchTermsAttribute)].Split(","));
            }
            return terms;
        }
        public static List<string> GetSearchTerms(string entity)
        {
            Type entityType = ReflectionService.TypeForShortName(entity);
            List<string> terms = SearchTermsForType(entityType);
            return terms;
        }

        /// <summary>
        /// Подпись элемента визуализации ассоциированного со заданным свойством 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string LabelFor(Type model, string name)
        {
            IDictionary<string, string> attrs = ForProperty(model, name);
            return attrs.ContainsKey(nameof(LabelAttribute)) ? attrs[nameof(LabelAttribute)] :
                attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : name;
        }


        /// <summary>
        /// Получение значения атрибута для текста надписи
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LabelFor(Type type)
        {
            IDictionary<string, string> attrs = ForType(type);
            return attrs.ContainsKey(nameof(EntityLabelAttribute)) ? attrs[nameof(EntityLabelAttribute)] :
                attrs.ContainsKey(nameof(LabelAttribute)) ? attrs[nameof(LabelAttribute)] : null;
        }

        public static string DescriptionFor(Type type, string property)
        {
            IDictionary<string, string> attrs = ForProperty(type, property);
            return attrs.ContainsKey(nameof(ClassDescriptionAttribute)) ? attrs[nameof(ClassDescriptionAttribute)] :
                attrs.ContainsKey(nameof(DescriptionAttribute)) ? attrs[nameof(DescriptionAttribute)] : null;
        }


        /// <summary>
        /// Получение атрибутов для обьекта
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ForObject(object p)
        {

            return ForType(p.GetType());
        }


        public static bool IsInput(Type type, string name)
        {
            return IsInput(type, name);
        }
        public static bool IsInput(Dictionary<string, string> attrs)
        {
            return attrs.ContainsKey(nameof(NotInputAttribute)) ? false : true;
        }

        public static string LabelFor(object p)
        {
            IDictionary<string, string> attrs = ForObject(p);
            return attrs.ContainsKey(nameof(EntityLabelAttribute)) ? attrs[nameof(EntityLabelAttribute)] :
                attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : p.GetType().Name;
        }

        public static string DescriptionFor(object p)
        {
            IDictionary<string, string> attrs = ForObject(p);
            return attrs.ContainsKey(nameof(DescriptionAttribute)) ? attrs[nameof(DescriptionAttribute)] :
                    attrs.ContainsKey(nameof(ClassDescriptionAttribute)) ? attrs[nameof(ClassDescriptionAttribute)] : "";
        }

        public static string IconFor(Type type, string property)
        {
            IDictionary<string, string> attrs = ForProperty(type, property);
            return attrs.ContainsKey(nameof(IconAttribute)) ? attrs[nameof(IconAttribute)] : "person";

        }
        /// <summary>
        /// Получить значения всех атрибутов заданных для свойства
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ForProperty(Type type, String property)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            if (AttrsByMembers.ContainsKey(type.Name) == false)
            {
                AttrsByMembers[type.Name] = new Dictionary<string, IDictionary<string, string>>();
            }
            if (AttrsByMembers[type.Name].ContainsKey(property))
            {
                return AttrsByMembers[type.Name][property];
            }

            IDictionary<string, string> attrs =
                AttrsByMembers[type.Name][property] =
                    new Dictionary<string, string>();
            PropertyInfo info = type.GetProperty(property);
            if (info == null)
            {
                throw new Exception($"Свойство {property} не найдено в обьекте типа {type.Name}");
            }
            foreach (var data in info.GetCustomAttributesData())
            {

                string key = data.AttributeType.Name;
                //ParameterInfo[] pars = data.AttributeType.GetConstructors()[0].GetParameters();
                if (data.ConstructorArguments == null || data.ConstructorArguments.Count == 0)
                {
                    attrs[key] = "";
                }
                else
                {
                    foreach (var arg in data.ConstructorArguments)
                    {

                        string value = arg.Value == null ? "" : arg.Value.ToString();
                        attrs[key] = value;
                    }
                }

                //model.Attributes[data.AttributeType] = null;

            }
            return attrs;
        }






        public static IDictionary<string, string> ForType(Type p)
        {
            if (AttrsByType.ContainsKey(p.Name))
            {
                return AttrsByType[p.Name];
            }
            else
            {
                IDictionary<string, string> attrs = new Dictionary<string, string>();
                foreach (var data in p.GetCustomAttributesData())
                {
                    string key = data.AttributeType.Name;
                    foreach (var arg in data.ConstructorArguments)
                    {
                        string value = arg.Value.ToString();
                        attrs[key] = value;
                    }

                }
                AttrsByType[p.Name] = attrs;
                return attrs;
            }
        }



        /// <summary>
        /// Извлечение метода HTTP из атрибутов
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string ParseHttpMethod(IDictionary<string, string> attributes)
        {
            foreach (var p in attributes)
            {
                switch (p.Key)
                {
                    case "HttpPostAttribute":
                        return "GET";
                    case "HttpPutAttribute":
                        return "PUT";
                    case "HttpPatchAttribute":
                        return "PATCH";
                    case "HttpDeleteAttribute":
                        return "DELETE";
                    default: return "GET";
                }
            }
            return "GET";
        }
        public static bool IsUniq(IDictionary<string, string> attributes)
        {
            return attributes.ContainsKey(nameof(UniqValidationAttribute));
        }
        public static bool IsUniq(Type t, string p)
        {
            return ForProperty(t, p).ContainsKey(nameof(UniqValidationAttribute));
        }

        public static string GetUniqProperty(IDictionary<string, IDictionary<string, string>> attrs)
        {
            foreach (var p in attrs)
            {
                if (IsUniq(attrs[p.Key]))
                {
                    return p.Key;
                }
            }
            return null;
        }

        public MyControllerModel CreateModel(Type controllerType)
        {
            var uri = "/";
            var attrs = AttrUtils.ForType(controllerType);
            if (attrs.ContainsKey("AreaAttribute")) uri += attrs["AreaAttribute"].ToString() + "/";
            if (attrs.ContainsKey("ForRoleAttribute")) uri += attrs["ForRoleAttribute"].ToString() + "/";
            
            string path = AttrUtils.ParseHttpMethod(attrs);
            Writing.ToConsole(path);
            MyControllerModel model = new MyControllerModel()
            {
                Name = controllerType.Name.Replace("`1", ""),
                Path = path,
                Actions = new CommonDictionary< MyActionModel>()
            };
            while (controllerType != null)
            {
                foreach (string name in ReflectionService.GetOwnMethodNames(controllerType))
                {
                    MethodInfo method = controllerType.GetMethod(name);
                    if (method.IsPublic && method.Name.StartsWith("get_") == false && method.Name.StartsWith("set_") == false)
                    {

                        IDictionary<string, string> attributes = AttrUtils.ForMethod(controllerType, method.Name);
                        IDictionary<string, object> pars = new Dictionary<string, object>();
                        model.Actions[method.Name] = new MyActionModel()
                        {
                            Name = method.Name,
                            Attributes = attributes,
                            Method = AttrUtils.ParseHttpMethod(attributes),
                            Parameters = new Dictionary<string, MyParameterDeclarationModel>(),
                            Path = model.Path + "/" + method.Name
                        };
                        foreach (ParameterInfo par in method.GetParameters())
                        {
                            model.Actions[method.Name].Parameters[par.Name] = new MyParameterDeclarationModel()
                            {
                                Name = par.Name,
                                Type = par.ParameterType.Name,
                                IsOptional = par.IsOptional
                            };
                        }
                    }
                }
                controllerType = controllerType.BaseType;
            }
            return model;
        }



        public static string GetRouteAttribute(Type type)
            => ForType(type).ContainsKey("RouteAttribute") ? ForType(type)["RouteAttribute"] :
               ForType(type).ContainsKey("Route") ? ForType(type)["Route"] :
                type.IsExtendsFrom("Controller") ? $"{type.Name}": $"/api/{type.GetName()}";


        public static object ForAllMethodsInType(Type type)
        {
            
            var result = new Dictionary<string, IDictionary<string, string>>();
            ReflectionService.GetOwnMethodNames(type).ForEach(name => 
            {
                try
                {
                    var info = type.GetMethods().Where(m => m.Name == name).FirstOrDefault();
                    result[name] = AttrUtils.ForMethod(type, name);
                }
                catch(AmbiguousMatchException ex)
                {
                     

                    Writing.ToConsole(
                        $"{ex}"+
                        $"{type.Name} имеет несколько реализаций метода {name} " +
                        $"{type.GetMethods().Where(m => m.Name == name).Count()}");
                }
            });

            return result;
        }
        
        public static string IconFor(IDictionary<string, string> attributes)
        {
            return "home";
        }
        public static string HelpFor(IDictionary<string, string> attributes)
        {
            return "home";
        }
        public static string[] KeysFor(IDictionary<string, string> attributes)
        {
            return attributes.Keys.ToArray();
        }
        public static MyValidation[] ValidatorFor(Dictionary<string, string> attributes)
        {
            return attributes.Keys.Where(k=>k.ToType().IsExtendsFrom("BaseValidationAttribute")).Select(k=>(MyValidation)k.ToType().New()).ToArray();
        }
    }



    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Integrated\Validation.cs **************/
    public class Validation
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }



        public static bool IsValidUrlSyntax(string url)
        {
            if ((url.ToLower().StartsWith("http://") ||
                    url.ToLower().StartsWith("https://") ||
                        url.ToLower().StartsWith("ftp://") ||
                            url.ToLower().StartsWith("file://")) &&
                url.Substring(url.IndexOf("://")).Length > 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidUrl(string url)
        {


            

            if ((url.ToLower().StartsWith("http://") ||
                    url.ToLower().StartsWith("https://") ||
                        url.ToLower().StartsWith("ftp://") ||
                            url.ToLower().StartsWith("file://")) &&
                url.Substring(url.IndexOf("://")).Length > 4)
            {






                return true;
            }
            else
            {
                return false;
            }
        }






        public static bool IsRus(string word)
        {
            return Regex.Match(word, "/^[а-яА-ЯёЁ]+$/", RegexOptions.IgnoreCase).Success;
        }
        public static bool IsEng( string word)
        {
            string alf = "qwertyuiopasdfghjklzxcvbnm" + " " + "qwertyuiopasdfghjklzxcvbnm".ToUpper();
            string text = word;
            for (int i = 0; i < text.Length; i++)
            {
                if (!alf.Contains(text[i]))
                {
                    return false;
                }
            }
            return true;
        }



        public static bool IsNumber(string text)
        {
            foreach (char ch in text.ToCharArray())
            {
                if (!"0123456789".Contains(ch))
                {
                    return false;
                }
            }
            return true;
        }


    }



    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\BinaryEncoder.cs **************/
    public class BinaryEncoder 
    {

        /// <summary>
        /// Преобразование строки в битовою последовательность
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>битовая последовательность</returns>
        public string ToBinary(string text)
        {
            string binary = "";
            foreach (char ch in text)
            {
                binary += this.ToBinary(ch);
            }
            return binary;
        }


        /// <summary>
        /// Преобразование символа в битовою строку
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>битовая строка</returns>
        public string ToBinary(char ch)
        {
            string binary = "";
            int x = ch;
            while (x > 0)
            {
                int dev = x % 2;
                x = (x - dev) / 2;
                binary = ((dev == 1) ? "1" : "0") + binary;
            }
            while (binary.Length < 8)
            {
                binary = "0" + binary;
            }
            return binary;
        }

        public string ToBinary(byte ch)
        {
            string binary = "";
            int x = ch;
            while (x > 0)
            {
                int dev = x % 2;
                x = (x - dev) / 2;
                binary = ((dev == 1) ? "1" : "0") + binary;
            }
            while (binary.Length < 8)
            {
                binary = "0" + binary;
            }
            return binary;
        }


        /// <summary>
        /// Преобразование битовой строки в символ
        /// </summary>
        /// <param name="binary"> битовая строка </param>
        /// <returns>символ</returns>
        private char ReadChar(string binary)
        {
            int code = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (binary[i] == '1')
                {
                    code += (int)Math.Pow(2, 7 - i);
                }
            }
            System.Diagnostics.Debug.WriteLine(binary + "  " + code + "  " + System.Convert.ToChar(code));
            return System.Convert.ToChar(code);
        }


        /// <summary>
        /// Преобразование битовой строки в символ
        /// </summary>
        /// <param name="binary"> битовая строка </param>
        /// <returns>символ</returns>
        public string FromBinary(string binary)
        {
            string result = "";
            while (binary.Length > 0)
            {
                string charBinaries = binary.Substring(0, Math.Min(8, binary.Length));
                result += this.ReadChar(charBinaries);
                binary = binary.Substring(8);
            }
            return result;
        }
    }



    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\CharacterEncoder.cs **************/
    public class CharacterEncoder :   IComparer<CharacterStats>
    {
        private CharacterStats searchRoot;
        private BinaryEncoder binaryEncoder;


        public CharacterEncoder()
        {
            this.binaryEncoder = new BinaryEncoder();
        }


        public CharacterEncoder(string text)
        {
            this.binaryEncoder = new BinaryEncoder();
            AnalizeStatistics(text);
        }

        protected string ToBinary(byte ch)
        {
            return this.binaryEncoder.ToBinary(ch);
        }


        /// <summary>
        /// Сжатие текстового сообщения на основе кода хаффмана.
        /// </summary>
        /// <param name="text"> текст сообщения </param>
        /// <returns> сжатое сообщение </returns>
        public string Encode(string text)
        {
            AnalizeStatistics(text);
            string binary = "";
            foreach (char ch in text)
            {
                string bits = this.Encode(ch);
                System.Console.WriteLine($"{ch}={bits}");
                binary += bits;
            }
            System.Console.WriteLine($"binary={binary}");
            return FlushEncode(binary);
        }


        /// <summary>
        /// Метод вывода сжатых данных в текстовое сообщение
        /// </summary>
        /// <param name="binary"> бинарный код </param>
        /// <returns> текстовое сообщение </returns>
        private string FlushEncode(string binary)
        {
            binary += "1";
            while (binary.Length % 8 != 0)
            {
                binary += "0";
            }
            return Seriallize() +
                this.binaryEncoder.FromBinary(binary);
        }


        /// <summary>
        /// Метод обратного декодирования сообщения
        /// </summary>
        /// <param name="encode"> закодированное сообщение</param>
        /// <returns> раскодированное сообщение </returns>
        public string Decode(string encode)
        {
            int begin = this.Deseriallize(encode);

            // получение текстового сообщения в бинарном формате
            string encodedText = encode.Substring(begin);
            string binaryCode = this.binaryEncoder.ToBinary(encodedText);
            binaryCode = binaryCode.Substring(0, binaryCode.LastIndexOf("1"));

            System.Console.WriteLine($"binaryCode: {binaryCode}");
            return this.FlushDecode(binaryCode);

        }


        /// <summary>
        /// Обход иерархии хаффмана вывод раскодированных символов
        /// </summary>
        /// <param name="binaryCode"></param>
        /// <returns></returns>
        private string FlushDecode(string binaryCode)
        {
            string text = "";
            CharacterStats pnode = this.searchRoot;
            foreach (char ch in binaryCode)
            {
                if (ch == '0')
                {
                    pnode = pnode.left;
                }
                else
                {
                    pnode = pnode.right;
                }
                if (pnode.text.Length == 1)
                {
                    text += pnode.text;
                    pnode = this.searchRoot;
                }
            }
            return text;
        }


        /// <summary>
        /// Получение бинарного кода символа 
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns> бинарный код </returns>
        private string Encode(char ch)
        {
            string binary = "";
            CharacterStats pnode = this.searchRoot;
            while (pnode.text != (ch + ""))
            {
                if (pnode.left != null && pnode.left.Contains(ch))
                {
                    binary += "0";
                    pnode = pnode.left;
                }
                else
                {
                    binary += "1";
                    pnode = pnode.right;
                }
            }
            return binary;
        }


        /// <summary>
        /// Метод инициаллизации кода Хаффмана по исходному тексту сообщения
        /// </summary>
        /// <param name="text"> текст сообщения </param>
        public void AnalizeStatistics(string text)
        {
            IDictionary<string, CharacterStats> charset = new Dictionary<string, CharacterStats>();
            foreach (char ch in text)
            {
                string code = ch + "";
                if (charset.ContainsKey(code))
                {
                    charset[code].value++;
                }
                else
                {
                    charset[code] = new CharacterStats()
                    {
                        text = code,
                        value = 1
                    };
                }
            }
            AnalizeStatistics(new List<CharacterStats>(charset.Values));
        }


        /// <summary>
        /// Анализ статистики и перестроение структуры символов 
        /// </summary>
        /// <param name="stats"> татистика использования </param>
        private void AnalizeStatistics(List<CharacterStats> stats)
        {
            stats.Sort(this);
            while (stats.Count > 1)
            {
                CharacterStats[] arr = stats.ToArray();
                CharacterStats parent = new CharacterStats()
                {
                    text = arr[1].text + arr[0].text,
                    value = arr[0].value + arr[1].value
                };
                stats.Remove(parent.left = arr[1]);
                stats.Remove(parent.right = arr[0]);
                stats.Add(parent);
                stats.Sort(this);
            }
            this.searchRoot = stats.ToArray()[0];
        }


        /// <summary>
        /// Сериализация в текстовое сообщение
        /// </summary>        
        /// <returns> текстовое сообщение </returns>
        public string Seriallize()
        {
            List<CharacterStats> stats = this.searchRoot.GetLists();
            string text = stats.Count + "";
            foreach (CharacterStats stat in stats)
            {
                text += "|" + stat.text + stat.value;
            }
            return text + "|";
        }


        /// <summary>
        /// Десериализация состояния из закодированного сообщения
        /// </summary>        
        /// <returns> кол-во считанных символов  </returns>
        public int Deseriallize(string text)
        {
            this.searchRoot = null;
            List<CharacterStats> charset = new List<CharacterStats>();

            int before = text.Length;

            // считывание кол-ва уникальных символов
            int indexOfSeparator = text.IndexOf("|");
            int n = int.Parse(text.Substring(0, indexOfSeparator));
            text = text.Substring(indexOfSeparator + 1);
            for (int i = 0; i < n; i++)
            {
                indexOfSeparator = text.IndexOf("|");
                CharacterStats chartStat = new CharacterStats()
                {
                    text = text.Substring(0, 1),
                    value = int.Parse(text.Substring(1, indexOfSeparator - 1))
                };
                charset.Add(chartStat);
                text = text.Substring(indexOfSeparator + 1);
            }
            this.AnalizeStatistics(charset);
            return before - text.Length;
        }


        /// <summary>
        /// Сравнение статистики вхождения символов
        /// </summary>
        /// <param name="x"> символ 1 </param>
        /// <param name="y"> символ 2 </param>
        /// <returns></returns>
        public int Compare(CharacterStats x, CharacterStats y)
        {
            return x.value - y.value;
        }


        /// <summary>
        /// Преобразование в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.searchRoot.ToString();
        }
    }



    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\CharacterStats.cs **************/
    public class CharacterStats
    {
        public string text;
        public int value;

        public CharacterStats left;
        public CharacterStats right;


        public bool Contains(char ch)
        {
            return this.text.IndexOf(ch) != -1;
        }


        /// <summary>
        /// Метод получения узлов не имющих потомков
        /// </summary>
        /// <returns></returns>
        public List<CharacterStats> GetLists()
        {
            List<CharacterStats> lists = new List<CharacterStats>();
            if (this.left == null && this.right == null)
            {
                lists.Add(this);
            }
            else
            {
                if (this.left != null)
                {
                    lists.AddRange(this.left.GetLists());
                }
                if (this.right != null)
                {
                    lists.AddRange(this.right.GetLists());
                }
            }
            return lists;
        }





        public string ToString(int level = 1)
        {
            string intendt = "";
            for (int i = 0; i < level; i++)
            {
                intendt += "    ";
            }
            string result = intendt + $"{this.text}[{this.value}]\n";
            if (this.left != null)
                result += this.left.ToString(level + 1);
            if (this.right != null)
                result += this.right.ToString(level + 1);
            return result;
        }
    }


    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\Counting.cs **************/
    public class Counting
    {
        /// <summary>
        /// Возвращает существительное во множественном числе
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GetMultiCountName(string table)
        {
            //определение наименования в множественном числе и единственном                        
            string tableName = table;
            string multicount_name = null;
            if (tableName.EndsWith("s"))
            {
                if (tableName.EndsWith("ies"))
                {
                    multicount_name = tableName;
                }
                else
                {
                    multicount_name = tableName;
                }
            }
            else
            {
                if (tableName.EndsWith("y"))
                {
                    multicount_name = tableName.Substring(0, tableName.Length - 1) + "ies";
                }
                else
                {
                    multicount_name = tableName + "s";
                }
            }
            return multicount_name;
        }


        /// <summary>
        /// Возвращает существительное в единственном
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSingleCountName(string name)
        {
            //определение наименования в множественном числе и единственном                        
            string tableName = name.Trim();
            string singlecount_name = null;
            if (tableName.EndsWith("s"))
            {
                if (tableName.EndsWith("ies"))
                {

                    singlecount_name = tableName.Substring(0, tableName.Length - 3) + "y";
                }
                else
                {
                    singlecount_name = tableName.Substring(0, tableName.Length - 1);
                }
            }
            else
            {
                if (tableName.EndsWith("y"))
                {

                    singlecount_name = tableName;

                }
                else
                {
                    singlecount_name = tableName;
                }
            }
            return singlecount_name;
        }
    }


    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\Naming.cs **************/
    public class Naming : Counting
    {
        private static string SPEC_CHARS = ",.?~!@#$%^&*()-=+/\\[]{}'\";:\t\r\n";
        private static string RUS_CHARS = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ" + "ёйцукенгшщзхъфывапролджэячсмитьбю";
        private static string DIGIT_CHARS = "0123456789";
        private static string ENG_CHARS = "qwertyuiopasdfghjklzxcvbnm" + "QWERTYUIOPASDFGHJKLZXCVBNM";




        /// <summary>
        /// Метод разбора идентификатора на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitName(string name)
        {
            NamingStyles style = ParseStyle(name);
            switch (style)
            {
                case NamingStyles.Kebab: return SplitKebabName(name);
                case NamingStyles.Snake: return SplitSnakeName(name);
                case NamingStyles.Capital: return SplitCapitalName(name);
                case NamingStyles.TSQL: return SplitTSQLName(name);

                case NamingStyles.Camel: return SplitCamelName(name);
                default:
                    throw new Exception($"Не удалось разобрать идентификатор {name}.");
            }
        }

        private static string[] SplitTSQLName(string name)
        {
            return name.Split("_");
        }


        /// <summary>
        /// Запись идентификатора в CapitalStyle
        /// </summary>
        /// <param name="lastname"> идентификатор </param>
        /// <returns>идентификатор в CapitalStyle</returns>
        public static string ToCapitalStyle(string lastname)
        {
            if (string.IsNullOrEmpty(lastname)) return lastname;
            string[] ids = SplitName(lastname);
            return ToCapitalStyle(ids);
        }
        public static string ToTSQLStyle(string lastname)
        {

            string[] ids = SplitName(lastname);
            return ToTSQLStyle(ids);


        }
        public static string ToTSQLStyle(string[] ids)
        {
            string name = "";
            foreach (string id in ids)
            {
                if (name == "")
                {
                    name += id.ToUpper();
                }
                else
                {
                    name += "_" + id.ToUpper();
                }

            }
            return name;
        }
        public static string ToCapitalStyle(string[] ids)
        {
            string name = "";
            foreach (string id in ids)
            {
                name += id.Substring(0, 1).ToUpper() + id.Substring(1).ToLower();
            }
            return name;
        }


        /// <summary>
        /// Запись идентификатора в CamelStyle
        /// </summary>
        /// <param name="lastname"> идентификатор </param>
        /// <returns>идентификатор в CamelStyle</returns>
        public static string ToCamelStyle(string lastname)
        {
            string name = ToCapitalStyle(lastname);
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }




        /// <summary>
        /// Запись идентификатора в KebabStyle
        /// </summary>
        /// <param name="lastname"> идентификатор </param>
        /// <returns>идентификатор в KebabStyle</returns>
        public static string ToKebabStyle(string lastname)
        {
            string name = "";
            foreach (string id in SplitName(lastname))
            {
                name += "-" + id.ToLower();
            }
            return name.Substring(1);
        }





        /// <summary>
        /// Запись идентификатора в SnakeStyle
        /// </summary>
        /// <param name="lastname"> идентификатор </param>
        /// <returns>идентификатор в SnakeStyle</returns>
        public static string ToSnakeStyle(string lastname)
        {
            string name = "";
            string[] names = SplitName(lastname);
            foreach (string id in names)
            {
                name += "_" + id.ToLower();
            }
            return name.Substring(1);
        }


        /// <summary>
        /// Метод разбора идентификатора записанного в CapitalStyle на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор записанный в CapitalStyle </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitCapitalName(string name)
        {
            List<string> ids = new List<string>();
            string word = "";
            bool WasUpper = false;
            foreach (char ch in name)
            {
                if (IsUpper(ch) && WasUpper == false)
                {
                    if (word != "")
                    {
                        ids.Add(word);
                    }
                    word = "";
                    WasUpper = true;
                }
                WasUpper = false;
                word += (ch + "");
            }
            if (word != "")
            {
                ids.Add(word);
            }
            word = "";
            return ids.ToArray();
        }


        /// <summary>
        /// Метод разбора идентификатора записанного в DollarStyle на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор записанный в DollarStyle </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitDollarName(string name)
        {
            List<string> ids = new List<string>();
            string word = "";
            bool first = true;
            foreach (char ch in name)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                if (IsUpper(ch))
                {
                    if (word != "")
                    {
                        ids.Add(word);
                    }
                    word = "";
                }
                word += (ch + "");
            }
            if (word != "")
            {
                ids.Add(word);
            }
            word = "";
            return ids.ToArray();
        }


        /// <summary>
        /// Метод разбора идентификатора записанного в CamelStyle на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор записанный в CamelStyle </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitCamelName(string name)
        {
            List<string> ids = new List<string>();
            string word = "";
            foreach (char ch in name)
            {
                if (IsUpper(ch))
                {
                    if (word != "")
                    {
                        ids.Add(word);
                    }
                    word = "";
                }
                word += (ch + "");
            }
            if (word != "")
            {
                ids.Add(word);
            }
            word = "";
            return ids.ToArray();
        }


        /// <summary>
        /// Метод разбора идентификатора записанного в SnakeStyle на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор записанный в SnakeStyle </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitSnakeName(string name)
        {
            return name.Split("_");
        }


        /// <summary>
        /// Метод разбора идентификатора записанного в KebabStyle на модификаторы 
        /// </summary>
        /// <param name="name"> идентификатор записанный в KebabStyle </param>
        /// <returns> модификаторы </returns>
        public static string[] SplitKebabName(string name)
        {
            return name.Split("-");
        }

        /// <summary>
        /// Перечисление стилей записи идентификаторов
        /// </summary>
        public enum NamingStyles
        {
            Capital, Kebab, Snake, Camel, TSQL
        }



        /// <summary>
        /// Метод определния стиля записи идентификатора
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> стиль записи </returns>
        public static NamingStyles ParseStyle(string name)
        {
            if (IsTSQLStyle(name))
                return NamingStyles.TSQL;
            if (IsCapitalStyle(name))
                return NamingStyles.Capital;
            if (IsKebabStyle(name))
                return NamingStyles.Kebab;
            if (IsSnakeStyle(name))
                return NamingStyles.Snake;

            if (IsCamelStyle(name))
                return NamingStyles.Camel;

            throw new Exception($"Стиль идентификатора {name} не определён.");
        }

        public static bool IsTSQLStyle(string name)
        {
            bool lastCharWasSeparator = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (i == 0 || i == (name.Length - 1))
                {
                    if (IsEnglish(name[i]) == false || IsUpper(name[i]) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    if (name[i] == '_')
                    {
                        if (lastCharWasSeparator)
                        {
                            return false;
                        }
                        else
                        {
                            lastCharWasSeparator = true;
                        }

                    }
                    else
                    {
                        lastCharWasSeparator = false;
                        if (IsEnglish(name[i]) == false)
                        {
                            return false;
                        }
                        if (IsUpper(name[i]) == false)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Проверка сивола на принадлежность с множеству цифровых символов
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>true, если символ цифровой</returns>
        public static bool IsDigit(char ch)
        {
            return Contains(DIGIT_CHARS, ch);
        }


        /// <summary>
        /// Проверка сивола на принадлежность с множеству символов русского алфавита
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>true, если символ из русского алфавита </returns>
        public static bool IsCharacter(char ch)
        {
            return IsRussian(ch) || IsEnglish(ch);
        }


        /// <summary>
        /// Проверка сивола на принадлежность с множеству символов русского алфавита
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>true, если символ из русского алфавита </returns>
        public static bool IsRussian(char ch)
        {
            return Contains(RUS_CHARS, ch);
        }


        /// <summary>
        /// Проверка сивола на принадлежность с множеству символов русского алфавита
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>true, если символ из русского алфавита </returns>
        public static bool IsEnglish(char ch)
        {
            return Contains(ENG_CHARS, ch);
        }


        /// <summary>
        /// Проверка принадлежности символа к строке
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool Contains(string text, char ch)
        {
            bool result = false;
            foreach (char rch in text)
            {
                if (rch == ch)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// Метод проверки символа на принадлежность к верхнему регистру
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns> true, если принадлежит верхнему регистру </returns>
        public static bool IsUpper(char ch)
        {
            return (ch + "") == (ch + "").ToUpper();
        }


        /// <summary>
        /// Проверка стиля записи CapitalStyle( UserId )
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> true, если идентификатор записан в CapitalStyle </returns>
        public static bool IsCapitalStyle(string name)
        {
            bool startedWithUpper = (name[0] + "") == (name[0] + "").ToUpper();
            bool containsSpecCharaters = name.IndexOf("_") != -1 || name.IndexOf("$") != -1;
            return startedWithUpper && !containsSpecCharaters;
        }


        /// <summary>
        /// Проверка стиля записи SnakeStyle( user_id, USER_ID )
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> true, если идентификатор записан в SnakeStyle </returns>
        public static bool IsSnakeStyle(string name)
        {
            bool upperCase = IsUpper(name[0]);
            bool startsWithCharacter = IsCharacter(name[0]);
            char separatorCharacter = '_';
            string anotherChars = new String(SPEC_CHARS).Replace(separatorCharacter + "", "");
            bool containsAnotherSpecChars = false;
            bool containsAnotherCase = false;
            bool containsDoubleSeparator = false;
            bool lastCharWasSeparator = false;
            if (startsWithCharacter == false)
            {
                return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
            }
            else
            {
                for (int i = 1; i < name.Length; i++)
                {
                    if (Contains(anotherChars, name[i]))
                    {
                        containsAnotherSpecChars = true;
                        break;
                    }
                    if (name[i] != separatorCharacter)
                    {
                        if (IsUpper(name[i]) != upperCase)
                        {
                            containsAnotherCase = true;
                            break;
                        }
                        lastCharWasSeparator = false;
                    }
                    else
                    {
                        if (lastCharWasSeparator)
                        {
                            containsDoubleSeparator = true;
                            break;
                        }
                        lastCharWasSeparator = true;
                    }
                }
            }
            return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
        }


        /// <summary>
        /// Проверка стиля записи CamelStyle( userId  )
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> true, если идентификатор записан в CamelStyle </returns>
        public static bool IsCamelStyle(string name)
        {
            return IsCapitalStyle(name.Substring(0, 1).ToUpper() + name.Substring(1)) && !IsUpper(name[0]) && IsCharacter(name[0]);
        }


        /// <summary>
        /// Проверка стиля записи DollarStyle( $userId  )
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> true, если идентификатор записан в DollarStyle </returns>
        public static bool IsDollarStyle(string name)
        {
            return IsCamelStyle(name.Substring(1)) && name[0] == '$';
        }


        /// <summary>
        /// Проверка стиля записи KebabStyle( user-id, USER-ID )
        /// </summary>
        /// <param name="name"> идентификатор </param>
        /// <returns> true, если идентификатор записан в KebabStyle </returns>
        public static bool IsKebabStyle(string name)
        {
            bool upperCase = IsUpper(name[0]);
            bool startsWithCharacter = IsCharacter(name[0]);
            char separatorCharacter = '-';
            string anotherChars = new String(SPEC_CHARS).Replace(separatorCharacter + "", "");
            bool containsAnotherSpecChars = false;
            bool containsAnotherCase = false;
            bool containsDoubleSeparator = false;
            bool lastCharWasSeparator = false;
            if (startsWithCharacter == false)
            {
                return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
            }
            else
            {
                for (int i = 1; i < name.Length; i++)
                {
                    if (Contains(anotherChars, name[i]))
                    {
                        containsAnotherSpecChars = true;
                        break;
                    }
                    if (name[i] != separatorCharacter)
                    {
                        if (IsUpper(name[i]) != upperCase)
                        {
                            containsAnotherCase = true;
                            break;
                        }
                        lastCharWasSeparator = false;
                    }
                    else
                    {
                        if (lastCharWasSeparator)
                        {
                            containsDoubleSeparator = true;
                            break;
                        }
                        lastCharWasSeparator = true;
                    }
                }
            }
            return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\NamingStyles.cs **************/

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\RusEngTranslite.cs **************/
    public class RusEngTranslite
    {
        public static string TransliteToLatine(string rus)
        {
            if (rus == null)
            {
                return null;
            }
            string latine = "";
            for (int i = 0; i < rus.Length; i++)
            {
                latine += map(rus[i]);
            }
            return latine;
        }

        private static string map(char v)
        {
            switch ((v + "").ToLower())
            {
                case "а": return "a";
                case "б": return "b";
                case "в": return "v";
                case "г": return "g";
                case "д": return "d";
                case "е": return "e";
                case "ё": return "yo";
                case "ж": return "j";
                case "з": return "z";
                case "и": return "i";
                case "й": return "yi";
                case "к": return "k";
                case "л": return "l";
                case "м": return "m";
                case "н": return "n";
                case "о": return "o";
                case "п": return "p";
                case "р": return "r";
                case "с": return "s";
                case "т": return "t";
                case "у": return "u";
                case "ф": return "f";
                case "х": return "h";
                case "ц": return "c";
                case "ш": return "sh";
                case "щ": return "sh";
                case "ъ": return "'";
                case "ы": return "u";
                case "ь": return "'";
                case "э": return "a";
                case "ю": return "y";
                case "я": return "ya";
                default: return v + "";
            }
        }
    }

    /******* * D:\gitlab\auth\Data\Resources\Common\CommonUtils\Text\Writing.cs **************/
    public class Writing
    {



        public static void ToConsoleJson(object target)
        {
            ToConsole(JObject.FromObject(target).ToString());
        }

        public static void ToConsole(Exception ex)
        {
            Writing.ToConsole("\n\n");
            Exception p = ex;
            while (p != null)
            {
                Writing.ToConsole(p.Message);
                p = p.InnerException;
            }
            Writing.ToConsole("\n\n");
            Writing.ToConsole(ex.StackTrace);
            Writing.ToConsole("\n\n");
        }

        public static void ToConsole(IEnumerable items)
        {
            foreach (var item in items)
            {
                Writing.ToConsole(item.ToString());
            }
        }

        public static void ToConsole(IEnumerable<KeyValuePair<object, object>> configuration)
        {

            foreach (var pair in configuration)
            {
                ToConsole($"\t{pair.Key}={pair.Value}");
            }
            ToConsole($"\n");

        }

        public static void ToConsole(string title, IDictionary<string, string> configuration)
        {
            ToConsole($"\n{title}");
            IEnumerator<KeyValuePair<string, string>> enumerator = configuration.AsEnumerable().GetEnumerator();
            while (enumerator.MoveNext())
            {
                ToConsole($"\t{enumerator.Current.Key}={enumerator.Current.Value}");
            }
            ToConsole($"\n");

        }



        public static void ToConsole(string message)
        {
            System.Console.WriteLine(message);
            Debug.WriteLine(message);
        }

        public static void ToConsole(string title, string[] messages)
        {
            ToConsole($"\n{title}");
            foreach (string message in messages)
            {
                ToConsole($"\t {message}");
            }
        }

        public static void Log(Exception ex)
        {
            Writing.ToConsole(ex.Message);
            Writing.ToConsole(ex.StackTrace);
        }
    }


}