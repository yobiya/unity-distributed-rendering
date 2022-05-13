using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TestTimerCreator : ITimerCreator
{
    private List<CancellationTokenSource> _cancelationTokenSourceList;

    public async UniTask Create(float time)
    {
        var source = new CancellationTokenSource();
        _cancelationTokenSourceList.Add(source);

        await UniTask.Never(source.Token);
    }

    public void EndTimer(int index)
    {
        _cancelationTokenSourceList[index].Cancel();
    }
}
