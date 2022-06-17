using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public TriggerEvent onUserSuccessfulSignIn;
    public ScriptableGameOBJ playerData;
    public TaskList list;
    public string mobilePath;

    public GameObject selectedPet;
    public GameObject selectedBG;

    public TriggerEvent onSave;
    public TriggerEvent onLoad;


    private void OnEnable()
    {
        onUserSuccessfulSignIn.Attach(OnCheckUserExist);
    }

    private void OnDisable()
    {
        onUserSuccessfulSignIn.Detach(OnCheckUserExist);
    }

    public void OnCheckUserExist()
    {
        mobilePath = Application.persistentDataPath;
        if (File.Exists(mobilePath + "/" + gameManager.GoogleUser.Email + ".json"))
        {
            onLoad.Invoke();
            SceneManager.LoadScene("GameScene");
        }

        else
        {
            // create & set player data
            playerData.emailID = gameManager.GoogleUser.Email;

            playerData.selectedPet = selectedPet;
            playerData.selectedBG = selectedBG;

            playerData.hp = 10;
            playerData.food = 10;
            playerData.money = 0;
            playerData.completedTasks = 0;
            playerData.overduedTasks = 0;
            playerData.filteredList = list;

            onSave.Invoke();
            SceneManager.LoadScene("PetSelectionScene");
        }
    }
}
