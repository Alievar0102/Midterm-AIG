using UnityEngine;

public class MotherDuckFSM : MonoBehaviour
{
    public enum State
    {
        Idle, Patrol, Investigate, Chase, Attack
    }

    public State currentState;

    public float radiusAttack = 1f;

    public float minIdlePatrol = 5f;
    public float maxIdlePatrol = 10f;

    private float timerIdlePatrol;

    public Sensor sensor;

    public Transform fox;

    void Start()
    {
        currentState = State.Idle;
    }

    void Update()
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

    private void SetRandomTimer()
    {
        timerIdlePatrol = Random.Range(minIdlePatrol, maxIdlePatrol);
    }

    private void CheckIdle()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (sensor.isFoxHeard) ChangeState(State.Investigate);
        else
        {
            timerIdlePatrol -= Time.deltaTime;
            if (timerIdlePatrol <= 0f) ChangeState(State.Patrol);
        }
    }

    private void CheckPatrol()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (sensor.isFoxHeard) ChangeState(State.Investigate);
        else
        {
            timerIdlePatrol -= Time.deltaTime;
            if (timerIdlePatrol <= 0f) ChangeState(State.Idle);
        }
    }

    private void CheckInvestigate()
    {
        if (sensor.isFoxSeen) ChangeState(State.Chase);
        else if (!sensor.isFoxHeard) ChangeState(State.Idle);
    }

    private void CheckChase()
    {
        if (!sensor.isFoxSeen) ChangeState(State.Investigate);
        else if (Vector3.Distance(transform.position, fox.position) <= radiusAttack)
            ChangeState(State.Attack);
    }

    private void CheckAttack()
    {
        if (!sensor.isFoxSeen) ChangeState(State.Investigate);
        else if (Vector3.Distance(transform.position, fox.position) > radiusAttack)
            ChangeState(State.Chase);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (currentState == State.Idle || currentState == State.Patrol)
            SetRandomTimer();
    }
}
