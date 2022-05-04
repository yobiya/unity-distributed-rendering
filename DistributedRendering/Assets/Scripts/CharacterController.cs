using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float _moveDistanceOfSecond = 1.0f;

    [SerializeField]
    Transform _cameraTransform;

    void Update()
    {
        // カメラの前方を移動方向の前方とする
        var forwordDirection = transform.position - _cameraTransform.transform.position;
        forwordDirection.y = 0.0f;
        forwordDirection = forwordDirection.normalized;

        Vector3 moveDistance = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDistance += forwordDirection;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDistance -= forwordDirection;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDistance -= Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDistance += Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        var position = transform.position;
        transform.position = position + moveDistance.normalized * _moveDistanceOfSecond * Time.deltaTime;
    }
}
