using UnityEngine;

public class Missile : MonoBehaviour {

    [SerializeField] float speed = 60f;

    [SerializeField] GameObject parts;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem explosionParticles;

    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip engineSound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State
    {
        Alive,
        Dead
    }

    State state = State.Alive;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void FixedUpdate () {
        if (state == State.Alive)
        ApplyThrust();
	}

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * speed);
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = 1.2f;
            audioSource.PlayOneShot(engineSound);
            engineParticles.Play();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode() //Simulatenously destroys missile components, plays explosion particles, and finally destroys the object.
    {
        audioSource.Stop();
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(explosionSound);
        state = State.Dead;
        Destroy(parts);
        Destroy(engineParticles, 0.2f);
        explosionParticles.Play();
        Destroy(gameObject, 0.5f);
    }
}
