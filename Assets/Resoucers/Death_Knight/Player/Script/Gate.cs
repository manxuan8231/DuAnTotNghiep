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
            Key key =  FindAnyObjectByType<Key>();
            if (key != null)
            {
                key.textKey.enabled = true;
            }
            CanhCua canhCua = FindAnyObjectByType<CanhCua>();
            if (canhCua != null)
            {
                canhCua.enabled = true;
            }
            other.transform.position = tele.transform.position;
        }
    }
}
