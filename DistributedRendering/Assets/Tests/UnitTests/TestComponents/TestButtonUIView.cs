using System;

namespace Common
{

public class TestButtonUIView : TestBaseUIView, IButtonUIView
{
    public event Action OnClicked;

    public void Click()
    {
        OnClicked?.Invoke();
    }
}

}
