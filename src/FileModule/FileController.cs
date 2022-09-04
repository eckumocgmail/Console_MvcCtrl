using System;
 
public class FileController<T> where T: class
{
    public string FilePath { get; set; }        
    public FileResource FileResource { get; set; }


    public T Model { get; set; }
    public bool Initialized = false;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    public FileController(string filePath)
    {
        if (System.IO.File.Exists(filePath) == false)
            System.IO.File.WriteAllText(filePath, typeof(T).New().ToJson());
        this.FilePath = filePath;
        this.InitFileController();
    }

    /// <summary>
    /// Создаёт файл со значениями по-умолчанию , если он отсутсвует
    /// </summary>
    private void InitFileController()
    {
        lock (this)
        {
            if( Initialized == false)
            {
                string json = null;
                if (System.IO.File.Exists(this.FilePath) == false)
                {
                    this.Model = (T)typeof(T).New();
                    json = Model.ToJson();
                    json.WriteToFile(this.FilePath);
                }
                this.FileResource = new FileResource(this.FilePath);
                json = this.FileResource.ReadText();
                Model = json.FromJson<T>();
                this.Initialized = true;

            }
        }
    }


    /// <summary>
    /// Возвращает модель считанную из файла
    /// </summary>
    public T Get()
    {
        lock (this)
        {
            this.InitFileController();

            return Model;
        }
    }


    /// <summary>
    /// Вывод модели в файл
    /// </summary>
    public void Set()
    {
        lock (this)
        {
            this.InitFileController();
            string json = Model.ToJson();
            this.FileResource.WriteText(json);
           
            Console.WriteLine($"\n{GetType().GetTypeName()} Записано: \n{json.Length} байт");            
            
        }
    }
}
 