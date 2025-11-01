using UnityEngine;
using Photon.Deterministic;

namespace Quantum {
  public unsafe class MovementSystem
    : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerAdded {

    static readonly FP PushStrength = FP.FromFloat_UNSAFE(20f);
    static readonly FP PushRadius   = FP.FromFloat_UNSAFE(1f);
    static readonly FP HeightSlack  = FP.FromFloat_UNSAFE(1f);
    static readonly FP Zero         = FP.FromFloat_UNSAFE(0f);
    static readonly FP Epsilon      = FP.FromFloat_UNSAFE(0.001f);

    public override void Update(Frame frame, ref Filter filter) {
      var input = frame.GetPlayerInput(filter.Link->Player);
      var move2D = input->Direction;

      if (move2D.Magnitude > 1)
        move2D = move2D.Normalized;

      if (input->Jump.WasPressed)
        filter.KCC->Jump(frame);

      var move3D = new FPVector3(move2D.X, Zero, move2D.Y);
      filter.KCC->Move(frame, filter.Entity, move3D, null, null, null, null);

      if (move2D.SqrMagnitude == Zero)
        return;

      if (!frame.Unsafe.TryGetPointer<Transform3D>(filter.Entity, out var playerTr))
        return;

      ApplyPushes(frame, playerTr->Position, move2D, filter.Entity);
    }

    static void ApplyPushes(Frame frame, FPVector3 playerPos, FPVector2 dir, EntityRef playerEntity) {
      var radius2 = PushRadius * PushRadius;

      foreach (var it in frame.GetComponentIterator<PhysicsBody3D>()) {
        var e = it.Entity;
        if (e.Equals(playerEntity))
          continue;

        if (!frame.Unsafe.TryGetPointer<PhysicsBody3D>(e, out var body))
          continue;
        if (!frame.Unsafe.TryGetPointer<Transform3D>(e, out var tr))
          continue;

        var delta = tr->Position - playerPos;
        if (AbsFP(delta.Y) > HeightSlack)
          continue;
        if (delta.X * delta.X + delta.Z * delta.Z > radius2)
          continue;

        var mass = MaxFP(body->Mass, Epsilon);
        var impulse = new FPVector3(dir.X, Zero, dir.Y) * (PushStrength / mass) * frame.DeltaTime;
        body->Velocity += impulse;
      }
    }

    static FP AbsFP(FP v) => v >= Zero ? v : -v;
    static FP MaxFP(FP a, FP b) => a >= b ? a : b;

    public struct Filter {
      public EntityRef Entity;
      public Transform3D* Transform;
      public CharacterController3D* KCC;
      public PlayerLink* Link;
    }

    public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime) {
      var runtimePlayer = f.GetPlayerData(player);
      var entity = f.Create(runtimePlayer.PlayerAvatar);

      f.Add(entity, new PlayerLink { Player = player });

      if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var tr)) {
        int playerIndex = (int)player; // works in every Quantum build
        var offsetX = FP.FromFloat_UNSAFE(playerIndex * 2f);
        tr->Position = new FPVector3(offsetX, FP.FromFloat_UNSAFE(2f), Zero);
      }
    }
  }
}
