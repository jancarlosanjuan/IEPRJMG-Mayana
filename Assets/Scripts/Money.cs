using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public TextMeshProUGUI amount;
    public IntEvent onChangeMoney;

    void Awake() => onChangeMoney.Attach(SetMoney);
    private void OnDisable() => onChangeMoney.Detach(SetMoney);
    public void SetMoney(int money) => amount.text = money.ToString();
}
