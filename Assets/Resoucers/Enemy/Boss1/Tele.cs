using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tele : MonoBehaviour
{
    public GameObject buttonF;
    public GameObject tele;
    public GameObject player;
    
    void Start()
    {
        buttonF.SetActive(false);
    }

   
    void Update()
    {
        if(buttonF.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
              player.transform.position = tele.transform.position;
              buttonF.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(false);
        }
    }
}
