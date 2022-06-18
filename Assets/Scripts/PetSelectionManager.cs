using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PetSelectionManager : MonoBehaviour
{
    private int selectedSprite = 0;
    private bool isButtonPressed = false;

    public List<Sprite> spriteList = new List<Sprite>();
    public List<GameObject> petList = new List<GameObject>();

    public GameObject choosenPet;
    public ScriptableGameOBJ playerData;


    public void Update()
    {
        if (isButtonPressed)
        {
            choosenPet.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];
            isButtonPressed = false;
        }
    }

    public void NextPetSprite()
    {
        selectedSprite++;
        if (selectedSprite == spriteList.Count) { selectedSprite = 0; }
        isButtonPressed = true;
    }

    public void PreviousPetSprite()
    {
        selectedSprite--;
        if (selectedSprite < 0) { selectedSprite = spriteList.Count - 1; }
        isButtonPressed = true;
    }

    public void ConfirmPet()
    {    
        playerData.selectedPetName = petList[selectedSprite].name;
        // delete?

        SceneManager.LoadScene("BackGroundSelection");
    }
}
