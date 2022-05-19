using System;

public interface IButtonUIView : IBaseUIView
{
    event Action OnClicked;
}
