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
            rikaron.TakeDamage(skillPlayer1.currentDameAir);
            // Tạo hiệu ứng particle tại vị trí va chạm        
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);
        }
        if (other.gameObject.CompareTag("Statue"))
        {
            Statue statue = other.gameObject.GetComponent<Statue>();
            statue.TakeDamage(skillPlayer1.currentDameAir);
        }
    }
}
