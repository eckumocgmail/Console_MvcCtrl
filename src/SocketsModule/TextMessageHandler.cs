
using MessageLevel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceEndpoint.ServiceEndpoint
{

    public interface IHandler<TMessage>
    {
        public TMessage OnMessage(TMessage input);
    }


    

    public interface IConverter<From, To>
    {
        public To Convert(From input);
    }

    




    public interface IProtocolBuilder:
            IConverter<byte[], string>,
            IConverter<string, string[]>,

            //тектовые данные => HttpMessageModel
            IConverter<string[], KeyValuePair<IDictionary<string,string>,byte[]>>,
            IConverter<KeyValuePair<IDictionary<string, string>, byte[]>, MyMessageModel>
    {
        public Type[] MessageTypes();
        
    }


    class HttpMessageModel
    {
        IList<string> Headers = new List<string>();
        byte[] Body = new byte[0];
    }


    class Proto: IConverter<byte[], HttpMessageModel>
    {
        class BinaryConverter : IConverter<byte[], string> 
        {
            public string Convert(byte[] input)
            {
                throw new NotImplementedException();
            }
        }
        class MessageConverter : IConverter<byte[], string>
        {
            public string Convert(byte[] input)
            {
                throw new NotImplementedException();
            }
        }

        public HttpMessageModel Convert(byte[] input)
        {
            throw new NotImplementedException();
        }
    }





    public class TextMessageHandler: TextMessage
    {
        
        public virtual Func<string, MessageLevel.MessageModel> GetConnection()
        {
            return (text) => {
                MessageLevel.MessageModel message = new MessageLevel.MessageModel();
                var state = Task.Run(()=> 
                {
                    text.Split(Environment.NewLine);
                });
                return message;
            };
        }

        public Func<string, TextMessage> InputText()
        {
            var ctrl = this;
            return (text) => {
                MessageLevel.TextMessage message = new MessageLevel.TextMessage();
                var state = Task.Run(() =>
                {
                    text.Split(Environment.NewLine);
                });

                message.ToArray();
                
                return message;
            };
        }
    }
}
