using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public GameObject ivt;
    void Start()
    {
        ivt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ivt.SetActive(true);
        }
    }
    public void ExitInventory()
    {
        ivt.SetActive(false);
    }
}
