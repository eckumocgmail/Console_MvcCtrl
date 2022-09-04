using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MessageLevel
{
    public class TextMessage: IEnumerable<byte[]>
    {
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();
 
        public TextMessage()
        {
        }

       

        public void Enqueue(byte[] package)
        {
            queue.Enqueue(package);
        }


        public byte[] Dequeue( )
        {
            byte[] data = null;
            queue.TryDequeue(out data);
            return data;
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queue.GetEnumerator();
        }
    }
}