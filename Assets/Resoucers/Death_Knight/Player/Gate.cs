using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public int sceneOnLoad;
    public Vector3 vector3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu Player chạm vào cổng
        {
            DontDestroyOnLoad(other.gameObject); // Giữ Player khi load Scene mới
            SceneManager.LoadScene(sceneOnLoad); // Chuyển sang Scene2
            other.transform.position = vector3;
        }
    }
}
