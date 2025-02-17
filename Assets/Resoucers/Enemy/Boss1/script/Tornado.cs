using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSkill3 : MonoBehaviour
{
    private SliderHp sliderHp;
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
            sliderHp = other.gameObject.GetComponent<SliderHp>();
            sliderHp.TakeDame(100);
            Destroy(gameObject, 3f);
        }
    }
}
