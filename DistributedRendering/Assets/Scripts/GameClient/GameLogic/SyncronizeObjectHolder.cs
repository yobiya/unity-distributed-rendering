using UnityEngine;

namespace GameClient
{

public class SyncronizeObjectHolder : ISyncronizeObjectHolder
{
    public Camera Camera { get; private set; }

    public SyncronizeObjectHolder(Camera camera)
    {
        Camera = camera;
    }

}

}
