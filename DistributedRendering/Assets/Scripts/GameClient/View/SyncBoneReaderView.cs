using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{

public class SyncBoneReaderView : MonoBehaviour
{
    public List<Transform> ReadTransforms()
    {
        var collection = new List<Transform>();
        CollectTransforms(collection, transform);

        return collection;
    }

    public void CollectTransforms(List<Transform> collection, Transform transform)
    {
        collection.Add(transform);
        for (int index = 0; index < transform.childCount; ++index)
        {
            CollectTransforms(collection, transform.GetChild(index));
        }
    }
}

}
