using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleNPC1 : MonoBehaviour
{
    AudioSource audioSource;

    private bool canSpeak = true;
    public float speechInterval = 10f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void EnableSpeak()
    {
        canSpeak = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!canSpeak) return;

        Debug.Log("On Trigger Enter");
        audioSource.Play();
        canSpeak = false;
        Invoke("EnableSpeak", speechInterval);

        transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Vector3.up);
    }
}
