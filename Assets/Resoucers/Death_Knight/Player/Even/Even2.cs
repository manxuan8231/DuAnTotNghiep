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
    public GameObject door;
    public Enemy3 enemy3;

    public CinemachineVirtualCamera getDoor;

    public GameObject endEven;
    void Start()
    {       
        textTru.text = $"Trụ:{tru}/{1}";
        textEnemy.text = $"Enemy:{enemy}/{10}";
        textManhBang.text = $"Mảnh băng:{manhBang}/{1}";
        door.SetActive(true);
    }

    void Update()
    {
       
        if ( enemy >= 10 && manhBang >= 1 && tru >= 1)
        {
            tru = Mathf.Clamp(tru, 0, 1);
            enemy = Mathf.Clamp(enemy, 0, 10);
            manhBang = Mathf.Clamp(manhBang, 0, 1);
            StartCoroutine(TagetCamera());           
        }
    }
    private IEnumerator TagetCamera()
    {
        getDoor.Priority = 20;
        yield return new WaitForSeconds(3);   
        getDoor.Priority = -1;
        door.SetActive(false);
        endEven.SetActive(false);
    } 
}
