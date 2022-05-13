using Cysharp.Threading.Tasks;

public interface ITimerCreator
{
    UniTask Create(float time);
}
