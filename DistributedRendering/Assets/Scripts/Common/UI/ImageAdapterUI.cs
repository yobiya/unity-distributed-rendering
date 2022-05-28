using UnityEngine;
using UnityEngine.UI;

namespace Common
{

public class ImageAdapterUI : MonoBehaviour, IImageUI
{
    [SerializeField]
    private Image _image;

    public Material Material
    {
        set => _image.material = value;
        get => _image.material;
    }
}

}
