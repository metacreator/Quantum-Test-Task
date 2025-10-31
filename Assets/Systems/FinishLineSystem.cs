using Photon.Deterministic;

namespace Quantum {
    public unsafe class FinishLineSystem : SystemMainThread, ISignalOnTriggerEnter3D {
        public override void Update(Frame f) {
        }

        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info) {
            var a = info.Entity;
            var b = info.Other;

            var isFinishA = f.Has<FinishLine>(a);
            var other = isFinishA ? b : a;
            if (!isFinishA && !f.Has<FinishLine>(b))
                return;

            if (!f.Has<PlayerLink>(other))
                return;

            f.Events.LevelComplete();
        }
    }
}