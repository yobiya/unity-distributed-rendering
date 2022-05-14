public class GameModeUIViewController
{
    public interface IUICollection
    {
        IButtonUIView GameClientModeButton { get; }
        IButtonUIView RenderingServerModeButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public GameModeUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.GameClientModeButton.Active = true;
        _uiCollection.RenderingServerModeButton.Active = true;
    }
}
