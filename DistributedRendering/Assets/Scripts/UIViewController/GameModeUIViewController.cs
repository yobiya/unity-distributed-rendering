using System;

public class GameModeUIViewController : IGameModeUIViewController
{
    public interface IUICollection
    {
        IButtonUIView GameClientModeButton { get; }
        IButtonUIView RenderingServerModeButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.GameClientModeButton.Active = true;
        _uiCollection.RenderingServerModeButton.Active = true;

        _uiCollection.GameClientModeButton.OnClicked += () => OnSelectedGameClientMode?.Invoke();
        _uiCollection.RenderingServerModeButton.OnClicked += () => OnSelectedRenderingServerMode?.Invoke();
    }
}
