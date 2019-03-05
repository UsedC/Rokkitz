using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float mainThrust = 1100f;
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem[] engineTrailParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem winParticles;

    [SerializeField] GameObject[] parts;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool isTransitioning = false;

    private void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	private void Update () {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        for (int i = 0; i < engineTrailParticles.Length; i++)
        {
            engineTrailParticles[i].Stop();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKey(KeyCode.C))
        {
            if (rigidBody.detectCollisions)
            {
                rigidBody.detectCollisions = false;
            }
            else
            {
                rigidBody.detectCollisions = true;
            }
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
            for (int i = 0; i < engineTrailParticles.Length; i++)
            {
                engineTrailParticles[i].Play();
            }
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                // Refill
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
        isTransitioning = true;
        audioSource.Stop();
        for (int i = 0; i < engineTrailParticles.Length; i++)
        {
            engineTrailParticles[i].Stop();
        }

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(deathSound);
        explosionParticles.Play();
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].AddComponent<Rigidbody>();
        }
        transform.DetachChildren();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1 ? currentSceneIndex + 1 : 0;
        SceneManager.LoadScene(nextSceneIndex); 
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
