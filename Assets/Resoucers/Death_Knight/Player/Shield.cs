using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float radius = 10f; // Phạm vi tác động của Shield
    public float pullSpeed = 5f; // Tốc độ hút đối tượng
    public int damage = 10; // Số lượng HP bị mất

    void Update()
    {
        // Tìm tất cả các đối tượng có tag "Enemy" và "Boss"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1");

        // Kiểm tra khoảng cách và hút các đối tượng nếu trong phạm vi
        foreach (GameObject enemy in enemies)
        {
            PullAndDamage(enemy);
        }

        foreach (GameObject boss in bosses)
        {
            PullAndDamage(boss);
        }
    }

    void PullAndDamage(GameObject target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Nếu khoảng cách nhỏ hơn hoặc bằng bán kính, hút và gây sát thương
        if (distance <= radius)
        {
            // Hút đối tượng vào nhân vật
            Vector3 direction = (transform.position - target.transform.position).normalized;
            target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, pullSpeed * Time.deltaTime);

            // Giảm HP của đối tượng
           
           
        }
    }
}
