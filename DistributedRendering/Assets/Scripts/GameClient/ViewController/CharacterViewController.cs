namespace GameClient
{

public interface ICharacterViewController
{
    public void Activate();
    public void Deactivate();
}

public class CharacterViewController : ICharacterViewController
{
    private readonly CharacterView _characterView;
    private readonly CharacterInputView _characterInputView;

    public CharacterViewController(CharacterView characterView, CharacterInputView characterInputView)
    {
        _characterView = characterView;
        _characterInputView = characterInputView;
    }

    public void Activate()
    {
        _characterView.Activate();
        _characterInputView.Activate();
    }

    public void Deactivate()
    {
        _characterView.Deactivate();
        _characterInputView.Deactivate();
    }
}

}
