using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    // Tham chiếu đến hiệu ứng particle
    public GameObject hitEffect;
    public SkillPlayer1 player1;
    public SliderHp sliderHp;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu va chạm với đối tượng có tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Lấy thành phần EnemyAnimationController từ kẻ thù
            EnemyAnimationController rikayon = other.gameObject.GetComponent<EnemyAnimationController>();
            // Nếu tìm thấy EnemyAnimationController, thực hiện trừ máu 
            if (rikayon != null)
            {
                rikayon.TakeDamage(player1.currentDame);
            }

            //enemy1
            Enemy1 enemy1 = other.gameObject.GetComponent<Enemy1>();
            if (enemy1 != null)
            {
                enemy1.TakeDamage(player1.currentDame);
            }
            //than lan
            ThanLan thanlan = other.gameObject.GetComponent<ThanLan>();
            if(thanlan != null)
            {
                thanlan.TakeDamage(100);
            }
            //enemy3
            Enemy3 enemy3 = other.gameObject.GetComponent<Enemy3>();
            if(enemy3 != null)
            {
                enemy3.TakeDame(player1.currentDame);
            }
            //thuyQuai
            ThuyQuai thuyQuai = other.gameObject.GetComponent<ThuyQuai>();
            if (thuyQuai != null)
            {
                thuyQuai.TakeDame(player1.currentDame);
            }
            //enemy 4
            Enemy4 enemy4 = other.gameObject.GetComponent<Enemy4>();
            if (enemy4 != null)
            {
                enemy4.TakeDamage(player1.currentDame);
            }
           //bao vat
           ManhBangFake manhBangFake = other.gameObject.GetComponent<ManhBangFake>();
            if (manhBangFake != null)
            {
                manhBangFake.TakeHealth(player1.currentDame);
            }
            //bao vat
            ManhBang manhBang = other.gameObject.GetComponent<ManhBang>();
            if (manhBang != null)
            {
                manhBang.TakeHealth(player1.currentDame);
            }
            // Tạo hiệu ứng particle tại vị trí va chạm        
            GameObject effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, 3f); // Hủy hiệu ứng sau 3 giây

            // Cộng năng lượng khi đánh enemy
            sliderHp.AddUlti(100);

        }

        if (other.gameObject.CompareTag("Statue"))
        {
            Statue statue = other.gameObject.GetComponent<Statue>();
            statue.TakeDamage(player1.currentDame);
        }
        if (other.gameObject.CompareTag("Boss1"))
        {
            Boss1 boss1 = other.gameObject.GetComponent<Boss1>();
            if(boss1.onTakeHealth == true) {
                boss1.TakeHealth(player1.currentDame);
                sliderHp.AddUlti(100);
            }
           
        }
        if (other.gameObject.CompareTag("StatueBoss2"))
        {
            StatueBoss2 statueBoss2 = other.gameObject.GetComponent<StatueBoss2>();
            statueBoss2.TakeDamage(player1.currentDame);
            sliderHp.AddUlti(100);
        }
        
    }
}
