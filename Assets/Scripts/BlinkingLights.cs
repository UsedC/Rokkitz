using System.Collections;
using UnityEngine;

public class BlinkingLights : MonoBehaviour {

    [SerializeField] float blinkDelay = 2f;

    [SerializeField] Light[] lights;

	void Start () {
        StartCoroutine(Blink());
	}

    IEnumerator Blink()
    {
        while (true)
        {
            TurnOnLights();
            Invoke("TurnOffLights", 0.5f);
            yield return new WaitForSeconds(blinkDelay);
        }
    }

    void TurnOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].range = 1;
        }
    }

    void TurnOffLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].range = 0;
        }
    }
}
