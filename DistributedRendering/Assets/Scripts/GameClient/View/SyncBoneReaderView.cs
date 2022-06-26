using System.Collections.Generic;
using MessagePackFormat;
using UnityEngine;

namespace GameClient
{

public class SyncBoneReaderView : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public List<TransformData> ReadTransforms()
    {
        var collection = new List<TransformData>();
        CollectTransforms(collection, transform);

        return collection;
    }

    private void CollectTransforms(List<TransformData> collection, Transform transform)
    {
        var data = new TransformData();
        data.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        data.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        data.scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        collection.Add(data);
        for (int index = 0; index < transform.childCount; ++index)
        {
            CollectTransforms(collection, transform.GetChild(index));
        }
    }
}

}
