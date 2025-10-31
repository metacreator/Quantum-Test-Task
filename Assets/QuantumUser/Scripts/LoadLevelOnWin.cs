using Quantum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelOnWin : MonoBehaviour {
    private void OnEnable() {
        QuantumEvent.Subscribe(this, (EventLevelComplete e) => {
            SceneManager.LoadScene(1);
        });
    }

    private void OnDisable() {
        QuantumEvent.UnsubscribeListener(this);
    }
}