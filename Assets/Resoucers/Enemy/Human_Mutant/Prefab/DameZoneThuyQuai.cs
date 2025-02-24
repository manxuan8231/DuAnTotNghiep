using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameZoneThuyQuai : MonoBehaviour
{
   
    public AudioSource audioSource;
    public AudioClip audioClipAttack;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        SliderHp sliderHp = FindAnyObjectByType<SliderHp>();
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("lấy máu");
            audioSource.PlayOneShot(audioClipAttack);
            sliderHp.TakeDame(20);           
        }
    }
}
