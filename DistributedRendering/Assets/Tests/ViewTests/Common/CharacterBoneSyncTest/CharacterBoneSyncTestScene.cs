using GameClient;
using RenderingServer;
using UnityEngine;

namespace Common
{

public class CharacterBoneSyncTestScene : MonoBehaviour
{
    [SerializeField]
    private SyncBoneReaderView _syncBoneReaderView;

    [SerializeField]
    private SyncBoneWriterView _syncBoneWriterView;

    void Awake()
    {
        DisableOnLoad.SetUp();
    }

    void Start()
    {
    }
}

}
