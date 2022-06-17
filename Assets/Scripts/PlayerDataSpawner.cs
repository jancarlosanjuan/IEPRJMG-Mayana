using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PlayerDataSpawner : MonoBehaviour
{
    public ScriptableGameOBJ playerData;
    public TMP_Text player;
    public TMP_Text bg;
    private string mobilePath;


    public List<GameObject> petList = new List<GameObject>();
    public List<GameObject> bgList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AssignSelectedPet();
        AssignSelectedBG();

        mobilePath = Application.persistentDataPath;
        string json = File.ReadAllText(mobilePath + "/" + playerData.emailID + ".json");
        JsonUtility.FromJsonOverwrite(json, playerData);

        player.text = playerData.selectedPet.name;
        bg.text = playerData.selectedBG.name;

        if (playerData.selectedPet == null)
        {
            player.text = "NULL";
        }

        else
        {
            player.text = playerData.selectedPetName;

        }

        if (playerData.selectedBG == null)
        {
            bg.text = "NULL";
        }

        else
        {
            bg.text = playerData.selectedBGName;
        }

        Instantiate(playerData.selectedPet);
        Instantiate(playerData.selectedBG);
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
}
