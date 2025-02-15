using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameZoneBoss : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SliderHp sliderHp = other.gameObject.GetComponent<SliderHp>();
            sliderHp.TakeDame(50);
        }
    }
}
