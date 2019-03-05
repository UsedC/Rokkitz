using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    [SerializeField] Vector3 offset;
	
	void Update () {
        transform.position = player.position + offset;
	}
}
