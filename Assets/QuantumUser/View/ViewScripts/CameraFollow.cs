using Quantum;
using UnityEngine;

public class CameraFollow : QuantumEntityViewComponent<GameContext>
{
    public Vector3 offset;
    private bool _isLocalPlayer;

    public override void OnActivate(Frame frame)
    {
        var link = frame.Get<PlayerLink>(EntityRef);
        _isLocalPlayer = Game.PlayerIsLocal(link.Player);
    }

    public override void OnUpdateView()
    {
        if (!_isLocalPlayer) return;
        ViewContext.MainCamera.transform.position = transform.position + offset;
        ViewContext.MainCamera.transform.LookAt(transform);
    }
}