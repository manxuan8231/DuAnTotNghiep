using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private LayerMask Item;
    public TextMeshProUGUI textItem;
    private string countItem;
    public GameObject button;

    [SerializeField] private LayerMask TruongThon;
    public GameObject buttonTT;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit, 10, Item))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
           
            button.SetActive(true);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.yellow);
            button.SetActive(false);
        }
        //truong thon
        if (Physics.Raycast(transform.position, transform.forward, out var other, 10,TruongThon))
        {
            Debug.DrawRay(transform.position, transform.forward * other.distance, Color.red);

            buttonTT.SetActive(true);
            Debug.Log("da nhin thay");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.yellow);
            buttonTT.SetActive(false);
        }
    }
}
