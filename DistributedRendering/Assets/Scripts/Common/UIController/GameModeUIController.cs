using System;

namespace Common
{

public class GameModeUIController : IGameModeUIController
{
    public interface IUICollection
    {
        bool IsActive { get; set; }

        IButtonUIView GameClientModeButton { get; }
        IButtonUIView RenderingServerModeButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeUIController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;
        _uiCollection.IsActive = true;

        _uiCollection.GameClientModeButton.OnClicked += () =>
        {
            _uiCollection.IsActive = false;
            OnSelectedGameClientMode?.Invoke();
        };
        _uiCollection.RenderingServerModeButton.OnClicked += () =>
        {
            _uiCollection.IsActive = false;
            OnSelectedRenderingServerMode?.Invoke();
        };
    }

    public void Activate()
    {
        _uiCollection.IsActive = true;
    }

    public void Deactivate()
    {
        _uiCollection.IsActive = false;
    }
}

}
