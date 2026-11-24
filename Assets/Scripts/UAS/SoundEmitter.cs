using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public LayerMask maskDuck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmitSound(float radiusSound)
    {
        Collider[] detectedDucks = Physics.OverlapSphere(transform.position, radiusSound, maskDuck);

        HashSet<Sensor> sensorListener = new HashSet<Sensor>();

        foreach (Collider duck in detectedDucks)
        {
            Sensor sensor = duck.GetComponentInParent<Sensor>();
            if (sensor != null && sensorListener.Add(sensor)) sensor.HearSound(transform.position);
        }
    }
}
