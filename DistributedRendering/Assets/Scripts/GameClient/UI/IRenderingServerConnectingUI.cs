using Common;

namespace GameClient
{

public interface IRenderingServerConnectingUI
{
    bool IsActive { get; set; }

    IButtonUIView ConnectingRequestButton { get; }
    ITextUIView ConnectingText { get; }
    ITextUIView ConnectedText { get; }
    ITextUIView FailedText { get; }
}

}
