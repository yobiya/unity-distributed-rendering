public class GameClientWaitConnectionUIViewControler : IGameClientWaitConnectionUIViewControler
{
    public interface UICollection
    {
        bool IsActive { get; set; }
    };

    private readonly UICollection _uiCollection;

    public GameClientWaitConnectionUIViewControler(UICollection uICollection)
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
