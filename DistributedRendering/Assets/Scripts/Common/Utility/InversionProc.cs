using System;
using System.Collections.Generic;

namespace Common
{

public class InversionProc
{
    private Stack<Action> _inversionStack = new Stack<Action>();

    public void Register(Action proc, Action inversion)
    {
        proc();
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
