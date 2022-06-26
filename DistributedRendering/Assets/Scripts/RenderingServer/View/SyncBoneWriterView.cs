using System.Collections.Generic;
using MessagePackFormat;
using UnityEngine;

namespace RenderingServer
{

public class SyncBoneWriterView : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void WriteTransforms(List<TransformData> overwriteTransforms)
    {
        ApplyTransforms(transform, overwriteTransforms);
    }

    private void ApplyTransforms(Transform transform, List<TransformData> overwriteTransforms)
    {
        if (overwriteTransforms.Count == 0)
        {
            return;
        }

        var overwriteTransform = overwriteTransforms[0];
        overwriteTransforms.RemoveAt(0);

        transform.position = overwriteTransform.position;
        transform.rotation = overwriteTransform.rotation;
        transform.localScale = overwriteTransform.scale;
        
        for (int index = 0; index < transform.childCount; ++index)
        {
            ApplyTransforms(transform.GetChild(index), overwriteTransforms);
        }
    }
}

}
