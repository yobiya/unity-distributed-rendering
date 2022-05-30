using UnityEngine;

namespace RenderingServer
{

public interface ISyncCameraView
{
    void Activate();
    void Deactivate();
    void UpdateTransform(Vector3 position, Vector3 forword);
}

}
