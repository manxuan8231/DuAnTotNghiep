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
    //các vật thể để hiện
    public GameObject tele;
    public GameObject chest3;
    public GameObject chest4;
    public GameObject chest5;
    public GameObject danDuong;
    void Start()
    {
        
        textEven.text = $"{ countEven }/{ countEvenMax}";
        tele.SetActive(false);
        chest3.SetActive(false);
        chest4.SetActive(false);
        chest5.SetActive(false);
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
            danDuong.SetActive(false);
        }
        if(countEven == 2)
        {
            chest3.SetActive(true);
            danDuong.SetActive(true);
        }
        if (countEven == 3)
        {
            chest4.SetActive(true);
        }
        if (countEven == 4)
        {
            chest5.SetActive(true);
        }
    }
}
