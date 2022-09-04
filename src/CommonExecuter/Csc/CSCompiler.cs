public class CSCompilerNamespace
{
    public interface CSCompiler
    {

        /// <summary>
        /// Версия языка
        /// </summary>
        public int LangVersion { get; set; }

        /// <summary>
        /// Рабочая директория( директория с исходными файлами )
        /// </summary>
        public string WorkDir { get; set; }

        /// <summary>
        /// Тип компиляции: exe,winexe,library,module,winmdobj,appcontainerexe
        /// 
        /// </summary>
        public string Target { get; set; }
    }
}