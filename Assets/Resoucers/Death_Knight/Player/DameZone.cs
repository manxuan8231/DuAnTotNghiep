using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    // Tham chiếu đến hiệu ứng particle
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu va chạm với đối tượng có tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Lấy thành phần EnemyAnimationController từ kẻ thù
            EnemyAnimationController rikayon = other.gameObject.GetComponent<EnemyAnimationController>();

            // Nếu tìm thấy EnemyAnimationController, thực hiện trừ máu         
            rikayon.TakeDamage(100);
           
            // Tạo hiệu ứng particle tại vị trí va chạm        
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);          
        }
    }
}
