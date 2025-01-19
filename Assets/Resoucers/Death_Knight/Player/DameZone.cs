using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject effectHitDame;
    void Start()
    {
        effectHitDame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            effectHitDame.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            effectHitDame.SetActive(false);
        }
    }
}
