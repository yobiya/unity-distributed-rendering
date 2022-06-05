using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace Common
{

public class MessagePackResolverSetup : MonoBehaviour
{
    private static bool _isSetuped = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Setup()
    {
        if (_isSetuped)
        {
            return;
        }

        var resolver
            = MessagePack.Resolvers.CompositeResolver.Create(
                MessagePack.Unity.Extension.UnityBlitResolver.Instance,
                MessagePack.Unity.UnityResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance);

        var option = MessagePackSerializerOptions.Standard.WithResolver(resolver);

        MessagePackSerializer.DefaultOptions = option;

        _isSetuped= true;
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void EditorSetup()
    {
        Setup();
    }
#endif
}

}
