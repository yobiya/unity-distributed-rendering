using System;

public class GameModeUIViewController : IGameModeUIViewController
{
    public interface IUICollection
    {
        bool IsActive { get; set; }

        IButtonUIView GameClientModeButton { get; }
        IButtonUIView RenderingServerModeButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public bool IsActive {
        get { return _uiCollection.IsActive; }
        set { _uiCollection.IsActive = value; }
    }

    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeUIViewController(IUICollection uiCollection)
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
}
