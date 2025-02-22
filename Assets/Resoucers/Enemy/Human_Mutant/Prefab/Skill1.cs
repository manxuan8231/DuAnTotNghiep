using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public SliderHp sliderHp;


    void Start()
    {

    }
  
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sliderHp.EnemyGetHp(30);
        }
    }
}
