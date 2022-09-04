using System;
using System.Collections.Generic;

using static ConsoleMvcApi;

/// <summary>
/// Набор программных интерфейсов 
/// </summary>
public class ConsoleMvcApi
{

    



    /// <summary>
    /// Методы управление рабочей директорией репозиторий
    /// </summary>
    public interface IRepository
    {
        public string Add(string path);
        public string Pull();
        public string Pull(string remote);
        public string Rebase();
        public string Rebase(string remote);
        public string Push();
        public string Push(string remote);
        public string Checkout();
        public string Checkout(string branch);
        public string Branch();
        public string Branch(string branch);
        public string Commit(string message);
        public string Version();
    }

}
