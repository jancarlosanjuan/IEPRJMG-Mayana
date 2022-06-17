using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TaskUpdate : MonoBehaviour
{
    [SerializeField]
    private TaskUI taskPrefab;
    [SerializeField]
    private ScriptableGameOBJ playerStats;
    [SerializeField]
    private GameObject content;


    private void Start()
    {
        for(int i = 0; i < playerStats.filteredList.tasksList.Count; i++)
        {
            TaskUI temp = GameObject.Instantiate(taskPrefab, content.transform);
            temp.title.text = playerStats.filteredList.tasksList[i].title;
            temp.description.text = playerStats.filteredList.tasksList[i].notes;
        }
    }

}
