using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PetSelectionManager : MonoBehaviour
{
    private int selectedSprite = 0;
    private bool isButtonPressed = false;

    public List<Sprite> spriteList = new List<Sprite>();
    public GameObject choosenPet;
    public ScriptableGameOBJ choosenPetData;

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
        if (choosenPetData.selectedPet.GetComponent<SpriteRenderer>())
            choosenPetData.selectedPet.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];
        SceneManager.LoadScene("BackGroundSelection");
    }
}
