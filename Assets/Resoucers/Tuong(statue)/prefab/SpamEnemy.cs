using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamEnemy : MonoBehaviour
{
    public BoxCollider box;
    public GameObject slider;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;
    public GameObject enemy7;

    void Start()
    {
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        enemy4.SetActive(false);
        enemy5.SetActive(false);
        enemy6.SetActive(false);
        enemy7.SetActive(false);
    }

    
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy1.SetActive(true);
            enemy2.SetActive(true);
            enemy3.SetActive(true);               
            enemy4.SetActive(true);
            enemy5.SetActive(true);
            enemy6.SetActive(true);
            enemy7.SetActive(true);
            Debug.Log("đã spame");
            box.enabled = false;
            slider.SetActive(true);
        }
    }
}
