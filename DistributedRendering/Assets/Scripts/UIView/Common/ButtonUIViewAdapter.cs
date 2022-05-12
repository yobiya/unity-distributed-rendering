using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIViewAdapter : BaseUIViewAdapter, IButtonUIView
{
    [SerializeField]
    private Button _button;

    public event Action OnClicked;

    void Start()
    {
        _button.onClick.AddListener(() => OnClicked?.Invoke());
    }
}
