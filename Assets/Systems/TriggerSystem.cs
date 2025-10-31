using Photon.Deterministic;

namespace Quantum
{
    public unsafe class TriggerSystem : SystemMainThread,
        ISignalOnTriggerEnter3D,
        ISignalOnMapChanged
    {
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            if (!f.TryFindAsset(f.Map.UserAsset.Id, out MapMeta meta) || !meta.NextMap.IsValid) return;
            if (f.IsVerified)
                f.Map = f.FindAsset(meta.NextMap);
        }

        public void OnMapChanged(Frame f, AssetRef<Map> previousMap) {
            f.Events.LevelComplete();

            foreach (var it in f.GetComponentIterator<PlayerLink>()) {
                var entity = it.Entity;

                if (!f.Unsafe.TryGetPointer<PlayerLink>(entity, out var link))
                    continue;

                if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var transform)) {
                    transform->Position = new FPVector3(link->Player * 2, 2, 0);
                    transform->Rotation = FPQuaternion.Identity;
                }

                if (f.Unsafe.TryGetPointer<CharacterController3D>(entity, out var kcc)) {
                    kcc->Velocity = FPVector3.Zero;
                }
            }
        }


        public override void Update(Frame f)
        {
        }
    }
}