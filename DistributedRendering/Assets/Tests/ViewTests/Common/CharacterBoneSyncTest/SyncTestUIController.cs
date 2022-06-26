using System;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Common
{

public class SyncTestUIController
{
    public event Action OnReadBonesButtonClicked;
    public event Action OnWriteBonesButtonClicked;

    private readonly Button _readBonesButton;
    private readonly Button _writeBonesButton;

    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public SyncTestUIController(Button readBonesButton, Button writeBonesButton)
    {
        _readBonesButton = readBonesButton;
        _writeBonesButton = writeBonesButton;
    }

    public void Activate()
    {
        UnityAction onReadBonesButtonClicked = () => { OnReadBonesButtonClicked?.Invoke(); };
        _inversionProc.Register(
            () =>
            {
                _readBonesButton.gameObject.SetActive(true);
                _readBonesButton.onClick.AddListener(onReadBonesButtonClicked);
            },
            () =>
            {
                _readBonesButton.gameObject.SetActive(false);
                _readBonesButton.onClick.RemoveListener(onReadBonesButtonClicked);
            });

        UnityAction onWriteBonesButtonClicked = () => { OnWriteBonesButtonClicked?.Invoke(); };
        _inversionProc.Register(
            () =>
            {
                _writeBonesButton.gameObject.SetActive(true);
                _writeBonesButton.onClick.AddListener(onWriteBonesButtonClicked);
            },
            () =>
            {
                _writeBonesButton.gameObject.SetActive(false);
                _writeBonesButton.onClick.RemoveListener(onWriteBonesButtonClicked);
            });
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
