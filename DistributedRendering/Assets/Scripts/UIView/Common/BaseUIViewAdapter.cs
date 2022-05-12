using UnityEngine;

public class BaseUIViewAdapter : MonoBehaviour, IBaseUIView
{
    public bool Active
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
}
