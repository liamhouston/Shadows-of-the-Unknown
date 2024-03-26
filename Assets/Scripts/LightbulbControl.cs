using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightbulbControl : MonoBehaviour
{

    public Light2D light2D; // Reference to the Light2D component
    public float minIntensity = 0.01f; // Minimum intensity
    public float maxIntensity = 0.3f; // Maximum intensity
    
    public float flickerSpeed = 1.0f; // Speed of the flickering

    private void Start()
    {
        StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            // Wait for random seconds
            yield return new WaitForSeconds(Random.Range(1, 5));

            // Start flickering for random seconds
            float flickerDuration = Random.Range(1, 4);
            float startTime = Time.time;

            while (Time.time < startTime + flickerDuration)
            {
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * flickerSpeed, 1.0f));
                intensity += Random.Range(-0.05f, 0.05f); // Add a random factor
                light2D.intensity = intensity;
                yield return null;
            }

            // Go back to normal
            light2D.intensity = maxIntensity;
        }
    }
}
