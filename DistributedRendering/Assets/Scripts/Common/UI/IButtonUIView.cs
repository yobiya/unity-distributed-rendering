using System;

namespace Common
{

public interface IButtonUIView : IBaseView
{
    event Action OnClicked;
}

}
