 
using COM;

using System;
 

namespace SpbPublicLibsUnit.Encoder
{
    class TestEncode: TestingElement
    {
        protected override void OnTest()
        {
            this.canConvertCharactersToBinaryCode();
            this.binaryConvertionTest();
        }

        private void canConvertCharactersToBinaryCode()
        {
            BinaryEncoder encoder = new BinaryEncoder();

            string text = "ABBCCCDDDDEEEEE";
            string encoded = encoder.ToBinary(text);
            string decoded = encoder.FromBinary(encoded);

            Console.WriteLine($"text: {text}");
            Console.WriteLine($"encoded: {encoded}");
            Console.WriteLine($"decoded: {decoded}");

           
            this.Messages.Add("реализован алгоритм кодирования хаффмана");
        }

        private void binaryConvertionTest()
        {
            string text = "this is a test";         
            BinaryEncoder haffman = new BinaryEncoder();
            string binary = haffman.ToBinary( text );
            string character = haffman.FromBinary( binary );
            if (character != text)
            {
                this.Report.failed = true;
                throw new Exception("convert to/from binary test");
            }
        }
         
    }
}
