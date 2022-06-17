using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyConfirmation : MonoBehaviour
{
    [SerializeField]
    private GameObject success;
    [SerializeField]
    private GameObject fail;
    [SerializeField]
    private ScriptableGameOBJ playerData;
    [SerializeField]
    private ItemEvent onBuyItem;
    [SerializeField]
    private IntEvent onChangeMoney;
    [SerializeField]
    private IntEvent onChangeHealth;
    [SerializeField]
    private IntEvent onChangeFood;
    [SerializeField]
    private TriggerEvent onCloseShop;
    [SerializeField]
    private TriggerEvent onCheckForCostumes;
    [SerializeField]
    private CostumeData data;

    private void Start()
    {
        HidePanels();
        onBuyItem.Attach(AssessBoughtItem);
    }

    private void OnDisable() => onBuyItem.Detach(AssessBoughtItem);
  

    public void AssessBoughtItem(Item item)
    {
        onCloseShop.Invoke();
        if (playerData.money < item.price)
        {
            fail.SetActive(true);
            return;
        }
            
      
        success.SetActive(true);
        playerData.money -= item.price;
        onChangeMoney.Invoke(playerData.money);

        if(item.type == ItemType.Consumable)
        {
            item.Consume(playerData);
            if (item.consumableType == ConsumableType.Health)
                onChangeHealth.Invoke(playerData.hp);
            else if(item.consumableType == ConsumableType.Food)
                onChangeFood.Invoke(playerData.food);
        }

        if(item.type == ItemType.Costume)
        {
            data.list.Add(item.costumeType);
            playerData.PopulateCostumeList(data);
            onCheckForCostumes.Invoke();
            data.UpdateReadOnly();
        }
    }

    public void HidePanels()
    {
        success.SetActive(false);
        fail.SetActive(false);
    }

}
