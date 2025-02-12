using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion1 : MonoBehaviour
{
    public SliderHp sliderHp; // Tham chiếu đến SliderHp
    private Coroutine takeHealthCoroutine; // Tham chiếu đến coroutine TakeHealth

    private void Start()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            takeHealthCoroutine = StartCoroutine(TakeHealth(other)); // Lưu tham chiếu đến coroutine
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (takeHealthCoroutine != null)
            {
                StopCoroutine(takeHealthCoroutine); // Dừng coroutine nếu người chơi thoát khỏi box
            }
        }
    }

    public IEnumerator TakeHealth(Collider other)
    {
        sliderHp = other.gameObject.GetComponent<SliderHp>(); // Lấy SliderHp từ Player

        yield return new WaitForSeconds(0.9f);

        sliderHp.TakeDame(150);
    }
}
