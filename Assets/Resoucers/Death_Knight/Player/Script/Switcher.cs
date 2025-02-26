﻿using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Thêm namespace Cinemachine

public class Switcher : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    private float cooldownTime = 3f; // Thời gian cooldown
    private float lastSwitchTime;    // Thời điểm đổi nhân vật gần nhất

    void Start()
    {
        lastSwitchTime = -cooldownTime; // Đảm bảo có thể đổi ngay khi bắt đầu
    }

    void Update()
    {
        // Kiểm tra nếu đủ thời gian cooldown
        if (Time.time - lastSwitchTime >= cooldownTime)
        {
            // Kiểm tra input từ bàn phím để đổi nhân vật
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchCharacter1();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchCharacter2();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchCharacter3();
            }
        }
    }

    private void SwitchCharacter1()
    {
        Vector3 currentPosition = player1.activeSelf ? player1.transform.position : GetActivePlayerPosition();
        player1.SetActive(true);
        player1.transform.position = currentPosition;

        player2.SetActive(false);
        player3.SetActive(false);

        UpdateLastSwitchTime();
    }

    private void SwitchCharacter2()
    {
        Vector3 currentPosition = player2.activeSelf ? player2.transform.position : GetActivePlayerPosition();
        player2.SetActive(true);
        player2.transform.position = currentPosition;

        player1.SetActive(false);
        player3.SetActive(false);

        UpdateLastSwitchTime();
    }

    private void SwitchCharacter3()
    {
        Vector3 currentPosition = player3.activeSelf ? player3.transform.position : GetActivePlayerPosition();
        player3.SetActive(true);
        player3.transform.position = currentPosition;

        player1.SetActive(false);
        player2.SetActive(false);

        UpdateLastSwitchTime();
    }

    private Vector3 GetActivePlayerPosition()
    {
        if (player1.activeSelf) return player1.transform.position;
        if (player2.activeSelf) return player2.transform.position;
        if (player3.activeSelf) return player3.transform.position;
        return Vector3.zero; // Vị trí mặc định nếu không có nhân vật nào được kích hoạt
    }

    private void UpdateLastSwitchTime()
    {
        lastSwitchTime = Time.time; // Cập nhật thời gian đổi gần nhất
    }
}
