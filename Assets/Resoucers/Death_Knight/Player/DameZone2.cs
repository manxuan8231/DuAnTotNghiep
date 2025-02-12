using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            boss1.TakeHealth(skillPlayer1.currentDameAir);
        }
    }
}
