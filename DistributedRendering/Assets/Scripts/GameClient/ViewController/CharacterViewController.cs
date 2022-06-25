using Common;
using UnityEngine;

namespace GameClient
{

public interface ICharacterViewController
{
    void Activate();
    void Deactivate();
}

public class CharacterViewController : ICharacterViewController
{
    private readonly InversionProc _inversionProc = new InversionProc();
    private readonly CharacterView _characterView;
    private readonly CharacterInputView _characterInputView;
    private readonly Transform _camerTransform;

    public CharacterViewController(CharacterView characterView, CharacterInputView characterInputView, Transform camerTransform)
    {
        _characterView = characterView;
        _characterInputView = characterInputView;
        _camerTransform = camerTransform;
    }

    public void Activate()
    {
        _inversionProc.Register(_characterView.Activate, _characterView.Deactivate);
        _inversionProc.Register(_characterInputView.Activate, _characterInputView.Deactivate);
        _inversionProc
            .Register(
                () => _characterInputView.OnInput += MoveCharacter,
                () => _characterInputView.OnInput -= MoveCharacter);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    private void MoveCharacter(bool front, bool back, bool left, bool right)
    {
        // カメラの前方を移動方向の前方とする
        var forwordDirection = _camerTransform.forward;
        forwordDirection.y = 0.0f;
        forwordDirection = forwordDirection.normalized;

        Vector3 moveDirection = Vector3.zero;

        if (front)
        {
            moveDirection += forwordDirection;
        }

        if (back)
        {
            moveDirection -= forwordDirection;
        }

        if (left)
        {
            moveDirection -= Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        if (right)
        {
            moveDirection += Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        _characterView.Move(moveDirection.normalized);
    }
}

}
