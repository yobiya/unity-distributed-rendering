using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Common
{

public class InversionProc
{
    private Stack<Action> _inversionStack = new Stack<Action>();

    public void Register(Action proc, Action inversion)
    {
        _inversionStack.Push(inversion);
        proc();
    }

    public async UniTask RegisterAsync(UniTask task, Action inversion)
    {
        // taskが途中で中断した場合でも、inversionが動作するように先に登録する
        _inversionStack.Push(inversion);
        await task;
    }

    public void RegisterInversion(Action inversion)
    {
        _inversionStack.Push(inversion);
    }

    public void Inversion()
    {
        foreach (var proc in _inversionStack)
        {
            proc();
        }

        _inversionStack.Clear();
    }
}

}
