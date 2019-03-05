using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 4f;

    float cycles;
    float rawSinWave;

    const float Tau = Mathf.PI * 2f;

    float movementFactor; //0 - not moved, 1 - fully moved

    Vector3 startingPos;

	void Start () {
        startingPos = transform.position;
	}
	
	void Update () {

        if (period <= Mathf.Epsilon) { return; }

        cycles = Time.time / period;

        rawSinWave = Mathf.Sin(cycles * Tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        transform.position = startingPos + movementVector * movementFactor;
	}
}
