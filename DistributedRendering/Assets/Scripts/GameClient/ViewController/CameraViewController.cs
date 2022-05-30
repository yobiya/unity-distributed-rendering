using System;
using UnityEngine;

namespace GameClient
{

public class CameraViewController : ICameraViewController
{
    public event Action<Transform> OnUpdateTransform;

    public CameraViewController(ICameraView camera)
    {
    }
}

}
