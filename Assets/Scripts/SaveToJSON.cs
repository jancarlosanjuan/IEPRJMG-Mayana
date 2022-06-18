using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveToJSON : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private ScriptableGameOBJ playerData;


    void OnApplicationFocus(bool hasFocus)
    {
        if(gameManager.GoogleUser != null)
            updateJSONonAction(gameManager.GoogleUser.Email);
    }


    void OnApplicationQuit()
    {
        if (gameManager.GoogleUser != null)
            updateJSONonAction(gameManager.GoogleUser.Email);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (gameManager.GoogleUser != null)
            updateJSONonAction(gameManager.GoogleUser.Email);
    }


    public void updateJSONonAction(string email)
    {
        string json = File.ReadAllText(gameManager.filePath + gameManager.fileName);
        AccountsListSerialized accountsListSerialized = JsonUtility.FromJson<AccountsListSerialized>(json);


        int emailIndex = FindEmailIndexInJSON(accountsListSerialized, email);
        if (emailIndex != -1)
        {
            AccountSerialized account = accountsListSerialized.accountsSerialized[emailIndex];

            account.selectedPetName = playerData.selectedPetName;
            account.selectedBGName = playerData.selectedBGName;
            account.hp = playerData.hp;
            account.food = playerData.food;
            account.money = playerData.money;
            account.costumeList = playerData.costumeList;


            updateJSONfile(accountsListSerialized, gameManager.filePath, gameManager.fileName);
        }


    }

    private int FindEmailIndexInJSON(AccountsListSerialized list, string email)
    {
        if (list == null) return -1;
        for (int i = 0; i < list.accountsSerialized.Count; i++)
        {
            if (list.accountsSerialized[i].Email == email)
            {
                return i;
            }
        }

        return -1;
    }

    private void updateJSONfile(AccountsListSerialized list, string filepath, string filename)
    {
        string emptyJson2 = JsonUtility.ToJson(list, true);
        //googleUserEmailTXT.text = "UpdatedJSONfile \n at:\n " +filepath+filename;
        Debug.Log("Created JSON data");
        File.WriteAllText(filepath + filename, emptyJson2);
    }
}
