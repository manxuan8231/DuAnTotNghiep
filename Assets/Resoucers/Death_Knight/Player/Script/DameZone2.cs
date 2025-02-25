using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DameZone2 : MonoBehaviour
{
    // Tham chiếu đến hiệu ứng particle
    public GameObject hitEffect;
    public SkillPlayer1 skillPlayer1;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAnimationController rikaron = other.gameObject.GetComponent<EnemyAnimationController>();
            if(rikaron != null)
            {
                rikaron.TakeDamage(skillPlayer1.currentDameAir);
            }
            
            Enemy1 enemy1 = other.gameObject.GetComponent<Enemy1>();
            if(enemy1 != null)
            {
                enemy1.TakeDamage(skillPlayer1.currentDameAir);
            }
            //than lan
            ThanLan thanlan = other.gameObject.GetComponent<ThanLan>();
            if (thanlan != null)
            {
                thanlan.TakeDamage(skillPlayer1.currentDameAir);
            }
            //enemy3
            Enemy3 enemy3 = other.gameObject.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.TakeDame(skillPlayer1.currentDameAir);
            }
            //thuyQuai
            ThuyQuai thuyQuai = other.gameObject.GetComponent<ThuyQuai>();
            if (thuyQuai != null)
            {
                thuyQuai.TakeDame(skillPlayer1.currentDameAir);
            }
            
            // Tạo hiệu ứng particle tại vị trí va chạm        
            GameObject effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, 3f); // Hủy hiệu ứng sau 3 giây
        }

        if (other.gameObject.CompareTag("Statue"))
        {
            Statue statue = other.gameObject.GetComponent<Statue>();
            statue.TakeDamage(skillPlayer1.currentDameAir);
        }

        if (other.gameObject.CompareTag("Boss1"))
        {
            Boss1 boss1 = other.gameObject.GetComponent<Boss1>();
            if(boss1.onTakeHealth == true)
            {
                boss1.TakeHealth(skillPlayer1.currentDameAir);
            }
          
        }
       
        if (other.gameObject.CompareTag("StatueBoss2"))
        {
            StatueBoss2 statueBoss2 = other.gameObject.GetComponent<StatueBoss2>();
            statueBoss2.TakeDamage(skillPlayer1.currentDameAir);
           
        }

    }
}
