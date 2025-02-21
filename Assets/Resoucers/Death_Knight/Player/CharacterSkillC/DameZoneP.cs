using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DameZoneP : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
      
            if (other.gameObject.CompareTag("Boss1"))
            {
                Boss1 boss1 = other.gameObject.GetComponent<Boss1>();
                boss1.TakeHealth(2000);
            }
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAnimationController rikaron = other.gameObject.GetComponent<EnemyAnimationController>();
            if (rikaron != null)
            {
                rikaron.TakeDamage(500);
            }

            Enemy1 enemy1 = other.gameObject.GetComponent<Enemy1>();
            if (enemy1 != null)
            {
                enemy1.TakeDamage(500);
            }
            //than lan
            ThanLan thanlan = other.gameObject.GetComponent<ThanLan>();
            if (thanlan != null)
            {
                thanlan.TakeDamage(500);
            }
            //enemy3
            Enemy3 enemy3 = other.gameObject.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.TakeDame(100);
            }
        }
    }
}
