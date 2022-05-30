using UnityEngine;

namespace RenderingServer
{

public class SyncCameraViewController : ISyncCameraViewController
{
    private readonly ISyncCameraView _syncCameraView;

    public SyncCameraViewController(ISyncCameraView syncCameraView)
    {
        _syncCameraView = syncCameraView;
    }

    public void Activate()
    {
        _syncCameraView.Activate();
    }

    public void Deactivate()
    {
        _syncCameraView.Deactivate();
    }

    public void Sync(string text)
    {
        if (!text.StartsWith("@camera:"))
        {
            return;
        }

        text = text.Remove(0, "@camera:".Length);

        var stringValues = text.Split(',');

        var position = new Vector3();
        position.x = float.Parse(stringValues[0]);
        position.y = float.Parse(stringValues[1]);
        position.z = float.Parse(stringValues[2]);

        var forword = new Vector3();
        forword.x = float.Parse(stringValues[3]);
        forword.y = float.Parse(stringValues[4]);
        forword.z = float.Parse(stringValues[5]);

        _syncCameraView.UpdateTransform(position, forword);
    }
}

}
