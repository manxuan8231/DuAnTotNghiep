using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyButton : MonoBehaviour
{
   public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PressAnyKey());
        
    }


    IEnumerator PressAnyKey()
    {

        if (Input.anyKeyDown)
        {
            animator.SetBool("isIdle", false); 
            animator.SetTrigger("Rage");
            yield return new WaitForSeconds(1.5f);  
            SceneManager.LoadScene(1);
        }
    }
}
