using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private LayerMask Ice;
    [SerializeField] private LayerMask Fire;

    public TextMeshProUGUI WeaponIce;
    private string countIce;

    public TextMeshProUGUI WeaponFire;
    private int countFire;
    void Start()
    {
        WeaponIce.text = $"WeaponIce: {countIce}";
        WeaponFire.text = $"WeaponFire: {countFire}";

    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit, 10, Ice))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            Destroy(hit.transform.gameObject);
            countIce+=1;
            WeaponIce.text = $"WeaponIce: {countIce}";
        }
       

        if (Physics.Raycast(transform.position, transform.forward, out var hit2, 10, Fire))
        {
            Debug.DrawRay(transform.position, transform.forward * hit2.distance, Color.red);
            Destroy(hit2.transform.gameObject);
            countFire += 1;
            WeaponFire.text = $"WeaponFire: {countFire}";
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.yellow);
        }
    }
}
