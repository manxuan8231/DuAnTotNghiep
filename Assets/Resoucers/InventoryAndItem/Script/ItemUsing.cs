using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsing : MonoBehaviour
{
    public SliderHp sliderHP;
    public Item item;
    public void UsingItemHeal()
    {
        Debug.Log("1");
        sliderHP.GetHp(item.itemValue);

        
        

    }
    public void UsingItemMana()
    {
        Debug.Log("2");

        sliderHP.GetMana(item.itemValue);
       
    }
}
