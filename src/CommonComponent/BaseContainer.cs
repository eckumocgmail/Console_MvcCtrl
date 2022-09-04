using System;
using System.Collections.Generic;




public class BaseContainer: BaseComponent
{
    public List<BaseComponent> Components = new List<BaseComponent>();
  
    public override void Init() => this.Components.ForEach(InitComponent);
    public override void Destroy() => this.Components.ForEach(DestroyComponent);

    public BaseContainer()
    {
    }

    private void InitComponent(BaseComponent component)
    {
        try
        {
            component.Init();
        }
        catch (Exception ex)
        {
            ThrowInitException(this, component, ex);
        }
    }

    private void DestroyComponent(BaseComponent component)
    {
        try
        {
            component.Destroy();
        }
        catch (Exception ex)
        {
            ThrowDestroyException(this, component, ex);
        }
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }


    

    public override void Change()
    {
        throw new System.NotImplementedException();
    }

    



    private void ThrowDestroyException(BaseContainer baseContainer, BaseComponent component, Exception ex)
    {
        throw new NotImplementedException();
    }

    private void ThrowInitException(BaseContainer baseContainer, BaseComponent component, Exception ex)
    {
        throw new NotImplementedException();
    }
}


