using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRSlash2 : MonoBehaviour
{
    public Boss1 boss1;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss1"))
        {
            boss1.TakeHealth(100);
        }
    }
}
