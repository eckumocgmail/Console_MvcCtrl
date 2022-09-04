using Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceEndpoint
{
    public class EncProgram
    {
        [InputTextAttribute]
        [RusText]
        public static string RusCharacters {get;set;}

        public static void Run()
        {
            
            string alf = "абвгджеёжзйиклмнпорстуфхцчшщъыьэюя" + " .,1234567890" + "абвгджеёжзйиклмнпорстуфхцчшщъыьэюя".ToUpper() + " ";
            var encodings = new List<Encoding>();            
            encodings.AddRange(Encoding.GetEncodings().Select(info=>info.GetEncoding()));            
            foreach (var enc in encodings)
            { 
                var codes = new Dictionary<char, IEnumerable<byte>>();
                foreach (var ch in alf)
                {
                    codes[ch] = enc.GetBytes(ch + "").ToList();
                };                
                Console.WriteLine(new { 
             
                    enc = enc,
                    codes = codes
                }.ToJsonOnScreen());
                new
                {                 
                    enc = enc,
                    codes = codes
                }.ToJsonOnScreen().WriteToFile($@"D:\encidngs\{enc.EncodingName}.txt");



            }
        }
    }
}
