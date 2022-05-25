using System;
using UnityEngine;

namespace Common
{

public class CameraViewAdapter : BaseViewAdapter, ICameraView
{
    [SerializeField]
    private Camera _camera;
}

}
