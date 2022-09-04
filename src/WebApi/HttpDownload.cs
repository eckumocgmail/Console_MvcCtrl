using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Transport
{

    /// <summary>
    /// Выполняет загрузку файлов с URL ресурс
    /// </summary>
    public  class HttpDownloadProgram
    {

        /// <summary>
        /// 
        /// </summary>
        public void Help()
        {
            Console.WriteLine(@"Интрумент загрузки бинарных данных по HTTP адреса");
            Console.WriteLine(@"Параметры вводятся в формате JSON, согласно спецификации TypeScript type params: {[property: string]: string}");
            Console.WriteLine(@"{ [http]: [file] }");
        }

        /// <summary>
        /// Выполнение програмы
        /// </summary>
        /// <param name="args"></param>
        public static void Run(string[] args)
        {
            var program = new HttpDownloadProgram();
            if (Environment.UserInteractive)
            {
                program.OnUserStart();
            }
            else
            {
                program.OnSystemStart();
            }
        }


        /// <summary>
        /// При запуске системой в  автоматизированной среде
        /// </summary>
        private void OnSystemStart()
        {
            var program = new HttpDownloadProgram();
            program.Help();           
        }


        /// <summary>
        /// При запуске пользователем
        /// </summary>
        private void OnUserStart()
        {
            Console.WriteLine("Для загрузки бинарных данных введите параметры: ");
            string url = Input("URL");
            string file = Input("File");
            byte[] data = Download(url).Result;
            System.IO.File.WriteAllBytes(file, data);
        }


        /// <summary>
        /// Ввод данных через консоль
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string Input(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
 



        /// <summary>
        /// Скачивание избражения с ресурса доступого по URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> Download(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            Console.WriteLine(JsonConvert.SerializeObject(response.Headers, new JsonSerializerSettings() { Formatting = Formatting.Indented }));
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync();

            byte[] data = new byte[ms.Length];
            ms.Read(data);
            return data;
        }
    }
}    