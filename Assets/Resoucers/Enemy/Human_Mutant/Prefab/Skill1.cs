using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
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
            StartCoroutine(CoolDownTake());
          
        }
    }
    private IEnumerator CoolDownTake()
    {
        SliderHp sliderHp = FindAnyObjectByType<SliderHp>();
        yield return new WaitForSeconds(1);
        sliderHp.TakeDame(100);
        sliderHp.EnemyGetMana(300);
    }
}
