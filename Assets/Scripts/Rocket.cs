using UnityEngine;
using UnityEngine.SceneManagement;

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

    private State _state = State.Alive;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // TODO: Stop sound on death
        if (_state != State.Alive) return;
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                _state = State.Transcending;
                // TODO: Parameterize time
                Invoke(nameof(LoadNextLevel), 1f);
                break;
            default:
                _state = State.Dying;
                // TODO: Parameterize time
                Invoke(nameof(LoadFirstLevel), 1f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        // Todo: Allow for more levels
        SceneManager.LoadScene(1);
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
