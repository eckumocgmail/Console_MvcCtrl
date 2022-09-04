using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace eckumoc_common_api.SecurityServices.SymmetricEncription
{
    public interface DataEncriptor
    {

        public byte[] GetInitiallizationVector();
        public byte[] GetPublicKey();



        public byte[] Dec(byte[] data);
        public byte[] Enc(string data);
    }
    public class Encriptor: DataEncriptor, IDisposable
    {

        public byte[] iv { get; set; }
        public byte[] pk { get; set; }

        private Aes aes = Aes.Create();

        public KeyValuePair<byte[], byte[]> PublicKey()
        {
                       
            return new KeyValuePair<byte[], byte[]>(this.iv = aes.IV, this.pk = aes.Key);
        }

        public byte[] GetInitiallizationVector() => iv;

        public byte[] GetPublicKey() => pk;



        public byte[] Dec(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Enc(string data )
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            this.aes.Dispose();
        }
    }
}
