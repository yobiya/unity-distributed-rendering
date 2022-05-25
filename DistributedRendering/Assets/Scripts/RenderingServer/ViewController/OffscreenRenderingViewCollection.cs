using Common;
using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingViewCollection : OffscreenRenderingViewController.IViewCollection
{
    [SerializeField]
    private CameraViewAdapter _camera;

    public ICameraView Camera => _camera;
}

}
