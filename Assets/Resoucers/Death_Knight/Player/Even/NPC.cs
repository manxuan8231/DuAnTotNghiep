using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại

    public string[] names; // Danh sách tên (ai đang nói)
    public string[] content; // Nội dung hội thoại

    private Coroutine coroutine;
    private bool isSkipping = false; // Kiểm tra trạng thái đang đẩy nhanh chữ
    private bool isReading = false; // Kiểm tra nếu đang chạy một câu thoại

    public GameObject buttonBG; // Nền nút
    public GameObject player; // Tham chiếu đến đối tượng Player

    public Button quitButton; // Nút thoát
    public Button startEventButton; // Nút bắt đầu event

    public GameObject evenGameObject;
    void Start()
    {
        NPCPanel.SetActive(false);
        NPCName.text = "";
        NPCContent.text = "";
        buttonBG.SetActive(false);

        // Ẩn 2 nút ban đầu
        quitButton.gameObject.SetActive(false);
        startEventButton.gameObject.SetActive(false);

        // Gán sự kiện cho các nút
        quitButton.onClick.AddListener(EndContent);
        startEventButton.onClick.AddListener(StartEvent);
    }

    private void Update()
    {
        // Kiểm tra nếu phím F được nhấn
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (NPCPanel.activeSelf)
            {
                EndContent(); // Dừng hội thoại nếu đang hiển thị
            }
            else
            {
                NPCPanel.SetActive(true);
                coroutine = StartCoroutine(ReadContent()); // Bắt đầu hội thoại
                buttonBG.SetActive(true);

                // Tắt CharacterController của Player
                if (player != null)
                {
                    var controller = player.GetComponent<CharacterController>();
                    if (controller != null)
                    {
                        controller.enabled = false;
                    }
                }
            }
        }

        // Kiểm tra nếu phím Space được nhấn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isReading)
            {
                isSkipping = true; // Kích hoạt trạng thái bỏ qua
            }
        }
    }

    private IEnumerator ReadContent()
    {
        // Lặp qua từng câu thoại và tên
        for (int i = 0; i < content.Length; i++)
        {
            isReading = true;
            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "Unknown"; // Hiển thị tên hoặc "Unknown" nếu không có tên

            foreach (var item in content[i])
            {
                if (isSkipping)
                {
                    // Nếu người chơi bấm Space, hiển thị toàn bộ nội dung còn lại của câu ngay lập tức
                    NPCContent.text = content[i];
                    yield return null; // Dừng vòng lặp chữ hiện tại
                    break;
                }

                NPCContent.text += item;
                yield return new WaitForSeconds(0.05f); // Tốc độ chạy chữ
            }

            // Reset trạng thái sau khi câu hiện xong
            isSkipping = false;

            // Chờ người chơi bấm Space để chuyển sang câu tiếp theo
            while (!isSkipping)
            {
                yield return null;
            }

            isSkipping = false; // Reset sau khi chuyển câu
        }

        isReading = false; // Kết thúc đọc
        ShowButtons(); // Hiển thị nút sau khi đọc xong
    }

    private void ShowButtons()
    {
        // Hiển thị các nút hành động
        quitButton.gameObject.SetActive(true);
        startEventButton.gameObject.SetActive(true);
    }

    public void EndContent()
    {
        NPCPanel.SetActive(false);
        buttonBG.SetActive(false);

        // Ẩn các nút hành động
        quitButton.gameObject.SetActive(false);
        startEventButton.gameObject.SetActive(false);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // Bật lại CharacterController của Player
        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = true;
            }
        }
    }

    public void StartEvent()
    {
        Debug.Log("Event bắt đầu!"); // Thay bằng logic cho sự kiện của bạn
        
        evenGameObject.SetActive(true);

        // Đóng hội thoại sau khi bắt đầu event
        EndContent();
    }
}
