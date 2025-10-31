using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Deterministic;

namespace Quantum {
  public class QuantumDebugInput : MonoBehaviour {
    private Vector2 _move;
    private bool _jumpPressed;

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput cb) => PollInput(cb));
      var kb = Keyboard.current;
      var gp = Gamepad.current;
    }

    private void OnDisable() {
      QuantumCallback.UnsubscribeListener(this);
    }

    private void Update() {
      var kb = Keyboard.current;
      var x = (kb.aKey.isPressed ? -1f : 0f) + (kb.dKey.isPressed ? 1f : 0f);
      var y = (kb.sKey.isPressed ? -1f : 0f) + (kb.wKey.isPressed ? 1f : 0f);
      _move = new Vector2(x, y);
      _jumpPressed |= kb.spaceKey.wasPressedThisFrame;

      if (_move.sqrMagnitude > 1f) _move.Normalize();
    }

    private void PollInput(CallbackPollInput callback) {
      if (callback.PlayerSlot > 0) return;

      var input = new Quantum.Input { Direction = new FPVector2(_move.x.ToFP(), _move.y.ToFP()), Jump = _jumpPressed };
      _jumpPressed = false;
      callback.SetInput(input, DeterministicInputFlags.Repeatable);
    }
  }
}