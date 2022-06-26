using System.Collections.Generic;
using GameClient;
using MessagePackFormat;
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

    private List<TransformData> _boneTransforms;

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
        _syncTestViewController.Activate();

        _syncTestUIController.OnReadBonesButtonClicked += () => _boneTransforms = _syncTestViewController.ReadBoneTransforms();
        _syncTestUIController.OnWriteBonesButtonClicked += () => _syncTestViewController.WriteBoneTransforms(_boneTransforms);
    }
}

}
