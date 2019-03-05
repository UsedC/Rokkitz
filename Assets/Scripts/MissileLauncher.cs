using System.Collections;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    [SerializeField] GameObject missileTube;
    [SerializeField] GameObject missileSpawn;
    [SerializeField] GameObject missilePrefab;

    [SerializeField] float warmUp = 1.5f;
    [SerializeField] float fireDelay = 4f;
    [SerializeField] float range = 30f;

    [SerializeField] LayerMask layerMask;

    bool playerIsInSight = false;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(LaunchMissile());
    }
	
	void FixedUpdate () {
        Vector3 rocketTransform = GameObject.Find("Rocket Ship").transform.position;

        SearchForPlayer(rocketTransform, range);

        if (playerIsInSight)
        {
            missileTube.transform.LookAt(rocketTransform);
            missileTube.transform.Rotate(90f, 0, 0);
        }
	}

    void SearchForPlayer(Vector3 playerPosition, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerPosition - transform.position, out hit, range, layerMask))
        {
            if (hit.collider.tag == "Player")
            {
                playerIsInSight = true;
            }
            else
            {
                playerIsInSight = false;
            }
        }
        else
        {
            playerIsInSight = false;
        }
    }
    
    IEnumerator LaunchMissile()
    {
        while (true)
        {
            if (playerIsInSight)
            {
                Invoke("SpawnMissile", warmUp);
                yield return new WaitForSeconds(fireDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnMissile()
    {
        if (playerIsInSight)
        {
            GameObject missile = Instantiate(missilePrefab, missileSpawn.transform.position, missileTube.transform.rotation);
            Collider[] missileColliders = missile.GetComponentsInChildren<Collider>();
            Collider[] tubeColliders = missileTube.GetComponentsInChildren<Collider>();
            for (int i = 0; i < missileColliders.Length; i++)
            {
                for (int j = 0; j < tubeColliders.Length; j++)
                {
                    Physics.IgnoreCollision(missileColliders[i], tubeColliders[j]);
                }
            }

            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
