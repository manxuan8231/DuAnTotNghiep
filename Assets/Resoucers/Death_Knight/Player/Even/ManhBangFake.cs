using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManhBangFake : MonoBehaviour
{
    public float health;
    public float maxHealth = 1f;
    public TextMeshProUGUI text;
    public GameObject spam;
    private AudioSource audioSource;
    public AudioClip audioClip; 
    void Start()
    {
        health = maxHealth;
        text.text = $"Đánh để lấy bảo vật";
        text.enabled = false;
        spam.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.text = $"Đánh để lấy bảo vật";
            text.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.enabled = false;
        }
    }
    public void TakeHealth(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            audioSource.PlayOneShot(audioClip);
            text.color = Color.red;
            text.text = $"oh no không có bảo vật";
            Destroy(gameObject,5f);
            spam.SetActive(true);
        }
    }
}
