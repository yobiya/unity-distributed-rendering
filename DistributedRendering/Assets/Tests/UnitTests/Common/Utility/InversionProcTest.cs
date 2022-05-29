using System.Collections.Generic;
using NUnit.Framework;

namespace Common
{

public class InversionProcTest
{
    [Test]
    public void Inversion()
    {
        var inversionProc = new InversionProc();

        var procResults = new List<int>();
        var inversionResults = new List<int>();

        // 登録して、第一引数の処理を実行
        inversionProc.Register(() => procResults.Add(1), () => inversionResults.Add(-1));
        inversionProc.Register(() => procResults.Add(2), () => inversionResults.Add(-2));
        inversionProc.Register(() => procResults.Add(3), () => inversionResults.Add(-3));

        // 第二引数の処理を逆順に実行
        inversionProc.Inversion();

        Assert.AreEqual(3, procResults.Count);
        Assert.AreEqual(1, procResults[0]);
        Assert.AreEqual(2, procResults[1]);
        Assert.AreEqual(3, procResults[2]);

        Assert.AreEqual(3, inversionResults.Count);
        Assert.AreEqual(-3, inversionResults[0]);
        Assert.AreEqual(-2, inversionResults[1]);
        Assert.AreEqual(-1, inversionResults[2]);
    }
}

}
