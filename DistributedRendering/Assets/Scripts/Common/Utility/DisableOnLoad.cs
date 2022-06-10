using UnityEngine;

namespace Common
{

public class DisableOnLoad
{
    static public void SetUp()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(SystemDefinisions.DisableOnLoadTag);
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
    }
}

}
