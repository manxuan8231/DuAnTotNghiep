using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemUsing : MonoBehaviour
{
    public SliderHp sliderHP;
    //public Item item;
    public void UsingItemHeal(Item item)
    {
        Debug.Log("using hp");
        sliderHP.GetHp( item.itemValue);
    }
    public void UsingItemMana(Item item)
    {
        Debug.Log("2");

        sliderHP.GetMana(item.itemValue);
       
    }
}
