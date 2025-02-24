using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThuyQuai : MonoBehaviour
{
    public float rangerPlayer = 30f;//khoamn cach thay player
    public float rangerPlayerAttack = 5f;//khoan cach thay player
    public float rangerPlayerShout = 30f; //shout
    public float rangerPlayerSkill = 60f; //skill

    private Animator animator;
    private Transform player;
    private Rigidbody rb;
    //hp
    public GameObject gameObjectSlider;
    public Slider currentHealth;
    public TextMeshProUGUI textHealth;
    public float maxHealth = 10000;

    //cooldown
    private float timeCoolDownAttack = 0;
    private float timeCoolDownSkill = 0;

    private bool isDie; //nó die thì ko cho nhận máu
    private bool isShout = true; //la 
    private bool isSkill;
    private bool isAttack;
    
    //va cham player takehealth
    public BoxCollider boxDame;

    //sounds
    private AudioSource audioSource;
    public AudioClip audioClipShout;

    //skill
    public GameObject skill1;

    private Vector3 currentPosion;

    private NavMeshAgent navMeshAgent;
    

    void Start()
    {
        currentPosion = transform.position;//vị trí ban đầu
       
        isSkill = false;
        isAttack = true;
        isShout = true;
        boxDame.enabled = false;
        isDie = true;
        skill1.SetActive(false);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //hp
        currentHealth.value = maxHealth;
        textHealth.text = $"{currentHealth.value}/{maxHealth}";

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }


    void Update()
    {
        FlipPlayer();//nó thấy
        AttackCombo();//nó attack
        Shot();//nó la
        Skill();

        navMeshAgent.SetDestination(currentPosion);      
    }
    private void FlipPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= rangerPlayer)
        {
            gameObjectSlider.SetActive(true);
            //xử lý xoay theo player
            Vector3 direction = (player.position - transform.position).normalized; // Hướng đến Player
            direction.y = 0; // Giữ y = 0 để tránh nghiêng đầu
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Tạo góc quay hướng về Player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Xoay mượt mà

        }
        else
        {

            gameObjectSlider.SetActive(false);
            currentHealth.value++;
            textHealth.text = $"{currentHealth.value}/{maxHealth}";
        }
    }
    private void AttackCombo()
    {
        float distanceAttack = Vector3.Distance(player.position, transform.position);
        if (distanceAttack <= rangerPlayerAttack && Time.time >= timeCoolDownAttack + 7f && isAttack == true)
        {
            StartCoroutine(IsSkill());
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                animator.SetTrigger("attack1");
            }
            if (random == 1)
            {
                animator.SetTrigger("attack2");
            }
            timeCoolDownAttack = Time.time;
        }
    }
    private void Skill()
    {
        float distanceSkill = Vector3.Distance(player.position, transform.position);
        if (distanceSkill <= rangerPlayerSkill && distanceSkill >= rangerPlayerAttack &&
            Time.time >= timeCoolDownSkill + 8f && isSkill == true)
        {
            StartCoroutine(IsAttack());
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                Debug.Log("skill1");
                isShout = true;
                animator.SetTrigger("skill1");
                StartCoroutine(Skill1(3));
            }
            if (random == 1)
            {
                Debug.Log("skill2");
                animator.SetTrigger("skill2");
                
            }
            timeCoolDownSkill = Time.time;
        }
    }
   
    private void Shot()
    {
        float distanceShout = Vector3.Distance(player.position, transform.position);
        if (distanceShout <= rangerPlayerShout && isShout == true)
        {
            animator.SetTrigger("shout");
            isShout = false;
        }

    }
    public void TakeDame(float amount)
    {
        SliderHp sliderHp = FindAnyObjectByType<SliderHp>();
        if (isDie == true)
        {
            currentHealth.value -= amount;
            textHealth.text = $"{currentHealth.value}/{maxHealth}";
            animator.SetTrigger("hit");
            sliderHp.AddUlti(100);
            if (currentHealth.value <= 0)
            {
                animator.SetTrigger("die");
                isDie = false;
                sliderHp.AddExp(100000);
                Destroy(gameObject, 2f);
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillR"))
        {
            TakeDame(40);
        }
        if (other.gameObject.CompareTag("SkillZ"))
        {
            TakeDame(1000);
        }
    }
    
    public void BeginDame()
    {
        boxDame.enabled = true;
    }
    public void EndDame()
    {
        boxDame.enabled = false;
    }
    public void SoundShout()
    {
        audioSource.PlayOneShot(audioClipShout);
    }

    private IEnumerator Skill1(float secon)
    {
        skill1.SetActive(true);
        yield return new WaitForSeconds(secon);
        skill1.SetActive(false);
    }
    private IEnumerator IsSkill()
    {
        isSkill = false;
        yield return new WaitForSeconds(4);
        isSkill = true;
    }
    private IEnumerator IsAttack()
    {
        isAttack = false;
        yield return new WaitForSeconds(4);
        isAttack = true;
    }
   
}
