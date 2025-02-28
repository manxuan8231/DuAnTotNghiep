using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoss2 : MonoBehaviour
{
    public GameObject boss2;
    void Start()
    {
        boss2.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boss2.SetActive(true);
        }
    }
}
