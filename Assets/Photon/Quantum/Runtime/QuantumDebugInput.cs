using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Deterministic;

namespace Quantum {
  public class QuantumDebugInput : MonoBehaviour {
    public static Vector2 Move;
    private Vector2 _move;
    private bool _jump;
    private static int _jumpSeq;

    public static int GetJumpSeq() => _jumpSeq;

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput cb) => PollInput(cb));
    }

    private void OnDisable() {
      QuantumCallback.UnsubscribeListener(this);
    }

    private void Update() {
      var keyboard = Keyboard.current;
      var x = (keyboard.aKey.isPressed ? -1f : 0f) + (keyboard.dKey.isPressed ? 1f : 0f);
      var y = (keyboard.sKey.isPressed ? -1f : 0f) + (keyboard.wKey.isPressed ? 1f : 0f);
      _move = new Vector2(x, y);
      if (_move.sqrMagnitude > 1f) _move.Normalize();
      Move = _move;
      if (!keyboard.spaceKey.wasPressedThisFrame) return;
      _jump = true;
      _jumpSeq++;
    }

    private void PollInput(CallbackPollInput callback) {
      var input = new Quantum.Input { Direction = new FPVector2(_move.x.ToFP(), _move.y.ToFP()), Jump = _jump };
      _jump = false;
      callback.SetInput(input, DeterministicInputFlags.Repeatable);
    }
  }
}