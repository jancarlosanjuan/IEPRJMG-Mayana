using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TaskUpdate : MonoBehaviour
{
    [SerializeField]
    private TaskUI taskPrefab;
    [SerializeField]
    private TaskList filterTasks;
    [SerializeField]
    private GameObject content;


    private void Start()
    {
        for(int i = 0; i < filterTasks.tasksList.Count; i++)
        {
            //if (playerStats.filteredList.tasksList[i].isListedAsOverDue ||
                //playerStats.filteredList.tasksList[i].isListedAsComplete)
                //return;

            TaskUI temp = GameObject.Instantiate(taskPrefab, content.transform);
            temp.title.text = filterTasks.tasksList[i].title;
            temp.description.text = filterTasks.tasksList[i].notes;
        }
    }

}
