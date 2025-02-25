using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SkillManager : MonoBehaviour
{
    private Transform player;// khai báo vị trí player
    [SerializeField] float radius = 30f; // nếu player đứng quá 30f thì sẽ bắn skill
    [SerializeField] float cantSkill = 5f;//nếu target vào vùng 5f thì sẽ không bắn skill
    [SerializeField] Animator animator;
    public GameObject skill1;
    public float skill1CoolDown;
    public GameObject skill2;//khai báo skill 2 partical 
    [SerializeField] private float lastTimeSkill1 = 0;
    [SerializeField] private string targetTag = "Player";
    public GameObject hideSword;
    
   
    

    //vị trí
    public Transform TranformSkill1;
    public Transform TranformSkill2;
    public Transform TranformSkill3;
    public Transform TranformSkill4;
    public Transform TranformSkill5;
    public Transform TranformSkill6;
    public Transform TranformSkill7;
    //
    public GameObject effect1;
    public GameObject effect2;       
    public GameObject effect3;
    public GameObject effect4;
    public GameObject effect5;
    public GameObject effect6;
    public GameObject effect7;
   
   




    void Start()
    {
        skill1.SetActive(false);
        animator.GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag(targetTag);
        if (playerObject != null)
        {
            player = playerObject.transform; // Gán Transform của đối tượng tìm được vào target
        }
        else
        {
            Debug.LogError($"Không tìm thấy đối tượng nào có tag: {targetTag}");
        }
    }


    void Update()
    {
        var playerTarget = Vector3.Distance(player.position, transform.position);//khoảng cách từ boss tới player
        if (playerTarget >= radius && playerTarget > cantSkill && Time.time >= lastTimeSkill1 + skill1CoolDown)
        {
            int randoom = Random.Range(0, 3);
            if (randoom == 0)
            {
                animator.SetTrigger("Skill1");
                skill1.SetActive(true);
                Debug.Log("Skill1");
                StartCoroutine(OffSword());
            }
            if (randoom == 1)
            {
                GameObject skill2Transform = Instantiate(skill2, player.position, Quaternion.identity);// tao skill ngay tai vi tri player
                Destroy(skill2Transform, 2f);

                animator.SetTrigger("Skill2");
                Debug.Log("Skill2");
                StartCoroutine(OffSword());

            }
            if (randoom == 2)
            {
                GameObject skill3Transform1 = Instantiate(effect1, TranformSkill1.position, Quaternion.identity);
                Destroy(skill3Transform1, 4f);
                GameObject skill3Transform2 = Instantiate(effect2, TranformSkill2.position, Quaternion.identity);
                Destroy(skill3Transform2, 4f);
                GameObject skill3Transform3 = Instantiate(effect3, TranformSkill3.position, Quaternion.identity);
                Destroy(skill3Transform3, 4f);
                GameObject skill3Transform4 = Instantiate(effect4, TranformSkill4.position, Quaternion.identity);
                Destroy(skill3Transform4, 4f);
                GameObject skill3Transform5 = Instantiate(effect5, TranformSkill5.position, Quaternion.identity);
                Destroy(skill3Transform5, 4f);
                GameObject skill3Transform6 = Instantiate(effect6, TranformSkill6.position, Quaternion.identity);
                Destroy(skill3Transform6, 4f);
                GameObject skill3Transform7 = Instantiate(effect7, TranformSkill7.position, Quaternion.identity);
                Destroy(skill3Transform7, 4f);

                animator.SetTrigger("Skill3");
                Debug.Log("Skill3");
                StartCoroutine(OffSword());

                
            }
            lastTimeSkill1 = Time.time;
        }

    }
    public void EndSkill1() {
        skill1.SetActive(false);
    }
    public IEnumerator OffSword()
    {
        hideSword.SetActive(false);
        yield return new WaitForSeconds(3f);
        hideSword.SetActive(true);  
    }
    

   
}
