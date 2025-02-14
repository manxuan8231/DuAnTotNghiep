using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    [SerializeField] private Transform player;// khai báo vị trí player
    [SerializeField] float radius = 30f; // nếu player đứng quá 30f thì sẽ bắn skill
    [SerializeField] float cantSkill = 5f;//nếu target vào vùng 5f thì sẽ không bắn skill
    [SerializeField] Animator animator;
    public GameObject skill1;
    
    [SerializeField] private float lastTimeSkill1 = 0;//đặt lại thời gian = 0
    void Start()
    {
        skill1.SetActive(false);
        animator.GetComponent<Animator>();

    }


    void Update()
    {
        var playerTarget = Vector3.Distance(player.position, transform.position);//khoảng cách từ boss tới player
        if (playerTarget >= radius && playerTarget > cantSkill && Time.time >= lastTimeSkill1 + 10)
        {
            int randoom = Random.Range(0, 2);
            if (randoom == 0)
            {
                animator.SetTrigger("Skill1");
                skill1.SetActive(true);

            }
            if (randoom == 1)
            {
                animator.SetTrigger("Skill2");
            }
            lastTimeSkill1 = Time.time;
        }

    }
    public void EndSkill1(){
        skill1.SetActive(false);
    }

}
