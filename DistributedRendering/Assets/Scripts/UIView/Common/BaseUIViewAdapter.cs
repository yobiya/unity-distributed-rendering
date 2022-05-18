using UnityEngine;

public class BaseUIViewAdapter : MonoBehaviour, IBaseUIView
{
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
}
