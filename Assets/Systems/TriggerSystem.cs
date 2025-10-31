using Photon.Deterministic;
using UnityEngine; // for Debug.Log

namespace Quantum {
    public unsafe class TriggerSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D {
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info) {
            f.Events.LevelComplete();
        }
    }
}