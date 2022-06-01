using Common;

public class GameClientWaitConnectionUIViewControler : IGameClientWaitConnectionUIViewControler
{
    public interface IUICollection
    {
        bool IsActive { get; set; }
        ITextUIView WaitConnectionText { get; }
        ITextUIView ConnectedText { get; }
    };

    private readonly IUICollection _uiCollection;

    public GameClientWaitConnectionUIViewControler(IUICollection uICollection)
    {
        _uiCollection = uICollection;
    }

    public void Activate()
    {
        _uiCollection.IsActive = true;
    }

    public void Deactivate()
    {
        _uiCollection.IsActive = false;
    }

    public void ShowWaitConnection()
    {
        _uiCollection.WaitConnectionText.IsActive = true;
        _uiCollection.ConnectedText.IsActive = false;
    }

    public void ShowConnected()
    {
        _uiCollection.WaitConnectionText.IsActive = false;
        _uiCollection.ConnectedText.IsActive = true;
    }
}
