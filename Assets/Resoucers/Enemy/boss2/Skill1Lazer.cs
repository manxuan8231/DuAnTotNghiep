using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Lazer : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
        }
    }
}
