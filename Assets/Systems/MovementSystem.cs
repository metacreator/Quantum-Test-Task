using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerAdded
    {
        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.Link->Player);

            var movementDirection = input->Direction;
            if (movementDirection.Magnitude > 1)
            {
                movementDirection = movementDirection.Normalized;
            }

            if (input->Jump.WasPressed)
            {
                filter.KCC->Jump(frame);
            }

            filter.KCC->Move(frame, filter.Entity, movementDirection.XOY);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public CharacterController3D* KCC;
            public PlayerLink* Link;
        }
        
        

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);

            var link = new PlayerLink()
            {
                Player = player
            };

            f.Add(entity, link);
            if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var transform))
            {
                transform->Position = new FPVector3(player * 2, 2, 0);
            }
        }
    }
}