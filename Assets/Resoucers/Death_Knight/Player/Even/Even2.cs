using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Even2 : MonoBehaviour
{
    public float tru = 0;
    public TextMeshProUGUI textTru;
    public float enemy = 0;
    public TextMeshProUGUI textEnemy;
    public float manhBang = 0;
    public TextMeshProUGUI textManhBang;

    //
    public GameObject boss;
    public GameObject door;
    public Enemy3 enemy3;

    public CinemachineVirtualCamera getDoor;

    void Start()
    {
        
        textTru.text = $"Trụ:{tru}/{1}";
        textEnemy.text = $"Enemy:{enemy}/{20}";
        textManhBang.text = $"Mảnh băng:{manhBang}/{1}";
        boss.SetActive(false);
        door.SetActive(true);
    }

    void Update()
    {
        tru = Mathf.Clamp(tru, 0, 1);
        enemy = Mathf.Clamp(enemy, 0, 20);
        manhBang = Mathf.Clamp(manhBang, 0, 1);
        if ( enemy >= 20 && manhBang >= 1 && tru >= 1)
        {
            StartCoroutine(TagetCamera());
            
        }
    }
    private IEnumerator TagetCamera()
    {
        getDoor.Priority = 20;
        yield return new WaitForSeconds(3);   
        getDoor.Priority = -1;
        door.SetActive(false);
        boss.SetActive(true);
    } 
}
