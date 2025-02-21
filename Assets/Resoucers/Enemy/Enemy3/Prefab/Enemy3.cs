using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy3 : MonoBehaviour
{
    public float rangerPlayer = 30f;//khoamn cach thay player
    public float rangerPlayerAttack = 30f;//khoamn cach thay player

    //skill
    public GameObject fireBall;
    public Transform positionAttack;
    public float cooldownAttack = 0;

    private Animator animator;
    private Transform player;

    private Rigidbody rb;
    private bool isOnSkill = true;
    //hp
    public Slider currentHealth;
    public TextMeshProUGUI textHealth;
    public float maxHealth = 1000;

    public SliderHp sliderHp;
    void Start()
    {
        isOnSkill = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //hpo
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
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= rangerPlayer)
        {
            //xử lý xoay theo player
            Vector3 direction = (player.position - transform.position).normalized; // Hướng đến Player
            direction.y = 0; // Giữ y = 0 để tránh nghiêng đầu
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Tạo góc quay hướng về Player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Xoay mượt mà
            StartCoroutine(FlipSKill());
        }
        
        if(distance <= rangerPlayerAttack && Time.time >= cooldownAttack + 5 && isOnSkill == true)
        {
            animator.SetTrigger("Attack");
            GameObject gameObject = Instantiate(fireBall, positionAttack.position, Quaternion.identity);
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 20;
            Destroy(gameObject,5f);
            cooldownAttack = Time.time;
        }
    }
    public void TakeDame(float amount)
    {
        currentHealth.value -= amount;
        sliderHp.AddUlti(100);
        textHealth.text = $"{currentHealth.value}/{maxHealth}";
        if (currentHealth.value <= 0)
        {
            sliderHp.AddExp(8888);
            Destroy(gameObject,0.4f);    
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillR"))
        {
            TakeDame(40);
        }
      
    }
    private IEnumerator FlipSKill()
    {
        yield return new WaitForSeconds(2);
        isOnSkill = true;
    }
}
