using UnityEngine;

namespace Quantum {
    public unsafe class AnimationView : QuantumEntityViewComponent<GameContext> {
        public Transform visual;
        public Animator animator;
        public string speedParam = "Speed";
        public string jumpParam = "Jumping";
        public float rotateSpeed = 12f;
        public float minInput = 0.05f;
        public float jumpBoolDuration = 0.25f;

        private Vector3 _lastPos;
        private bool _init;
        private int _lastJumpSeq = -1;
        private float _jumpTimer;

        public override void OnInitialize() {
            base.OnInitialize();
            _init = false;
            _jumpTimer = 0f;
            if (animator) animator.SetBool(jumpParam, false);
        }

        public override unsafe void OnUpdateView() {
            if (!animator) return;

            var f = Game.Frames.Predicted;
            if (f == null) return;
            if (!f.Unsafe.TryGetPointer<PlayerLink>(EntityRef, out var link)) return;
            if (!Game.PlayerIsLocal(link->Player)) return;  

            var input = QuantumDebugInput.Move;
            var t = visual ? visual : transform;

            if (input.sqrMagnitude >= minInput * minInput) {
                var dir = new Vector3(input.x, 0f, input.y);
                var target = Quaternion.LookRotation(dir.normalized, Vector3.up);
                t.rotation = Quaternion.Slerp(t.rotation, target, rotateSpeed * Time.deltaTime);
            }

            var spd = input.magnitude;
            animator.SetFloat(speedParam, spd);

            var seq = QuantumDebugInput.GetJumpSeq();
            if (seq != _lastJumpSeq) {
                _lastJumpSeq = seq;
                _jumpTimer = jumpBoolDuration;
                animator.SetBool(jumpParam, true);
            }

            if (!(_jumpTimer > 0f)) return;
            _jumpTimer -= Time.deltaTime;
            if (_jumpTimer <= 0f) animator.SetBool(jumpParam, false);
        }
    }
}