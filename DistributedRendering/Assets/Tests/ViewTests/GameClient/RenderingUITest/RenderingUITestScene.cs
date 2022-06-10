using UnityEngine;
using Common;
using MessagePackFormat;

namespace GameClient
{

public class RenderingUITestScene : MonoBehaviour
{
    [SerializeField]
    private RenderingUI _renderingUI;

    [SerializeField]
    private Camera _camera;

    private RenderingUIController _sut;

    void Awake()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(SystemDefinisions.DisableOnLoadTag);
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
    }

    void Start()
    {
        _sut = new RenderingUIController(_renderingUI);

        var setupData = new SetupData();
        setupData.renderingRect = new RectInt(RenderingDefinisions.RenderingTextureWidth / 2, 0, RenderingDefinisions.RenderingTextureWidth / 2, RenderingDefinisions.RenderingTextureHight);

        _sut.Activate(_camera, setupData);
    }

    void Update()
    {
        // 自分の担当する範囲の描画を行う
        _sut.RenderBaseImage();
    }
}

}
