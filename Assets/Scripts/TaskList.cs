using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task List",
                 menuName = "Scriptable OBJ/ Task List")]
public class TaskList : ScriptableObject
{
    [SerializeField]
    public List<Task> tasksList = new List<Task>();
}
