using UnityEngine;

public class MotherDuckFSM : Singleton<MotherDuckFSM>
{
    public enum State
    {
        Idle, Patrol, Investigate, Chase, Attack
    }

    public State currentState = State.Idle;

    public Sensor sensor;

    public Transform fox;

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                CheckIdle();
                break;

            case State.Patrol:
                CheckPatrol();
                break;

            case State.Investigate:
                CheckInvestigate();
                break;

            case State.Chase:
                CheckChase();
                break;

            case State.Attack:
                CheckAttack();
                break;
        }
    }

    private void CheckIdle()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (sensor.isFoxHeard) ChangeState(State.Investigate);
    }

    private void CheckPatrol()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (sensor.isFoxHeard) ChangeState(State.Investigate);
    }

    private void CheckInvestigate()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (!sensor.isFoxHeard) ChangeState(State.Idle);
    }

    private void CheckChase()
    {

    }

    private void CheckAttack()
    {

    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }
}
