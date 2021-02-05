using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private State _state = State.Alive;

    [SerializeField]
    private float _rcsThrust = 100f;

    [SerializeField]
    private float _mainThrust = 100f;

    [SerializeField]
    private AudioClip _mainEngineSound;

    [SerializeField]
    private AudioClip _deathSound;

    [SerializeField]
    private AudioClip _successSound;

    [SerializeField]
    private ParticleSystem _engineParticles;

    [SerializeField]
    private ParticleSystem _deathParticles;

    [SerializeField]
    private ParticleSystem _successParticles;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_state != State.Alive) return;
        RespondToThrustInput();
        RespondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successSound);

        if (_engineParticles.isPlaying)
            _engineParticles.Stop();

        _successParticles.Play();

        // TODO: Parameterize time
        Invoke(nameof(LoadNextLevel), 1f);
    }

    private void StartDeathSequence()
    {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_deathSound);

        if (_engineParticles.isPlaying)
            _engineParticles.Stop();

        _deathParticles.Play();
        // TODO: Parameterize time
        Invoke(nameof(LoadFirstLevel), 1f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        // TODO: Allow for more levels
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(_mainEngineSound);

            if (!_engineParticles.isPlaying)
                _engineParticles.Play();
        }
        else if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _engineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = _mainThrust * Time.deltaTime;
        _rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
    }

    private void RespondToRotateInput()
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
