using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Data",
                 menuName = "Scriptable OBJ/ New Player Data")]
public class ScriptableGameOBJ : ScriptableObject
{
    public string emailID;

    public string selectedPetName;
    public string selectedBGName;

    public int hp;
    public int food;
    public int money;

    public int completedTasks;
    public int overduedTasks;

    public TaskList filteredList;
    

    public List<string> costumeList = new List<string>();

    public void PopulateCostumeList(CostumeData costumeListData)
    {
        costumeList.Clear();
        foreach (CostumeType costume in costumeListData.list)
        {
            costumeList.Add(costume.ToString());
        }
    }
}

