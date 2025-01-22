using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamEnemy : MonoBehaviour
{
    public BoxCollider box;
    public GameObject slider;
    public GameObject enemy1;
   
    //spam dot 2
    public GameObject enemy2;
   

    private Statue tuong;

    void Start()
    {
        enemy1.SetActive(false);
        

        enemy2.SetActive(false);
        

        // Khởi tạo biến tuong
        tuong = FindObjectOfType<Statue>();
    }

    void Update()
    {
        if (tuong != null && tuong.CurrentHealth() == 2500)
        {
            enemy2.SetActive(true);           
            Debug.Log("spam dot 2");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy1.SetActive(true);          
            Debug.Log("đã spame 1");
            box.enabled = false;
            slider.SetActive(true);
        }
    }
}
