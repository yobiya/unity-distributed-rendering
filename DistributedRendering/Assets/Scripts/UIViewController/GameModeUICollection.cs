using UnityEngine;

public class GameModeUICollection : MonoBehaviour, GameModeUIViewController.IUICollection
{
    [SerializeField]
    ButtonUIViewAdapter _gameClientModeButton;

    [SerializeField]
    ButtonUIViewAdapter _renderingServerModeButton;

    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public IButtonUIView GameClientModeButton => _gameClientModeButton;
    public IButtonUIView RenderingServerModeButton => _renderingServerModeButton;
}
