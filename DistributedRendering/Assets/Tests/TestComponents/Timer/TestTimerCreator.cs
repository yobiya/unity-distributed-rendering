using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TestTimerCreator : ITimerCreator
{
    private List<UniTaskCompletionSource> _sourceList = new List<UniTaskCompletionSource>();

    public async UniTask Create(float time)
    {
        var completionSource = new UniTaskCompletionSource();
        _sourceList.Add(completionSource);

        await completionSource.Task;
    }

    public void EndTimer(int index)
    {
        // タスクを終了させる
        _sourceList[index].TrySetResult();
    }
}
