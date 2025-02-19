using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsing : MonoBehaviour
{
    public SliderHp sliderHP;
    public Item item;
    public void UsingItemHeal()
    {
        sliderHP.GetHp(item.itemValue);
    }
    public void UsingItemMana()
    {
       
        sliderHP.GetMana(item.itemValue);
    }
}
