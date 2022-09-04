using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Threading.Tasks;

namespace eckumoc.Utils
{
    /// <summary>
    /// JsonConverter   
    /// <c>Program.Main</c>
    /// </summary>      
    /// <remarks>
    /// This class can get json property from file and set json property in file.
    /// </remarks>
    public class JsonFileEditor
    {
        /// <summary>
        /// Get json property from file.
        /// </summary>
        /// <param name="filename">Full filename of json file.</param>
        /// <param name="selector">Full path (property names,sepated '.') for example: app.name;</param>
        /// <returns>Value of json property</returns>
        /// <exception cref="System.ArgumentException">
        /// path is a zero-length string, contains only white space, or contains one or more
        /// invalid characters as defined by System.IO.Path.InvalidPathChars.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// path is null
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// path specified a file that is read-only. -or- This operation is not supported
        /// on the current platform. -or- path specified a directory. -or- The caller does
        /// not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>    
        /// <exception cref="Newtonsoft.Json.JsonReaderException">
        /// Error parsing json.
        /// </exception>            
        /// <exception cref="System.NullReferenceException">
        /// Selection return null reference.
        /// </exception>            
        public static JToken Get(string filename, string selector)
        {            
            string jsontext = System.IO.File.ReadAllText(filename);
            JToken json = JsonConvert.DeserializeObject<JObject>(jsontext);
            if ( String.IsNullOrEmpty(selector))
            {
                return json;
            }
            else
            {                
                foreach (string id in selector.Split("."))
                {
                    json = json[id];
                    if(json == null)
                    {
                        throw new System.NullReferenceException("Selection return null reference.");
                    }
                }
                return json;
            }                        
        }
        public async static Task<JToken> GetAsync(string filename, string selector)
        {
            string jsontext = await System.IO.File.ReadAllTextAsync(filename);
            JToken json = JsonConvert.DeserializeObject<JObject>(jsontext);
            if (String.IsNullOrEmpty(selector))
            {
                return json;
            }
            else
            {
                foreach (string id in selector.Split("."))
                {
                    json = json[id];
                    if (json == null)
                    {
                        throw new System.NullReferenceException("Selection return null reference.");
                    }
                }
                return json;
            }
        }

        /// <summary>
        /// Set json property in file.
        /// </summary>
        /// <param name="filename">Full filename of json file.</param>
        /// <param name="selector">Full path (property names,sepated '.') for example: app.name;</param>
        /// <param name="value">Value of selected json property.</param>        
        /// <exception cref="System.ArgumentException">
        /// path is a zero-length string, contains only white space, or contains one or more
        /// invalid characters as defined by System.IO.Path.InvalidPathChars.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// path is null
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// path specified a file that is read-only. -or- This operation is not supported
        /// on the current platform. -or- path specified a directory. -or- The caller does
        /// not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>    
        /// <exception cref="Newtonsoft.Json.JsonReaderException">
        /// Error parsing json.
        /// </exception>            
        /// <exception cref="System.NullReferenceException">
        /// Selection return null reference.
        /// </exception>       
        public static void Set(string filename, string selector, string value)
        {
            string jsontext = System.IO.File.ReadAllText(filename);
            JToken root = JObject.FromObject(JsonConvert.DeserializeObject<object>(jsontext));
           
            JToken json = root;
            string[] ids = selector.Split(".");
            for( int i=0; i<(ids.Length-1); i++ )
            {
                json = json[ids[i]];
            }
            json[ids[ids.Length-1]] = value;
            System.IO.File.WriteAllText(filename, root.ToString());            
        }
        public async static Task SetAsync(string filename, string selector, string value)
        {
            string jsontext = await System.IO.File.ReadAllTextAsync(filename);
            JToken root = JObject.FromObject(JsonConvert.DeserializeObject<object>(jsontext));

            JToken json = root;
            string[] ids = selector.Split(".");
            for (int i = 0; i < (ids.Length - 1); i++)
            {
                json = json[ids[i]];
            }
            json[ids[ids.Length - 1]] = value;
            await System.IO.File.WriteAllTextAsync(filename, root.ToString());
        }
    }
}
