using Quantum;
using UnityEngine;

public class LoadNextLevelOnWin : MonoBehaviour
{
    public Transform FirstLevel;
    public Transform SecondLevel;

    private bool _firstActive = true;

    private void OnEnable()
    {
        QuantumEvent.Subscribe(this, (EventLevelComplete e) =>
        {
            if (_firstActive)
            {
                if (FirstLevel) FirstLevel.gameObject.SetActive(false);
                if (SecondLevel) SecondLevel.gameObject.SetActive(true);
                _firstActive = false;
            }
            else
            {
                if (FirstLevel) FirstLevel.gameObject.SetActive(true);
                if (SecondLevel) SecondLevel.gameObject.SetActive(false);
                _firstActive = true;
            }
        });
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}