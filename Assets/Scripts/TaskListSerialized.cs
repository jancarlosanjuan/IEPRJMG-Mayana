using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskListSerialized
{
    public string TaskListID;
    public List<Task> tasks = new List<Task>();
}

[System.Serializable]
public class AccountSerialized
{
    public string Email;
    public List<TaskListSerialized> tasksListSerialized = new List<TaskListSerialized>();

    public string selectedPetName;
    public string selectedBGName;

    public int hp;
    public int food;
    public int money;

    public List<string> costumeList = new List<string>();


}

[System.Serializable]
public class AccountsListSerialized
{
    public List<AccountSerialized> accountsSerialized = new List<AccountSerialized>();
}