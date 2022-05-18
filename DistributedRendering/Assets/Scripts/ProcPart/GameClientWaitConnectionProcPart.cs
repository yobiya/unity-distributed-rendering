public class GameClientWaitConnectionProcPart : IGameClientWaitConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;

    public GameClientWaitConnectionProcPart(IGameClientWaitConnectionUIViewControler gameClientWaitConnectionUIViewControler)
    {
        _gameClientWaitConnectionUIViewControler = gameClientWaitConnectionUIViewControler;
    }

    public void Activate()
    {
        _gameClientWaitConnectionUIViewControler.Activate();
    }

    public void Deactivate()
    {
        _gameClientWaitConnectionUIViewControler.Deactivate();
    }
}
