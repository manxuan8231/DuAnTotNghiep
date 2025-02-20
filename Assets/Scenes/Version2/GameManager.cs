using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private string targetSpawnTag;
    private GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không bị hủy khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTargetSpawnTag(string tag)
    {
        targetSpawnTag = tag;
    }

    public void TeleportPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        GameObject targetSpawn = GameObject.FindGameObjectWithTag(targetSpawnTag);
        if (targetSpawn != null)
        {
            player.transform.position = targetSpawn.transform.position; // Dịch chuyển Player đến vật thể có tag chỉ định
        }
        else
        {
            Debug.LogWarning("Không tìm thấy vật thể có tag spawn trong Scene 2!");
        }
    }

    public void PlayerDied()
    {
        if (player != null)
        {
            TeleportPlayer(); // Đưa player về vật thể spawn
        }
    }
}
