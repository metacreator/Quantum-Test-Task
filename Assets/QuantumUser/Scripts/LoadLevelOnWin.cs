using Quantum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelOnWin : MonoBehaviour
{
    public void OnEnable()
    {
        QuantumEvent.Subscribe(this, (EventLevelComplete e) =>
        {
            SceneManager.LoadScene(2);
        });
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}