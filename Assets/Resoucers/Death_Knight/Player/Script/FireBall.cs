using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FireBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {          
            EnemyAnimationController rikayon = collision.gameObject.GetComponent<EnemyAnimationController>();
            if( rikayon != null ) {
                rikayon.TakeDamage(500);
                Destroy(gameObject);
            }
           
            Enemy1 enemy1 = collision.gameObject.GetComponent<Enemy1>();
            if( enemy1 != null )
            {
                enemy1.TakeDamage(500);
                Destroy(gameObject);
            }
            //than lan
            ThanLan thanlan = collision.gameObject.GetComponent<ThanLan>();
            if (thanlan != null)
            {
                thanlan.TakeDamage(500);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Statue"))
        {
            Statue statue = collision.gameObject.GetComponent<Statue>();
            statue.TakeDamage(500);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Boss1"))
        {
            Boss1 boss1 = collision.gameObject.GetComponent<Boss1>();
            if (boss1.onTakeHealth == true)
            {
                boss1.TakeHealth(500);
                Destroy(gameObject);
            }         
        }
    }
}
