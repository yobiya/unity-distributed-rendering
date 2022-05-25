using Common;
using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingViewCollection : MonoBehaviour, OffscreenRenderingViewController.IViewCollection
{
    [SerializeField]
    private CameraViewAdapter _camera;

    public ICameraView Camera => _camera;
}

}
