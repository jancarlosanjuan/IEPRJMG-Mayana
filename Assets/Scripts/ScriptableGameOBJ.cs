using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Data",
                 menuName = "Scriptable OBJ/ New Player Data")]
public class ScriptableGameOBJ : ScriptableObject
{
    public GameObject selectedPet;
    public GameObject selectedBG;
}
