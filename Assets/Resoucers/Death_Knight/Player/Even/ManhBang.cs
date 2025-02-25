using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManhBang : MonoBehaviour
{
    public float health;
    public float maxHealth = 1f;
    public TextMeshProUGUI text;
    private AudioSource audioSource;
    public AudioClip audioClip;
    void Start()
    {
        health = maxHealth;
        text.text = $"Đánh để lấy bảo vật";
        text.enabled = false;
       
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.text = $"Đánh để lấy mảnh băng";
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
        if (health <= 0)
        {
           
            Even2 even2 = FindAnyObjectByType<Even2>();
            even2.manhBang += 1;
            even2.textManhBang.text = $"Mảnh băng:{even2.manhBang}/{1}";
            audioSource.PlayOneShot(audioClip);
            text.color = Color.blue;
            text.text = $"Bạn nhận được một Mảnh băng";
            Destroy(gameObject);

        }
    }
}
