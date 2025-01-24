using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject destroy;
    public GameObject button;
    void Start()
    {
        button.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(audioClip);
            button.SetActive(true);
            Destroy(destroy,0.5f);           
        }
    }
}
