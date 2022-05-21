using UnityEngine;

public class TestMessageSendUICollection : MonoBehaviour, TestMessageSendUIViewController.IUICollection
{
    [SerializeField]
    ButtonUIViewAdapter _sendButton;

    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public IButtonUIView SendButton => _sendButton;
}
