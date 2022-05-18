public class GameClientWaitConnectionUIViewControler : IGameClientWaitConnectionUIViewControler
{
    public interface IUICollection
    {
        bool IsActive { get; set; }
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
        throw new System.NotImplementedException();
    }

    public void ShowConnected()
    {
        throw new System.NotImplementedException();
    }
}
