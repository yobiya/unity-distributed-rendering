using System;

public class TestButtonUIView : IButtonUIView
{
    public event Action OnClicked;

    public void Click()
    {
        OnClicked?.Invoke();
    }
}
