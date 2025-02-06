using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    
    public Vector3 vector3;
    public GameObject tele;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu Player chạm vào cổng
        {
           
            other.transform.position = tele.transform.position;
        }
    }
}
