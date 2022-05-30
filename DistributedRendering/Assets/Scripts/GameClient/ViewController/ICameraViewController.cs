using System;
using UnityEngine;

namespace GameClient
{

public interface ICameraViewController
{
    event Action<Transform> OnUpdateTransform;
}

}
