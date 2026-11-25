using System.Collections;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float radiusSight;
    [Range(0, 360)] public float angleSight;

    public float radiusHearing;
    public Vector3 lastHeardPosition;
    public float lastHeardTime;
    public float soundMemory;

    public Transform fox;
    public Transform duckEyes;

    public LayerMask maskFox;
    public LayerMask maskObstacle;

    public bool isFoxSeen;
    public bool isFoxHeard;

    public bool IsFoxDetected { get { return isFoxSeen || isFoxHeard; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SensoryRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (isFoxHeard && (Time.time - lastHeardTime > soundMemory)) ClearSound();
    }

    private IEnumerator SensoryRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            SensorCheck();
        }
    }

    private void SensorCheck()
    {
        CheckSight();
    }

    private void CheckSight()
    {
        isFoxSeen = false;

        if (fox == null || duckEyes == null) return;

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radiusSight, maskFox);

        if (rangeChecks.Length != 0)
        {
            Vector3 directionToFox = (fox.position - duckEyes.position).normalized;

            if (Vector3.Angle(duckEyes.forward, directionToFox) < (angleSight / 2))
            {
                float distanceToFox = Vector3.Distance(duckEyes.position, fox.position);

                if (!Physics.Raycast(duckEyes.position, directionToFox, distanceToFox, maskObstacle))
                    isFoxSeen = true;
                else isFoxSeen = false;
            }
            else isFoxSeen = false;
        }
    }

    public void HearSound(Vector3 soundPosition)
    {
        float soundDistance = Vector3.Distance(transform.position, soundPosition);

        if (soundDistance <= radiusHearing)
        {
            isFoxHeard = true;
            lastHeardPosition = soundPosition;
            lastHeardTime = Time.time;
        }
    }

    public void ClearSound()
    {
        isFoxHeard = false;
    }
}
