using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public ScriptableGameOBJ playerData;
    private void Start()
    {
        Debug.Log(playerData.selectedPet.name);
        Debug.Log(playerData.selectedBG.name);
    }
}
