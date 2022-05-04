using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float _moveDistanceOfSecond = 1.0f;

    void Update()
    {
        Vector3 moveDistance = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDistance.z += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDistance.z -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDistance.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDistance.x += 1;
        }

        var position = transform.position;
        transform.position = position + moveDistance.normalized * _moveDistanceOfSecond * Time.deltaTime;
    }
}
