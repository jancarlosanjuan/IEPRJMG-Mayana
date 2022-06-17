using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSelectionManager : MonoBehaviour
{
    private int selectedSprite = 0;
    private bool isButtonPressed = false;

    public List<Sprite> spriteList = new List<Sprite>();
    public List<GameObject> bgList = new List<GameObject>();

    public GameObject choosenBG;
    public ScriptableGameOBJ playerData;

    //public TriggerEvent onSave;

    private void Start()
    {
        Debug.Log(playerData.selectedPet.name);
        Instantiate(playerData.selectedPet);
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
        // make this into scriptable object
        if (playerData.selectedBG.GetComponent<SpriteRenderer>())
            playerData.selectedBG.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];

        // SAVE DATA
        //onSave.Invoke();

        SceneManager.LoadScene("GameScene");
    }

    public void BackToPetSelection()
    {
        if (playerData.selectedBG.GetComponent<SpriteRenderer>())
            playerData.selectedBG.GetComponent<SpriteRenderer>().sprite = spriteList[selectedSprite];

        if (playerData.selectedBG != null)
        {
            playerData.selectedBG = null;
            playerData.selectedBG = bgList[selectedSprite];
        }

        SceneManager.LoadScene("PetSelectionScene");
    }
}
