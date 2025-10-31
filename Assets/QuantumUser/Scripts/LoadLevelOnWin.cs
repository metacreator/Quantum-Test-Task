using Quantum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelOnWin : MonoBehaviour
{
    public void OnEnable()
    {
        QuantumEvent.Subscribe(this, (EventLevelComplete e) =>
        {
            Debug.Log("[LoadNextLevelOnWin] EventLevelComplete received — loading next scene");
            SceneManager.LoadScene(2);
        });
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}