public interface EventSource
{
    string Info(params object[] args);
    string Debug(params object[] args);
    string Warn(params object[] args);
    string Error(params object[] args);
}
public interface OnInit
{
    public void Init();
}


public interface OnChange
{
    public void Change();
}


public interface OnDestroy
{
    public void Destroy();
}

public interface OnUpdate
{
    public void Update();
}
