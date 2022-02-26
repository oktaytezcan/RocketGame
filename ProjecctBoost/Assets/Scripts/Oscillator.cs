using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
     float movementfactor;
    [SerializeField] float period = 2f;
    
    void Start()
    {
        startingPosition = transform.position;
    }

    
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }//nan nan nan errroru kaldýrdýk.
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementfactor = (rawSinWave + 1f) / 2f;
        Vector3 offset = movementfactor * movementVector;
        transform.position = startingPosition + offset;
    }
}
