using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserStatsManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableGameOBJ playerData;
    [SerializeField]
    private IntEvent onChangeHealth;
    [SerializeField]
    private IntEvent onChangeFood;
    [SerializeField]
    private IntEvent onChangeMoney;
    [SerializeField]
    private TriggerEvent onInitializeResources;
    [SerializeField]
    private TextMeshProUGUI resourceText;
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private void Start()
    {
        onInitializeResources.Invoke();

        if (playerData.overduedTasks > 0)
        {
           
            for(int i = 0; i < playerData.overduedTasks; i++)
            {
                if (playerData.food - 1 < 0)
                {
                    playerData.hp--;
                    if (playerData.hp < 0)
                        playerData.hp = 0;
                }
                else
                    playerData.food--;
                
            }
            resourceText.text = playerData.overduedTasks.ToString() + " incomplete tasks!";
        }
        
        if(playerData.completedTasks > 0)
        {
            moneyText.text = playerData.completedTasks.ToString() + " completed tasks! + " 
                + (playerData.completedTasks * 10).ToString() + " money!";

            playerData.money += playerData.completedTasks * 10;
        }

        onChangeHealth.Invoke(playerData.hp);
        onChangeFood.Invoke(playerData.food);
        onChangeMoney.Invoke(playerData.money);
    }
}
