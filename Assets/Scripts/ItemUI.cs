using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private ItemEvent onBuyItem;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private CostumeData data;
    [SerializeField]
    private GameObject buyButton;

    public Item item;

    private void OnValidate() => priceText.text = item.price.ToString();

    private void OnEnable()
    {
        if (item.type != ItemType.Costume)
            return;

        buyButton.gameObject.SetActive(true);
        if (data.list.Contains(item.costumeType))
            buyButton.gameObject.SetActive(false);
    }

    public void TryBuyItem() => onBuyItem.Invoke(item);
}
