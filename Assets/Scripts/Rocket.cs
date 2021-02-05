using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [SerializeField]
    private float _rcsThrust = 100f;

    [SerializeField]
    private float _mainThrust = 100f;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            default:
                print("dead");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = _mainThrust * Time.deltaTime;
            _rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
        else if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _audioSource.time = 0f;
        }
    }

    private void Rotate()
    {
        // take manual control of rotation
        _rigidbody.freezeRotation = true;

        float rotationThisFrame = _rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        // give control back to physics engine
        _rigidbody.freezeRotation = false;
    }
}
