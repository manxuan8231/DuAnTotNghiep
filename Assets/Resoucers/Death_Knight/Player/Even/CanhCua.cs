using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanhCua : MonoBehaviour
{
    public GameObject button;
    public GameObject door;
    public GameObject chest;
    public Key key;
    public Vector3 positionDoor;
    public float speedDoor = 2f;
    private bool isMovingDoor = false;
    public GameObject textDoor;

    public CinemachineVirtualCamera virtualCameraDoor;
    public CinemachineVirtualCamera virtualCameraPlayer;
    void Start()
    {
        button.SetActive(false);    
        chest.SetActive(false);
        textDoor.SetActive(false);
    }

   
    void Update()
    {
        if(button.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(key.KeyCount() >= 1)
                {
                    //door.SetActive(false);
                    chest.SetActive(true);
                          
                    isMovingDoor = true;
                    StartCoroutine(FocusDoor());
                }              
            }
        }
        if(isMovingDoor)
        {
            MoveDoorToPosition();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
            textDoor.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);
            textDoor.SetActive(false);
        }
    }

    void MoveDoorToPosition()
    {
        // Di chuyển cửa dần dần đến vị trí mong muốn
        door.transform.position = Vector3.MoveTowards(door.transform.position, positionDoor, speedDoor * Time.deltaTime);

        // Kiểm tra nếu cửa đã đến vị trí chỉ định thì dừng lại
        if (Vector3.Distance(door.transform.position, positionDoor) < 0.01f)
        {
            isMovingDoor = false;
        }
    }
    public IEnumerator FocusDoor()
    {
        
        virtualCameraDoor.Priority = 20;
        virtualCameraPlayer.Priority = 0;

        yield return new WaitForSeconds(4);

        virtualCameraDoor.Priority = 0;
        virtualCameraPlayer.Priority = 20;
    }
}
