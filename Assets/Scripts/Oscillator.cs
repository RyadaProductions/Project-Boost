using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _movementVector;

    // TODO: Remove from inspector later
    // 0 is not moved, 1 is fully moved
    [Range(0,1)]
    [SerializeField]
    private float _movementFactor;

    private Vector3 _startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var displacement = _movementVector * _movementFactor;
        transform.position = _startingPosition + displacement;
    }
}
