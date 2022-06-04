using UnityEngine;

namespace Common
{

public class SyncronizeView : ISyncronizeView
{
    public Camera Camera { get; private set; }

    public SyncronizeView(Camera camera)
    {
        Camera = camera;
    }

}

}
