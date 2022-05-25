using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{

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

}
