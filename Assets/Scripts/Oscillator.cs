using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _movementVector = new Vector3(10, 10, 10);

    [SerializeField]
    private float _period = 2f;

    private float _movementFactor;
    private Vector3 _startingPosition;

    private const float _tau = Mathf.PI * 2f; // about 6.28

    void Start()
    {
        _startingPosition = transform.position;
    }

    void Update()
    {
        // TODO: protect against 0 _period value

        // Infinitely grows
        var cycles = Time.time / _period;

        // rawSinWave goes from -1 to +1
        var rawSinWave = Mathf.Sin(cycles * _tau);

        // make the rawSinWave go between 0 and 1 instead of -1 and +1 so we can use it in our displacement
        _movementFactor = rawSinWave / 2f + 0.5f;

        var displacement = _movementVector * _movementFactor;
        transform.position = _startingPosition + displacement;
    }
}
