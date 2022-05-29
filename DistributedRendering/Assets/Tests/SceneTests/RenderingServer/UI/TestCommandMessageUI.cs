using System;
using UnityEngine;
using UnityEngine.UI;

namespace RenderingServer
{

public class TestCommandMessageUI : MonoBehaviour, ITestCommandMessageUI
{
    [SerializeField]
    private Button _renderButton;

    public event Action OnClickedRender;

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}

}
