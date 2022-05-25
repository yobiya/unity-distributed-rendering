using UnityEngine;

namespace Common
{

public class BaseUIViewAdapter : MonoBehaviour, IBaseView
{
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
}

}
