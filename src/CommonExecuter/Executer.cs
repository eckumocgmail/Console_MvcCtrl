using eckumoc_common_api.CommonCollections;

using System;
using System.Collections.Generic;

namespace ConsoleMvc.Common.CommonExecuter
{
    
    public interface IUseCase<TService>
    {

    }
    public class ProgramUseCaseSearchRequest: KeyNotFoundException
    {

    }



    /// <summary>
    /// Исполнитель умеет выполнять однозадачном режим
    /// </summary>
    public class Executer: CommonDictionary<ICMD>
    {
        

        public string CmdExec(string command)
        {
            string result = "";
            if( command.IsFile() )
            {
                var ext = GetFilenameExt(command);
                string programeName = WhatCanIUseWith(ext);
                OpenWith(programeName, command);                
            }
            return result;
        }

        private void OpenWith(string program, string path)
        {
            string message = CmdExec($"{program} {path}" );
            

        }

        private string GetFilenameExt(string command)
        {
            int i = command.LastIndexOf(".");
            string ext = (i >= 0) ? command.Substring(i + 1) : "";
            return ext.ToUpper();
        }

        private string WhatCanIUseWith(string ext)
        {
            switch( ext )
            {
                case ".HTML":
                    break;
                default: 
                    throw new KeyNotFoundException(ext);
            }
            throw new KeyNotFoundException(ext);
        }

        public string Find(string regularExpression, string filename)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, object> Search(string regularExpression)
        {
            throw new System.NotImplementedException();
        }

        public string Search(string path, string pattern)
        {
            throw new System.NotImplementedException();
        }

        public void StartProcess(string command)
        {
            throw new System.NotImplementedException();
        }
    }
}
