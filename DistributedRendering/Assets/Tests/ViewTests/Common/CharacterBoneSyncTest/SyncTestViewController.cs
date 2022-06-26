using System.Collections.Generic;
using GameClient;
using MessagePackFormat;
using RenderingServer;
using VContainer;

namespace Common
{

public class SyncTestViewController
{
    private readonly SyncBoneReaderView _syncBoneReaderView;
    private readonly SyncBoneWriterView _syncBoneWriterView;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public SyncTestViewController(SyncBoneReaderView syncBoneReaderView, SyncBoneWriterView syncBoneWriterView)
    {
        _syncBoneReaderView = syncBoneReaderView;
        _syncBoneWriterView = syncBoneWriterView;
    }

    public void Activate()
    {
        _inversionProc.Register(_syncBoneReaderView.Activate, _syncBoneReaderView.Deactivate);
        _inversionProc.Register(_syncBoneWriterView.Activate, _syncBoneWriterView.Deactivate);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public List<TransformData> ReadBoneTransforms()
    {
        return _syncBoneReaderView.ReadTransforms();
    }

    public void WriteBoneTransforms(List<TransformData> transforms)
    {
        _syncBoneWriterView.WriteTransforms(transforms);
    }
}

}
