using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Transform player;// khai báo vị trí player
    [SerializeField] float radius = 30f; // nếu player đứng quá 30f thì sẽ bắn skill
    [SerializeField] float cantSkill = 5f;//nếu target vào vùng 5f thì sẽ không bắn skill
    [SerializeField] Animator animator;
    public GameObject skill1;
    public float skill1CoolDown;
    public GameObject skill2;//khai báo skill 2 partical 
    [SerializeField] private float lastTimeSkill1 = 0;//đặt lại thời gian = 0
    public GameObject hideSword;
    void Start()
    {
        hideSword.SetActive(true);
        skill1.SetActive(false);
        animator.GetComponent<Animator>();

    }


    void Update()
    {
        var playerTarget = Vector3.Distance(player.position, transform.position);//khoảng cách từ boss tới player
        if (playerTarget >= radius && playerTarget > cantSkill && Time.time >= lastTimeSkill1 + skill1CoolDown)
        {
            int randoom = Random.Range(0, 2);
            if (randoom == 0)
            {
                animator.SetTrigger("Skill1");
                skill1.SetActive(true);
                Debug.Log("Skill1");
                hideSword.SetActive(false);
                Debug.Log("Đã Ẩn Sword");
            }
            if (randoom == 1)
            {
                GameObject skill2Transform = Instantiate(skill2, player.position, Quaternion.identity);// tao skill ngay tai vi tri player
                Destroy(skill2Transform, 2f);
                animator.SetTrigger("Skill2");
                Debug.Log("Skill2");
                hideSword.SetActive(false);
                Debug.Log("Đã Ẩn Sword");

            }
            lastTimeSkill1 = Time.time;
        }

    }
    public void EndSkill1(){
        skill1.SetActive(false);
    }

}
