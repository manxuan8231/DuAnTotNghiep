using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTimer : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public Vector3 spawnPositionScene2 = new Vector3(8730.7f, 6.2f, -6038.6f); // Vị trí Player spawn trong Scene 2
    private bool sceneChanging = false; // Biến để tránh load scene nhiều lần

    private void Update()
    {
        if (sceneChanging) return; // Nếu đang đổi scene, không chạy tiếp

        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            sceneChanging = true; // Đánh dấu đã đổi scene để tránh load nhiều lần
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        // Tự động tìm và giữ lại các đối tượng quan trọng khi load scene
        GameObject playerScene = GameObject.FindWithTag("Player");
        GameObject cameraScene = GameObject.FindWithTag("camera"); 
        if (playerScene != null) DontDestroyOnLoad(playerScene);
        if (cameraScene != null) DontDestroyOnLoad(cameraScene);

        // Đợi scene load xong rồi di chuyển Player
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);

        yield return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player"); // Tìm lại Player trong scene mới
        if (player != null)
        {
            player.transform.position = spawnPositionScene2; // Đặt lại vị trí Player
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; // Xóa sự kiện để tránh lỗi chồng lặp
    }
}
