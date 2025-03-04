using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemUsing : MonoBehaviour
{
    public SliderHp sliderHP;
    //public Item item;
    public void UsingItemHeal(Item item)
    {
        
        sliderHP.GetHp( item.itemValue);
    }
    public void UsingItemMana(Item item)
    {
      

        sliderHP.GetMana(item.itemValue);
       
    }
}
