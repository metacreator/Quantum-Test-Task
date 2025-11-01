using Photon.Deterministic;

namespace Quantum
{
    public unsafe class TriggerSystem : SystemMainThread,
        ISignalOnTriggerEnter3D,
        ISignalOnMapChanged
    {
        private bool _setBoxMassesNextFrame;
        
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
            
            _setBoxMassesNextFrame = true;
        }


        public override void Update(Frame f) {
            if (!_setBoxMassesNextFrame)
                return;
            _setBoxMassesNextFrame = false;

            int index = 0;

            foreach (var it in f.GetComponentIterator<PhysicsBody3D>()) {
                var entity = it.Entity;

                if (!f.Unsafe.TryGetPointer<PhysicsBody3D>(entity, out var body))
                    continue;

                FP m = index == 0 ? FP.FromFloat_UNSAFE(1f) :       
                    index == 1 ? FP.FromFloat_UNSAFE(10f) :     
                    FP.FromFloat_UNSAFE(50f);      


                body->Mass = m;
                body->Velocity = FPVector3.Zero;
                body->AngularVelocity = FPVector3.Zero;

                index++;
                if (index >= 3)
                    break;
            }
        }
        
    }
}