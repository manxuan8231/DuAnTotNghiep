using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public TextMeshProUGUI textUnlockSkill;
    private string skillE = "Chúc mừng bạn mở khóa skill FreeFire";
    private string skillR = "Chúc mừng bạn mở khóa skill Ám sát";
    private string skillZ = "Chúc mừng bạn mở khóa skill Gomen Amanai";
    private string skillC = "Chúc mừng bạn mở khóa skill Triệu hồi quân đoàn bóng tối";

    private SliderHp sliderHp;
    void Start()
    {
        sliderHp = FindAnyObjectByType<SliderHp>();
        textUnlockSkill.enabled = false;
    }

    
    void Update()
    {
        
        float lv = sliderHp.GetCurrentLevel();

        if(lv == 5)
        {
            StartCoroutine(TextSkill(skillE));
        }
        if(lv == 10) 
        {
            StartCoroutine(TextSkill(skillR));
        }
        if (lv == 20)
        {
            StartCoroutine(TextSkill(skillZ));
        }
        if (lv == 30)
        {
            StartCoroutine(TextSkill(skillC));
        }
    }
    private IEnumerator TextSkill(string text)
    {
        textUnlockSkill.enabled = true;
        yield return new WaitForSeconds(3f);
        textUnlockSkill.enabled = false;
    }
}
