using UnityEngine;
using Common;

public class GameClientWaitConnectionUICollection : MonoBehaviour, GameClientWaitConnectionUIViewControler.IUICollection
{
    [SerializeField]
    TextUIViewAdapter _waitConnectionText;

    [SerializeField]
    TextUIViewAdapter _connectedText;

    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public ITextUIView WaitConnectionText => _waitConnectionText;
    public ITextUIView ConnectedText => _connectedText;
}
