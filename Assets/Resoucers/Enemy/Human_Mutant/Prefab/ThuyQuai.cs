using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThuyQuai : MonoBehaviour
{
    public float rangerPlayer = 30f;//khoamn cach thay player
    public float rangerPlayerAttack = 5f;//khoan cach thay player

    private Animator animator;
    private Transform player;

    //hp
    public GameObject gameObjectSlider;
    public Slider currentHealth;
    public TextMeshProUGUI textHealth;
    public float maxHealth = 10000;

    public SliderHp sliderHp;

    private bool isDie;
    void Start()
    {
        isDie = true;
        animator = GetComponent<Animator>();
      
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
            currentHealth.value ++;
            textHealth.text = $"{currentHealth.value}/{maxHealth}";
        }

       
    }
    public void TakeDame(float amount)
    {
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
   
}
