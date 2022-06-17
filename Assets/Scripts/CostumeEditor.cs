using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CostumeSprites
{
    public Sprite sprite;
    public CostumeType type;
}

public class CostumeEditor : MonoBehaviour
{
    public CostumeSprites[] sprites;
    [SerializeField]
    private SpriteRenderer toEdit;
    [SerializeField]
    private TriggerEvent changeSprite;
    [SerializeField]
    private CostumeData data;

    private int index = 0;


    private void Awake()
    {
        changeSprite.Attach(ChangeSprite);
    }

    private void OnDisable()
    {
        changeSprite.Detach(ChangeSprite);
    }


    public void ChangeSprite()
    {
        if (data.list.Count == 0)
            return;

        index++;
        if (index >= sprites.Length)
            index = 0;

        while (!data.list.Contains(sprites[index].type) && index != 0)
        {
            index++;
            if (index >= sprites.Length)
                index = 0;
        }
       
        toEdit.sprite = sprites[index].sprite;
    }

}
