using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Even : MonoBehaviour
{
    [SerializeField] private GameObject panelEven;
    [SerializeField] private TextMeshProUGUI textEven;
    private  float countEven = 0;
    private static float countEvenMax = 5;
    public GameObject tele;
    void Start()
    {
        
        textEven.text = $"{ countEven }/{ countEvenMax}";
        tele.SetActive(false);
    }

    
    void Update()
    {
        
    }
    public void Even1(float amount)
    {
        countEven += amount;
        textEven.text = $"{countEven}/{countEvenMax}";
        if(countEven == 5)
        {
            tele.SetActive(true);
        }
    }
}
