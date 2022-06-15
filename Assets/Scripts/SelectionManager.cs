using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SelectionManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> spriteList = new List<Sprite>();

    private int selectedSprite = 0;
    public GameObject choosenPet;
    public GameObject choosenBG;

    public void NextSprite()
    {
        selectedSprite++;
        if (selectedSprite == spriteList.Count) { selectedSprite = 0; }
        spriteRenderer.sprite = spriteList[selectedSprite];
    }
    
    public void PreviousSprite()
    {
        selectedSprite--;
        if (selectedSprite < 0) { selectedSprite = spriteList.Count - 1; }
        spriteRenderer.sprite = spriteList[selectedSprite];
    }
    
    public void ConfirmPet()
    {
        // make this into scriptable object
        PrefabUtility.SaveAsPrefabAsset(choosenPet, "Assets/Prefabs/selectedPet.prefab");
        SceneManager.LoadScene("BackGroundSelection");
    }

    public void ConfirmBG()
    {
        // make this into scriptable object
        PrefabUtility.SaveAsPrefabAsset(choosenBG, "Assets/Prefabs/selectedBG.prefab");
        SceneManager.LoadScene("GameScene");
    }

    public void BackToPetSelection()
    {
        SceneManager.LoadScene("PetSelectionScene");
    }
}
