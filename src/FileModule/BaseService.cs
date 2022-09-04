public class BaseService<T>: TestingElement
{
    public BaseService()
    {
    }

    protected override void OnTest()
    {        
        Messages.Add("Успешно создали новыц экземпляр класса  "+nameof(T));
        Messages.Add("Can create success new instance of " + nameof(T));

        var instance = typeof(T).New();
        var opers = instance.GetOperations();
        foreach (var kv in opers)
        {
            Messages.Add($"\t{kv.Key}={kv.Value}");
        }
    }
}