using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Data",
                 menuName = "Scriptable OBJ/ New Player Data")]
public class ScriptableGameOBJ : ScriptableObject
{
    public string emailID;

    public GameObject selectedPet;
    public string selectedPetName;

    public GameObject selectedBG;
    public string selectedBGName;

    public int hp;
    public int food;
    public int money;

    public int completedTasks;
    public int overduedTasks;

    public TaskList filteredList;

    //
}
