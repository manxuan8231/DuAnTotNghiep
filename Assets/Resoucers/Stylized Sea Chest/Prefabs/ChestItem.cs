using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject destroy;
    public GameObject button;
    public Even even;
    void Start()
    {
        button.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            audioSource.PlayOneShot(audioClip);
            button.SetActive(true);
            
            if (even != null)
            {
                even.Even1(1);
                Destroy(destroy, 0.2f);
            }
 
        }
    }
}
