using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion1 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TakeHealth(other)); // Truyền other vào
        }
    }

    public IEnumerator TakeHealth(Collider other)
    {
        SliderHp sliderHp = other.gameObject.GetComponent<SliderHp>(); // Lấy SliderHp từ Player

        if (sliderHp == null)
        {
            Debug.LogWarning("Không tìm thấy SliderHp trên " + other.gameObject.name);
            yield break;
        }

        yield return new WaitForSeconds(0.9f);
        sliderHp.TakeDame(150);
    }
}
