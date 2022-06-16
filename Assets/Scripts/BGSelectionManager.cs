using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSelectionManager : MonoBehaviour
{
    private int selectedSprite = 0;
    private bool isButtonPressed = false;

    public List<Sprite> spriteList = new List<Sprite>();
    public GameObject choosenBG;
    public ScriptableGameOBJ choosenBGData;

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
        // make this into scriptable object
        if (choosenBGData.selectedBG.GetComponent<SpriteRenderer>())
            choosenBGData.selectedBG.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];
        SceneManager.LoadScene("GameScene");
    }

    public void BackToPetSelection()
    {
        if (choosenBGData.selectedBG.GetComponent<SpriteRenderer>())
            choosenBGData.selectedBG.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];
        SceneManager.LoadScene("PetSelectionScene");
    }
}
