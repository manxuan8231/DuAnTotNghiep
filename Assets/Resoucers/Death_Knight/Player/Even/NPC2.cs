using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC2 : MonoBehaviour
{
    [SerializeField] private GameObject NPCPanel; // Panel hiển thị hội thoại
    [SerializeField] private TextMeshProUGUI NPCName; // Tên của NPC
    [SerializeField] private TextMeshProUGUI NPCContent; // Nội dung hội thoại
    [SerializeField] private GameObject buttonBG; // Nền nút
    [SerializeField] private Button quitButton; // Nút thoát
    [SerializeField] private Button startEventButton; // Nút bắt đầu event
    [SerializeField] private GameObject evenGameObject; // Đối tượng kích hoạt sự kiện
    [SerializeField] private GameObject pressFUI; // UI nhấn F để bắt đầu hội thoại
    [SerializeField] private string[] names; // Danh sách tên (ai đang nói)
    [SerializeField] private string[] content; // Nội dung hội thoại

    private Coroutine coroutine;
    private bool isSkipping = false; // Kiểm tra trạng thái đang đẩy nhanh chữ
    private bool isReading = false; // Kiểm tra nếu đang chạy một câu thoại
    private bool isPlayerNear = false; // Kiểm tra nếu Player đang trong phạm vi
    private bool isDialogueActive = false; // Kiểm tra nếu hội thoại đang diễn ra

    private GameObject player; // Tham chiếu đến Player

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Ẩn tất cả UI ban đầu
        NPCPanel.SetActive(false);
        buttonBG.SetActive(false);
        quitButton.gameObject.SetActive(false);
        startEventButton.gameObject.SetActive(false);
        pressFUI.SetActive(false); // Ẩn UI nhấn F

        // Gán sự kiện cho các nút
        quitButton.onClick.AddListener(EndContent);
        startEventButton.onClick.AddListener(StartEvent);
    }

    private void Update()
    {
        // Nếu Player ở gần và chưa có hội thoại, hiển thị hướng dẫn bấm F
        if (isPlayerNear && !isDialogueActive)
        {
            pressFUI.SetActive(true);
        }
        else
        {
            pressFUI.SetActive(false);
        }

        // Khi nhấn F, bắt đầu hội thoại
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F) && !isDialogueActive)
        {
            StartDialogue();
        }

        // Khi nhấn Space, bỏ qua hiệu ứng chạy chữ
        if (Input.GetKeyDown(KeyCode.Space) && isReading)
        {
            isSkipping = true;
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true; // Đánh dấu hội thoại đang diễn ra
        NPCPanel.SetActive(true);
        buttonBG.SetActive(true);
        pressFUI.SetActive(false); // Ẩn hướng dẫn nhấn F
        coroutine = StartCoroutine(ReadContent());

        // Tắt CharacterController của Player để tránh di chuyển khi đang đọc hội thoại
        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
        }
    }

    private IEnumerator ReadContent()
    {
        for (int i = 0; i < content.Length; i++)
        {
            isReading = true;
            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "Unknown"; // Hiển thị tên NPC

            foreach (var item in content[i])
            {
                if (isSkipping)
                {
                    NPCContent.text = content[i]; // Hiện toàn bộ ngay lập tức
                    yield return null;
                    break;
                }

                NPCContent.text += item;
                yield return new WaitForSeconds(0.05f); // Tốc độ chạy chữ
            }

            isSkipping = false; // Reset trạng thái bỏ qua

            // Chờ người chơi bấm Space để chuyển sang câu tiếp theo
            while (!isSkipping)
            {
                yield return null;
            }
        }

        isReading = false; // Kết thúc đọc
        ShowButtons();
    }

    private void ShowButtons()
    {
        quitButton.gameObject.SetActive(true);
        startEventButton.gameObject.SetActive(true);
    }

    public void EndContent()
    {
        isDialogueActive = false; // Đánh dấu hội thoại đã kết thúc
        NPCPanel.SetActive(false);
        buttonBG.SetActive(false);
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
        EndContent();
        Debug.Log("Sự kiện bắt đầu!");
        if (evenGameObject != null)
        {
            evenGameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            pressFUI.SetActive(true); // Hiển thị hướng dẫn nhấn F
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            pressFUI.SetActive(false); // Ẩn hướng dẫn nhấn F
            EndContent();
        }
    }
}
