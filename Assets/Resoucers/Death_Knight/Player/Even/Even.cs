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
    void Start()
    {
        
        textEven.text = $"{ countEven }/{ countEvenMax}";
      
    }

    
    void Update()
    {
        
    }
    public void Even1(float amount)
    {
        countEven += amount;
        textEven.text = $"{countEven}/{countEvenMax}";
    }
}
