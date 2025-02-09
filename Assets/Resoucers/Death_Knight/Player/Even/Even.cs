using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Even : MonoBehaviour
{
    [SerializeField] private GameObject panelEven;
    [SerializeField] private TextMeshProUGUI textEven;
    public int countEven;  // Biến này sẽ được cập nhật
    private int countEvenMax = 5;
    public GameObject tele;
    public GameObject chest3;
    public GameObject chest4;
    public GameObject chest5;
    public GameObject danDuong;
    void Start()
    {
        textEven.text = $"{countEven}/{countEvenMax}";
    }

    void Update()
    {
    }

    public void Even1(int amount)
    {
        // Tăng countEven
        countEven += amount;

        // Đảm bảo không vượt quá countEvenMax
        if (countEven > countEvenMax)
        {
            countEven = countEvenMax;
        }

        // Cập nhật lại text hiển thị
        textEven.text = $"{countEven}/{countEvenMax}";

        if (countEven == 2)
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
        if(countEven == 5)
        {
            tele.SetActive(true);
            Destroy(danDuong, 3f);
        }
    }
}
