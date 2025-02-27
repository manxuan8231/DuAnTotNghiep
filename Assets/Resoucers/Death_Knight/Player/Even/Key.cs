using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject button;
    public GameObject text;
    public TextMeshProUGUI textKey;
    private float keyCount = 0;
    void Start()
    {
        textKey.enabled = false;
        button.SetActive(false);
        text.SetActive(false);
    }

   
    void Update()
    {
        if(button.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                keyCount += 1;
                textKey.text = $"Key: {keyCount}";              
                button.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
           text.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);
            text.SetActive(false) ;
        }
    }
    public float KeyCount() 
    {
        return keyCount;
    }
}
