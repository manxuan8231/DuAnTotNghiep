using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform khu1Transform;
    public Transform khu2Transform;
    public Transform khu3Transform;
    private GameObject player;
   
    public TextMeshProUGUI textKhu;

    private string a = "Khu bí ẩn";
    private string b = "Hồ nước";
    private string c = "Lâu đài";
    void Start()
    {
        textKhu.enabled = false;      
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.transform.position = khu1Transform.position;
            StartCoroutine(TextKhu1(a));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.transform.position = khu2Transform.position;
            StartCoroutine(TextKhu1(c));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.transform.position = khu3Transform.position;
            StartCoroutine(TextKhu1(b));
        }
    }
    public void Khu1()
    {
        player.transform.position = khu1Transform.position;
        StartCoroutine(TextKhu1(a));
    }
    public void Khu2()
    {
        player.transform.position = khu2Transform.position;
        StartCoroutine(TextKhu1(c));
    }
    public void Khu3()
    {
        player.transform.position = khu3Transform.position;
        StartCoroutine(TextKhu1(b));
    }

    private IEnumerator TextKhu1(string _input)
    {
        textKhu.enabled = true;
        textKhu.text = _input;
        yield return new WaitForSeconds(3);
        textKhu.enabled = false;      
    }
    
}
