using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    //hien dame
    public TextMeshProUGUI textHealth;
    private float textCount = 0;
    //hp
    public float currentHealth;
    public float maxHealth = 1000;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //hpo
        currentHealth = maxHealth;
        if (textHealth != null)
        {
            textHealth.text = "0";
        }

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
        }
        
        if(distance <= rangerPlayerAttack && Time.time >= cooldownAttack + 5)
        {
            animator.SetTrigger("Attack");
            GameObject gameObject = Instantiate(fireBall, positionAttack.position, Quaternion.identity);
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 10;
            Destroy(gameObject,7f);
            cooldownAttack = Time.time;
        }
    }
    public void TakeDame(float amount)
    {
        currentHealth -= amount;
        textCount += amount;
        
        if (textHealth != null)
        {
            textHealth.text = textCount.ToString();
        }
        StartCoroutine(TextCoolDown());
        if (currentHealth <= 0)
        {
            Destroy(gameObject);    
        }
    }
    private IEnumerator TextCoolDown()
    {  
        yield return new WaitForSeconds(3);
        textCount = 0;
        if (textHealth != null)
        {
            textHealth.text = "0";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillR"))
        {
            TakeDame(20);
        }
        if (other.gameObject.CompareTag("SkillZ"))
        {
            TakeDame(100);
        }
    }
}
