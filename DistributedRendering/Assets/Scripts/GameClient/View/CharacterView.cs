using UnityEngine;

namespace GameClient
{

public class CharacterView : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 3.0f;

    [SerializeField]
    private Rigidbody _rigidbody;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _moveSpeed);
        transform.forward = direction;
    }
}

}
