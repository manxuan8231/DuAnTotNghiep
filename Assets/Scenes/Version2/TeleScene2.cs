using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleScene2 : MonoBehaviour
{
    public GameObject secondObject; // GameObject thứ 2 cần kéo theo
    public Vector3 spawnPositionScene2 = new Vector3(12002.2f, 6.7f, -6032.2f); // Vị trí Player spawn trong Scene 2
    public Boss1 boss1;
    void OnTriggerEnter(Collider other)
    {
       /* boss1 = other.GetComponent<Boss1>();
        if(boss1.currentHealth.value <= 0)
        {}*/
            if (other.CompareTag("Player"))
            {
                DontDestroyOnLoad(other.gameObject); // Giữ Player khi đổi Scene

                if (secondObject != null)
                {
                    DontDestroyOnLoad(secondObject); // Giữ lại GameObject thứ 2
                }

                SceneManager.sceneLoaded += OnSceneLoaded; // Lắng nghe sự kiện load Scene
                SceneManager.LoadScene(3); // Chuyển Scene 3
            }
        
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPositionScene2; // Đặt lại vị trí Player
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; // Xóa sự kiện để tránh lỗi chồng lặp
    }
}
