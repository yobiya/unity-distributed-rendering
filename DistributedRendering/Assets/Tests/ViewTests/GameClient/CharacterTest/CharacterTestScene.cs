using UnityEngine;
using Common;
using VContainer;
using VContainer.Unity;

namespace GameClient
{

public class CharacterTestScene : MonoBehaviour
{
    [SerializeField]
    private CharacterView _characterView;

    [SerializeField]
    private CharacterInputView _characterInputView;

    [SerializeField]
    private Transform _cameraTransform;

    private IObjectResolver _objectResolver;
    private ICharacterViewController _characterViewController;

    void Awake()
    {
        DisableOnLoad.SetUp();
    }

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeField
            containerBuilder.RegisterComponent<CharacterView>(_characterView);
            containerBuilder.RegisterComponent<CharacterInputView>(_characterInputView);
            containerBuilder.RegisterComponent<Transform>(_cameraTransform);

            // ViewController
            containerBuilder.Register<ICharacterViewController, CharacterViewController>(Lifetime.Scoped);
        }

        _objectResolver = containerBuilder.Build();

        _characterViewController = _objectResolver.Resolve<ICharacterViewController>();

        _characterViewController.Activate();
    }
}

}
