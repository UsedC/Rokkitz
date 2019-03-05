using UnityEngine;

public class FallTrigger : MonoBehaviour {

    [SerializeField] GameObject spike;

    [SerializeField] float fallSpeed = 25f;

    bool isTriggered = false;

    void Update()
    {
        if (isTriggered && spike.transform.position.y > -100)
        {
            FallDown();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            spike.transform.parent = null;
        }
    }

    void FallDown()
    {
        spike.transform.Translate(0, -(fallSpeed * Time.deltaTime), 0);
    }
}
