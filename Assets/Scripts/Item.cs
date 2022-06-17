using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item
{
    public ItemType type;
    public ConsumableType consumableType;
    public CostumeType costumeType;
    public int price;

    public void Consume(ScriptableGameOBJ playerData)
    {
        if (type != ItemType.Consumable)
            return;

        if(consumableType == ConsumableType.Health)
        {
            playerData.hp++;
            if (playerData.hp > 10)
                playerData.hp = 10;
        }
        else if(consumableType == ConsumableType.Food)
        {
            playerData.food++;
            if (playerData.food > 10)
                playerData.food = 10;
        }
    }
}


public enum ItemType
{
    None,
    Consumable,
    Costume,
}

public enum ConsumableType
{
    None,
    Health,
    Food,
}

public enum CostumeType
{ 
    None,
    Wizard,
    Office,
    Angel,
    Dinosaur,
    Police,
    Scary,
}


