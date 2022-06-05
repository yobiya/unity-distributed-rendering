using System;
using UnityEngine;

namespace GameClient
{

public class CameraView : MonoBehaviour, ICameraView
{
    [SerializeField]
    float _moveDistanceOfSecond = 1.0f;

    [SerializeField]
    private Transform _targetTransform;

    public event Action<Transform> OnUpdateTransform;

    void Update()
    {
        // 対象の向き
        var forwordDirection = _targetTransform.position - transform.position;
        forwordDirection = forwordDirection.normalized;

        Vector3 moveDistance = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDistance += forwordDirection;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDistance -= forwordDirection;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDistance -= Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDistance += Vector3.Cross(Vector3.up, forwordDirection).normalized;
        }

        if (moveDistance == Vector3.zero)
        {
            return;
        }

        var position = transform.position;
        transform.position = position + moveDistance.normalized * _moveDistanceOfSecond * Time.deltaTime;

        transform.LookAt(_targetTransform);
    }
}

}
