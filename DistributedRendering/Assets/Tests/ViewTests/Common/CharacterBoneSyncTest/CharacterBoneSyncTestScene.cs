using GameClient;
using RenderingServer;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Common
{

public class CharacterBoneSyncTestScene : MonoBehaviour
{
    [SerializeField]
    private SyncBoneReaderView _syncBoneReaderView;

    [SerializeField]
    private SyncBoneWriterView _syncBoneWriterView;

    [SerializeField]
    private Button ReadSyncTransformButton;

    [SerializeField]
    private Button WriteSyncTransformButton;

    private IObjectResolver _objectResolver;
    private SyncTestUIController _syncTestUIController;
    private SyncTestViewController _syncTestViewController;

    void Awake()
    {
        DisableOnLoad.SetUp();
    }

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeField
            containerBuilder.RegisterComponent<SyncBoneReaderView>(_syncBoneReaderView);
            containerBuilder.RegisterComponent<SyncBoneWriterView>(_syncBoneWriterView);

            // ViewController
            containerBuilder.Register<SyncTestViewController>(Lifetime.Scoped);
        }

        _objectResolver = containerBuilder.Build();

        _syncTestUIController = new SyncTestUIController(ReadSyncTransformButton, WriteSyncTransformButton);
        _syncTestViewController = _objectResolver.Resolve<SyncTestViewController>();

        _syncTestUIController.Activate();
    }
}

}
