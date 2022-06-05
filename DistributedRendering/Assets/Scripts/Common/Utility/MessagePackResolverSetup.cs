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

        StaticCompositeResolver.Instance.Register(
            MessagePack.Resolvers.GeneratedResolver.Instance,
            MessagePack.Resolvers.StandardResolver.Instance);

        var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

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
