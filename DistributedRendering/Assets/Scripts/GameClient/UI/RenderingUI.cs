using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{

public class RenderingUI : MonoBehaviour, IRenderingUI
{
    [SerializeField]
    RawImage _image;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
