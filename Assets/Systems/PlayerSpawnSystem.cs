using Photon.Deterministic;

namespace Quantum {
    public unsafe class PlayerBootstrapSystem : SystemSignalsOnly, ISignalOnPlayerAdded {

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime) {
            var data = f.GetPlayerData(player);
            if (!data.PlayerAvatar.IsValid)
                return;

            foreach (var it in f.GetComponentIterator<PlayerLink>()) {
                if (f.Unsafe.TryGetPointer<PlayerLink>(it.Entity, out var pl) && pl->Player == player)
                    return;
            }

            var entity = f.Create(data.PlayerAvatar);
            f.Add(entity, new PlayerLink { Player = player });

            int index = 0;
            foreach (var _ in f.GetComponentIterator<PlayerLink>()) index++;

            if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var tr)) {
                tr->Position = new FPVector3(
                    FP.FromFloat_UNSAFE((index - 1) * 2f),
                    FP.FromFloat_UNSAFE(2f),
                    FP._0
                );
                tr->Rotation = FPQuaternion.Identity;
            }
        }
    }
}