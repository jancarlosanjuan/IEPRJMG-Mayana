using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class PlayerDataSpawner : MonoBehaviour
{
    public ScriptableGameOBJ playerData;
    private string mobilePath;

    public List<GameObject> petList = new List<GameObject>();
    public List<GameObject> bgList = new List<GameObject>();
    public TaskList list;
    public CostumeData costumeData;


    private int currentYearInt;
    private int currentMonthInt;
    private int currentDayInt;

    private int currentTaskYearInt;
    private int currentTaskMonthInt;
    private int currentTaskDayInt;

    private void Awake()
    {
        string currentDate = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy MM dd");

        string currentYearStr = currentDate.Substring(0, 4);
        string currentMonthStr = currentDate.Substring(5, 2);
        string currentDayStr = currentDate.Substring(8, 2);

        currentYearInt = Int32.Parse(currentYearStr);
        currentMonthInt = Int32.Parse(currentMonthStr);
        currentDayInt = Int32.Parse(currentDayStr);

        AssignSelectedPet();
        AssignSelectedBG();
        AssignTaskList();
        AssignCostumeData();

        mobilePath = Application.persistentDataPath;
        string loadjson = File.ReadAllText(mobilePath + "/" + playerData.emailID + ".json");
        JsonUtility.FromJsonOverwrite(loadjson, playerData);

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
            if (playerData.filteredList.tasksList[i].status == "Completed")
            {
                if (playerData.filteredList.tasksList[i].isListedAsComplete == false)
                {
                    playerData.completedTasks++;
                    playerData.filteredList.tasksList[i].isListedAsComplete = true;
                    playerData.filteredList.tasksList.RemoveAt(i);
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
                            playerData.filteredList.tasksList.RemoveAt(i);
                        }
                    }
                }
            }
        }

        Instantiate(playerData.selectedPet);
        Instantiate(playerData.selectedBG);

        // SAVE
        SaveData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        SaveData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        SaveData();
    }

    void SaveData()
    {
        Debug.Log("Called");
        mobilePath = Application.persistentDataPath;
        string saveJson = JsonUtility.ToJson(playerData);
        File.WriteAllText(mobilePath + "/" + playerData.emailID + ".json", saveJson);
    }

    private void AssignSelectedPet()
    {
        switch (playerData.selectedPetName)
        {
            case "Cat1":
                playerData.selectedPet = petList[0];
                break;

            case "Cat2":
                playerData.selectedPet = petList[1];
                break;

            case "Cat3":
                playerData.selectedPet = petList[2];
                break;

            case "Dog1":
                playerData.selectedPet = petList[3];
                break;

            case "Dog2":
                playerData.selectedPet = petList[4];
                break;

            case "Dog3":
                playerData.selectedPet = petList[5];
                break;
        }
    }

    private void AssignSelectedBG()
    {
        switch (playerData.selectedBGName)
        {
            case "BG1":
                playerData.selectedBG = bgList[0];
                break;

            case "BG2":
                playerData.selectedBG = bgList[1];
                break;

            case "BG3":
                playerData.selectedBG = bgList[2];
                break;
        }
    }

    private void AssignTaskList()
    {
        playerData.filteredList = list;
    }

    private void AssignCostumeData()
    {
        for (int i = 0; i < playerData.costumeList.Count; i++)
        {
            CostumeType parsed_enum = (CostumeType)System.Enum.Parse(typeof(CostumeType), playerData.costumeList[i]);
            costumeData.list.Add(parsed_enum);
        }
    }

    public void ResetPlayerDataTasks()
    {
        playerData.overduedTasks = 0;
        playerData.completedTasks = 0;
    }
}
