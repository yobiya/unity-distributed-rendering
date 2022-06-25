using System;
using UnityEngine;

namespace GameClient
{

public class CharacterInputView : MonoBehaviour
{
    public event Action<bool, bool, bool, bool> OnInput;

    void Update()
    {
        bool front = false;
        bool back = false;
        bool left = false;
        bool right = false;

        if (Input.GetKey(KeyCode.W))
        {
            front = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            back = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }

        if (front || back || left || right)
        {
            OnInput?.Invoke(front, back, left, right);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
