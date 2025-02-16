using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Boss1 : MonoBehaviour
{
    public Transform player;

    //xử lý hiệu ứng tấn công
    public GameObject EffectAttacking;
    // Xử lý chức năng launching
    public CinemachineVirtualCamera bossCam;
    public CinemachineVirtualCamera playerCam;
    public float detectRange = 10f;
    public float focusDuration = 3f;
    private bool isFocusing = false;

    // Audio sounds
    public AudioSource audioSource;
    public AudioClip audioClipLaughVFX;
    public AudioClip audioClipHeyyaVFX;
    public AudioClip audioClipMedicVFX;
    public AudioClip audioClipWhyVFX;
    public AudioClip audioClipAreYouVFX;
    public AudioClip audioClipDieVFX;
    // Xử lý tấn công
    public float distanceAttack = 10f; // Khoảng cách nhìn thấy player để tấn công
    public float attackCooldown = 10f; // Thời gian hồi chiêu
    private float lastAttackTime = 0f;
    public GameObject weappon;
    public Animator animator;
    private bool onAttack = true;
    public Transform teleAttack3;
    public GameObject effectAttack3;

    //xử lý skill
    public float distanceSkill = 50; //khoản cách nhìn player để dùng skill
    private bool onSkill = false;
    public float lastSkillTime = 0f;
 
    public GameObject ballSkill1;//tạo quả cầu chổ player (skill1)
    public GameObject laserSkill2;
    public GameObject tornadoSkill3;
    public Transform tranformSkill3;//vị trí bắn
    public GameObject effectSkill4;
    public GameObject effectDameSkill4;
    //xử lý di chuyển
    public float distanceWalk = 100; //khoản cách thấy player
    private bool onWalk = false;                                 
    public NavMeshAgent navMeshAgent;

    //xử lý hp
    public Slider currentHealth;
    private float maxHealth = 10000f;
    public TextMeshProUGUI textHealth;
    public GameObject health;
    public bool onTakeHealth = true;
    public BoxCollider boxCollider;

    // khi death
    public GameObject effectDeath;
    public Transform deathTransfrom;

    //xử lí effect slash
    //attack1
    public GameObject effectSlash1;

    public SliderHp sliderHp;
    private void Start()
    {
        playerCam.Priority = 20;
        bossCam.Priority = 0;
        weappon.SetActive(false);
        onSkill = false;
        onWalk = false;
        onAttack = true;
        effectSkill4.SetActive(false);
        effectAttack3.SetActive(false);
        health.SetActive(false);
        effectDameSkill4.SetActive(false);
        //hp
        currentHealth.maxValue = maxHealth;
        textHealth.text = $"{currentHealth.value}/{maxHealth}".ToString();
        onTakeHealth = true;
        //xử lý lấy hp player
        boxCollider.enabled = false;
        effectDeath.SetActive(false);

        //xử lý effect slash1
        effectSlash1.SetActive(false);
    }

    private void Update()
    {
        CameraFocus(); // Xử lý camera
        Attack(); // Xử lý tấn công
        //skill
        if(onSkill == true)
        {
            Skill();
        }
        //walk
        Movemen();
    }

    // Xử lý camera focus
    private void CameraFocus()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFocusing)
        {
            isFocusing = true;
            animator.SetBool("isLaughing", true);
            StartCoroutine(FocusOnBoss());
        }
    }

    private System.Collections.IEnumerator FocusOnBoss()
    {
        bossCam.Priority = 20;
        playerCam.Priority = 10;
        PlayLaughSound();
        health.SetActive(true);
        yield return new WaitForSeconds(focusDuration);

        bossCam.Priority = 10;
        playerCam.Priority = 20;
        onWalk = true;
        StopLaughSound();
    }

    // Phát âm thanh cười
    private void PlayLaughSound()
    {
        if (audioSource != null && audioClipLaughVFX != null && !audioSource.isPlaying)
        {
            audioSource.clip = audioClipLaughVFX;
            audioSource.Play();
        }
    }

    // Dừng âm thanh
    private void StopLaughSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Xử lý tấn công
    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.position);
       
        if (distance <= distanceAttack && Time.time >= lastAttackTime + attackCooldown && onAttack)
        {
            StartCoroutine(OnSkill());//onskill bật
            int random = Random.Range(0, 3);
            if(random == 0)
            {
                Debug.Log("Thực hiện attack 1");
                animator.SetTrigger("Attack");
                audioSource.PlayOneShot(audioClipMedicVFX);
                weappon.SetActive(true);
                StartCoroutine(EffectAttack1());
            }
            if (random == 1)
            {
                Debug.Log("Thực hiện attack 2");
                animator.SetTrigger("Attack2");
                weappon.SetActive(true);
                StartCoroutine(EffectAttack1());
            }
            if(random == 2)
            {
                Debug.Log("Thực hiện dịch chuyển ra");
                animator.SetTrigger("Attack3");               
                StartCoroutine(OnEffect());
                weappon.SetActive(false);
                audioSource.PlayOneShot(audioClipWhyVFX);
            }
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
        }
    }
    //xử lý hiệu ứng attack3
    private IEnumerator OnEffect()
    {
        effectAttack3.SetActive(true);
        yield return new WaitForSeconds(2f);
        transform.position = teleAttack3.position;
        effectAttack3.SetActive(false);
    }

    //xử lý cho skill khi dứng xa thì delay 3f mới bắn skill
    private IEnumerator OnSkill()
    {
        onSkill = false;
        yield return new WaitForSeconds(3);
        onSkill = true;
    }

    //xử lý skill
    private void Skill()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= distanceSkill && distance >= 10 && Time.time >= lastSkillTime + 4f)
        {
            int random = Random.Range(0, 4);

            if(random == 0)
            {
                Debug.Log("Skill1");
                animator.SetTrigger("Skill1");
                audioSource.PlayOneShot(audioClipAreYouVFX);
                //xử lý chức nang skill
                Vector3 playerPosition = new Vector3(player.position.x, 20.2f, player.position.z); //vị trí suất hiện
                GameObject ball = Instantiate(ballSkill1, playerPosition, Quaternion.identity);
                Destroy(ball,1.3f);

                //bật tắt weapon và offwalk
                weappon.SetActive(false);
                StartCoroutine(NoTaget());//khi dùng skill ko cho di chuyển
            }
            if(random == 1)
            {
                Debug.Log("Skill2");
                animator.SetTrigger("Skill2");
               
                weappon.SetActive(false);
                StartCoroutine(NoTaget());               
                //xử lý chức nang skill
                Vector3 playerPosition = new Vector3(player.position.x, 20f, player.position.z) ;
                GameObject laser = Instantiate(laserSkill2, playerPosition, Quaternion.identity);
                Destroy(laser,2f);
                
            }
            if(random == 2)
            {
                Debug.Log("Skill3");
                animator.SetTrigger("Skill3");
                weappon.SetActive(false);
                StartCoroutine(NoTaget());//khi dùng skill ko cho di chuyển
                //xử lý chức nang skill(có r)
               
            }
            if(random == 3)
            {
                Debug.Log("Skill4");
                animator.SetTrigger("Skill4");

                //xử lý chức năng của skill
                transform.position = player.transform.position;
                audioSource.PlayOneShot(audioClipHeyyaVFX);
                StartCoroutine(OnAttack());//ko cho tấn công khi dùng skill

                //bật tắt weapon và offwalk
                weappon.SetActive(true);
                StartCoroutine(NoTaget());
            }
            lastSkillTime = Time.time;
        }
        
    }
    //tắt attack khi dùng skill4
    public IEnumerator OnAttack()
    {
        effectSkill4.SetActive(true);
        onAttack = false;
        yield return new WaitForSeconds(2);
        effectSkill4.SetActive(false);
        onAttack = true;
    }
    public void Skill3()
    {
        Vector3 spam = new(tranformSkill3.position.x, tranformSkill3.position.y, tranformSkill3.position.z);
        GameObject ball = Instantiate(tornadoSkill3, spam, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 40f;
        Destroy(rb, 3f);
    }
    //xử lý walk
    private void Movemen()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= distanceWalk && onWalk && currentHealth.value > 0)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(player.position);
                animator.SetBool("isMoving", true);
                //xử lý xoay theo player
                Vector3 direction = (player.position - transform.position).normalized; // Hướng đến Player
                direction.y = 0; // Giữ y = 0 để tránh nghiêng đầu
                Quaternion targetRotation = Quaternion.LookRotation(direction); // Tạo góc quay hướng về Player
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Xoay mượt mà
            }
        }
        else
        {
            if(navMeshAgent == null)
            {
                // Nếu ngoài phạm vi, dừng di chuyển
                navMeshAgent.ResetPath();
                animator.SetBool("isMoving", false);
            }
            
        }
        if(distance <= 5)
        {
            animator.SetBool("isMoving", false);
        }
    }

    //xử lý khi đang dùng skill thì ko cho taget
    public IEnumerator NoTaget()
    {
        onWalk = false;
        yield return new WaitForSeconds(2);
        onWalk = true;
    }

    //xử lý hp
    public void TakeHealth(float amount)
    {       
            currentHealth.value -= amount;
            sliderHp.AddExp(1000);
            textHealth.text = $"{currentHealth.value}/{maxHealth}".ToString();
            currentHealth.value = Mathf.Clamp(currentHealth.value, 0, maxHealth);
                 
        if (currentHealth.value <= 0)
        {         
            onAttack = false;
            onSkill = false;
            onWalk = false;
            onTakeHealth = false;
            weappon.SetActive(false);
            transform.position = deathTransfrom.position;
            StartCoroutine(cameraTaget());
            animator.SetTrigger("death");               
            Destroy(gameObject, 6f);
        }
    }
    private IEnumerator cameraTaget()
    {        
        bossCam.Priority = 20;
        playerCam.Priority = 0;     
        yield return new WaitForSeconds(4);
        effectDeath.SetActive(true);
        bossCam.Priority = 0;
        playerCam.Priority = 20;
    }
    //xử lý lấy hp player
    public void StartDame()
    {
        boxCollider.enabled = true;
    }
    public void EndDame()
    {
        boxCollider.enabled= false;
    }

    //xử lý skill4 effect
    public void StartEffectSkill4()
    {
        effectDameSkill4.SetActive(true);
    }
    public void EndEffectSkill4()
    {
        effectDameSkill4.SetActive(false);
    }
    
    //xử lý effect slash
    public IEnumerator EffectAttack1()
    {
        effectSlash1.SetActive(true);
        yield return new WaitForSeconds(5f);
        effectSlash1.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillR"))
        {
            TakeHealth(1000);
        }
        if (other.gameObject.CompareTag("SkillZ"))
        {
            TakeHealth(1000);
        }
    }

    //xử lý hiệu ứng tấn công
    public void StartEffect()
    {

    }
    public void EndEffect()
    {

    }
}
