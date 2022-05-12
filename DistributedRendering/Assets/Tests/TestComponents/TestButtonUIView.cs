using System;

public class TestButtonUIView : IButtonUIView
{
    public bool Active { get; set; } = true;

    public event Action OnClicked;

    public void Click()
    {
        OnClicked?.Invoke();
    }
}
