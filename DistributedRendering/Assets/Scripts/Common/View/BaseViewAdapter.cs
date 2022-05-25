using UnityEngine;

namespace Common
{

public class BaseViewAdapter : MonoBehaviour, IBaseView
{
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
}

}
