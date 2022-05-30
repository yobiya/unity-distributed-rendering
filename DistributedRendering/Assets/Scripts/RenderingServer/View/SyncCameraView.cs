using UnityEngine;

namespace RenderingServer
{

public class SyncCameraView : MonoBehaviour, ISyncCameraView
{
    [SerializeField]
    Camera _camera;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTransform(Vector3 position, Vector3 forword)
    {
        _camera.transform.position = position;
        _camera.transform.forward = forword;
    }
}

}
