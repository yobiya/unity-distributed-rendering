using System;
using UnityEngine;

namespace GameClient
{

public interface ICameraView
{
    event Action<Transform> OnUpdateTransform;
}

}
