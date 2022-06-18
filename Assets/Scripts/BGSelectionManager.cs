using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class BGSelectionManager : MonoBehaviour
{
    private int selectedSprite = 0;
    private bool isButtonPressed = false;

    public List<Sprite> spriteList = new List<Sprite>();
    public List<GameObject> petList = new List<GameObject>();
    public List<GameObject> bgList = new List<GameObject>();

    public GameObject choosenBG;
    public ScriptableGameOBJ playerData;

    public string mobilePath;

    private GameObject petObject;

    private void Start()
    {
        AssignSelectedPet();
        Debug.Log(petObject.name);
        Instantiate(petObject);
    }

    public void Update()
    {
        if (isButtonPressed)
        {
            choosenBG.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];
            isButtonPressed = false;
        }
    }

    public void NextBGSprite()
    {
        selectedSprite++;
        if (selectedSprite == spriteList.Count) { selectedSprite = 0; }
        isButtonPressed = true;
    }

    public void PreviousBGSprite()
    {
        selectedSprite--;
        if (selectedSprite < 0) { selectedSprite = spriteList.Count - 1; }
        isButtonPressed = true;
    }

    public void ConfirmBG()
    {
            
        playerData.selectedBGName = bgList[selectedSprite].name;

        // SAVE DATA
        SaveSelectedOBJs();
        SceneManager.LoadScene("GameScene");
    }

    public void BackToPetSelection()
    {
        playerData.selectedBGName = bgList[selectedSprite].name;
        // delete?

        SceneManager.LoadScene("PetSelectionScene");
    }

    public void SaveSelectedOBJs()
    {
        mobilePath = Application.persistentDataPath;
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(mobilePath + "/" + playerData.emailID + ".json", json);
    }

    private void AssignSelectedPet()
    {
        switch (playerData.selectedPetName)
        {
            case "Cat1":
                petObject = petList[0];
                break;

            case "Cat2":
                petObject = petList[1];
                break;

            case "Cat3":
                petObject = petList[2];
                break;

            case "Dog1":
                petObject = petList[3];
                break;

            case "Dog2":
                petObject = petList[4];
                break;

            case "Dog3":
                petObject = petList[5];
                break;
        }
    }
}
