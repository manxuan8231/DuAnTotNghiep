using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tru : MonoBehaviour
{
    public float health;
    public float maxHealth = 1000;
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }
    public void TakeHealh(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Even2 even2 = FindAnyObjectByType<Even2>();
            even2.tru += 1;
            even2.textTru.text = $"Trụ:{even2.tru}/{1}";
            Destroy(gameObject);
        }
    }
}
