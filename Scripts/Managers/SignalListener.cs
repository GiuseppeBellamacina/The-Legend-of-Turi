using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public Signals signal;
    public UnityEvent response;

    public void OnSignalRaised()
    {
        response.Invoke();
    }

    void OnEnable()
    {
        signal.RegisterListener(this);
    }

    void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
