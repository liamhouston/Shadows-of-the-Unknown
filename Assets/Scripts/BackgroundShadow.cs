using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShadow : MonoBehaviour
{
    public float leftBound;
    public float rightBound;

    public float waitTime = 5.0f;

    private bool shadowIsVisible = false;

    private void Start()
    {
        // wait before shadow appears in scene
        StartCoroutine(MoveShadow());
    }

    IEnumerator MoveShadow()
    {
        float newX = Random.Range(leftBound, rightBound);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        shadowIsVisible = false;

        yield return new WaitForSeconds(waitTime);

        shadowIsVisible = true;
        sr.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && shadowIsVisible)
        {
            SoundManager.Instance.PlaySound2D("ScaryAmbientWind");
            StartCoroutine(SoundManager.Instance.TurnOffSound(5));
            StartCoroutine(MoveShadow());
        }
    }
}
