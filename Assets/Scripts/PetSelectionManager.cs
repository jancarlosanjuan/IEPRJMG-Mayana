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

    private void Start()
    {
        playerData.selectedPet = null;
        playerData.selectedPet = choosenPet;
    }

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
        // make this into scriptable object
        if (playerData.selectedPet.GetComponent<SpriteRenderer>())
            playerData.selectedPet.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];

        if (playerData.selectedPet != null)
        {
            playerData.selectedPet = null;
            playerData.selectedPet = petList[selectedSprite];
        }

        SceneManager.LoadScene("BackGroundSelection");
    }
}
