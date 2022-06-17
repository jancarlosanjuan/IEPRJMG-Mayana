using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OnLoad : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public ScriptableGameOBJ playerData;
    public string mobilePath;

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
        mobilePath = Application.persistentDataPath;
        string json = File.ReadAllText(mobilePath + "/" + gameManager.GoogleUser.Email + ".json");
        JsonUtility.FromJsonOverwrite(json, playerData);

        /*
        // filter out completed and 
        for (int i = 0; i < playerData.filteredList.tasksList.Count; i++)
        {
            // compare if completed
            if (playerData.filteredList.tasksList[i].status == "Completed")
            {
                // remove
                //playerData.filteredList.tasksList[i].
                //playerData.completedTasks++;
            }

            // check if overdue
            
            else if ()
            {
                // remove
                //playerData.filteredList.tasksList[i].
                playerData.overduedTasks++;
            }
        }*/
    }
}
