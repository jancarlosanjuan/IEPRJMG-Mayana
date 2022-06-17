using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSpawner : MonoBehaviour
{
    public ScriptableGameOBJ playerData;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerData.selectedBG);
        Instantiate(playerData.selectedPet);
    }
}
