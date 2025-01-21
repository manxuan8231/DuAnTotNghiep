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
    //spam dot 2
    public GameObject enemy8;
    public GameObject enemy9;
    public GameObject enemy10;
    public GameObject enemy11;
    public GameObject enemy12;
    public GameObject enemy13;
    public GameObject enemy14;

    private Statue tuong;

    void Start()
    {
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        enemy4.SetActive(false);
        enemy5.SetActive(false);
        enemy6.SetActive(false);
        enemy7.SetActive(false);

        enemy8.SetActive(false);
        enemy9.SetActive(false);
        enemy10.SetActive(false);
        enemy11.SetActive(false);
        enemy12.SetActive(false);
        enemy13.SetActive(false);
        enemy14.SetActive(false);

        // Khởi tạo biến tuong
        tuong = FindObjectOfType<Statue>();
    }

    void Update()
    {
        if (tuong != null && tuong.CurrentHealth() == 2500)
        {
            enemy8.SetActive(true);
            enemy9.SetActive(true);
            enemy10.SetActive(true);
            enemy11.SetActive(true);
            enemy12.SetActive(true);
            enemy13.SetActive(true);
            enemy14.SetActive(true);
            Debug.Log("spam dot 2");
        }
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
