using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class OnLoad : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public ScriptableGameOBJ playerData;
    public string mobilePath;

    private int currentYearInt;
    private int currentMonthInt;
    private int currentDayInt;

    private int currentTaskYearInt;
    private int currentTaskMonthInt;
    private int currentTaskDayInt;

    public CostumeData costumeData;

    private void Start()
    {
        string currentDate = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy MM dd");

        string currentYearStr = currentDate.Substring(0, 4);
        string currentMonthStr = currentDate.Substring(5, 2);
        string currentDayStr = currentDate.Substring(8, 2);

        currentYearInt = Int32.Parse(currentYearStr);
        currentMonthInt = Int32.Parse(currentMonthStr);
        currentDayInt = Int32.Parse(currentDayStr);
    }

    public TriggerEvent onLoad;

    private void OnEnable()
    {
        onLoad.Attach(LoadJSON);
    }

    private void OnDisable()
    {
        onLoad.Detach(LoadJSON);
    }

    public void LoadJSON()
    {
        /*
        mobilePath = Application.persistentDataPath;
        string json = File.ReadAllText(mobilePath + "/" + gameManager.GoogleUser.Email + ".json");
        JsonUtility.FromJsonOverwrite(json, playerData);

        
        // FILTER LIST
        for (int i = 0; i < playerData.filteredList.tasksList.Count; i++)
        {
            // DUE DATE FORMAT: 2022-06-08T00:00:000Z
            // check if overdue
            string currentTaskYearStr = playerData.filteredList.tasksList[i].dueDate.Substring(0, 4);
            string currentTaskMonthStr = playerData.filteredList.tasksList[i].dueDate.Substring(5, 2);
            string currentTaskDayStr = playerData.filteredList.tasksList[i].dueDate.Substring(8, 2);

            currentTaskYearInt = Int32.Parse(currentTaskYearStr);
            currentTaskMonthInt = Int32.Parse(currentTaskMonthStr);
            currentTaskDayInt = Int32.Parse(currentTaskDayStr);

            // compare if completed
            if (playerData.filteredList.tasksList[i].status == "completed")
            {
                if (playerData.filteredList.tasksList[i].isListedAsComplete == false)
                {
                    playerData.completedTasks++;
                    playerData.filteredList.tasksList[i].isListedAsComplete = true;
                    //playerData.filteredList.tasksList.RemoveAt(i);
                }
            }

            // compare to current date
            else if (currentTaskYearInt <= currentYearInt)
            {
                if (currentTaskMonthInt <= currentMonthInt)
                {
                    if (currentTaskDayInt < currentDayInt)
                    {
                        if (playerData.filteredList.tasksList[i].isListedAsOverDue == false)
                        {
                            playerData.overduedTasks++;
                            playerData.filteredList.tasksList[i].isListedAsOverDue = true;
                            //playerData.filteredList.tasksList.RemoveAt(i);
                        }
                    }
                }
            }
        }


        for (int i = 0; i < playerData.costumeList.Count; i++)
        {
            CostumeType parsed_enum = (CostumeType)System.Enum.Parse(typeof(CostumeType), playerData.costumeList[i]);
            costumeData.list.Add(parsed_enum);
        }
        */
    }
}