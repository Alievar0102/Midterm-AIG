using UnityEngine;

public class DucklingFSM : MonoBehaviour
{
    public enum State
    {
        Idle, Play, Alert, Flee
    }

    public State currentState;

    public Sensor sensor;

    public Transform fox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                CheckIdle();
                break;

            case State.Play:
                CheckPlay();
                break;

            case State.Alert:
                CheckAlert();
                break;

            case State.Flee:
                CheckFlee();
                break;
        }
    }

    private void CheckIdle()
    {
        if (sensor.isFoxSeen) ChangeState(State.Flee);
        else if (sensor.isFoxHeard) ChangeState(State.Alert);
    }

    private void CheckPlay()
    {
        if (sensor.isFoxSeen) ChangeState(State.Flee);
        else if (sensor.isFoxHeard) ChangeState(State.Alert);
    }

    private void CheckAlert()
    {
        if (sensor.isFoxSeen) ChangeState(State.Flee);
        else if (!sensor.isFoxHeard) ChangeState(State.Idle);
    }

    private void CheckFlee()
    {
        if (!sensor.isFoxSeen && !sensor.isFoxHeard) ChangeState(State.Idle);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }
}
