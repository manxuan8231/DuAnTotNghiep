using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Coroutine damageCoroutine; // Biến lưu Coroutine gây sát thương
    public float damageInterval = 1.0f; // Thời gian giữa các lần gây sát thương
    public int damageAmount = 30; // Lượng sát thương mỗi lần gây

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true; // Bật collider
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Bắt đầu gây sát thương liên tục
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Dừng gây sát thương khi Player rời vùng
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(Collider other)
    {
        SliderHp sliderHp = other.gameObject.GetComponent<SliderHp>();

        while (true) // Vòng lặp vô hạn, sẽ dừng khi `StopCoroutine` được gọi
        {
            yield return new WaitForSeconds(damageInterval); // Chờ một khoảng thời gian
            if (sliderHp != null)
            {
                sliderHp.TakeDame(damageAmount);
            }
        }
    }
}
