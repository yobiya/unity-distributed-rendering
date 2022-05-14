using UnityEngine;

public class GameModeUICollection : MonoBehaviour, GameModeUIViewController.IUICollection
{
    public IButtonUIView GameClientModeButton => throw new System.NotImplementedException();
    public IButtonUIView RenderingServerModeButton => throw new System.NotImplementedException();
}
