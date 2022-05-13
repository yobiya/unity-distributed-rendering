using Cysharp.Threading.Tasks;

public class TimerCreator : ITimerCreator
{
    public async UniTask Create(float time)
    {
        await UniTask.Delay((int)(time * 1000));
    }
}
