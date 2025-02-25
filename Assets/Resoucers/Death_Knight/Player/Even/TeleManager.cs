using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleManager : MonoBehaviour
{
    public GameObject teleManager;

    void Start()
    {
        teleManager.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Đảo trạng thái bật/tắt
            teleManager.SetActive(!teleManager.activeSelf);
        }
    }
}
