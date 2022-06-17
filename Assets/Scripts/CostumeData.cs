using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Costume Data",
                 menuName = "Scriptable OBJ/Costume Data")]
public class CostumeData : ScriptableObject
{
    public HashSet<CostumeType> list = new HashSet<CostumeType>();
    [SerializeField]
    private List<CostumeType> readOnly = new List<CostumeType>();

    public void UpdateReadOnly()
    {
        readOnly.Clear();
        foreach (CostumeType type in list)
        {
            readOnly.Add(type);
        }
    }
}
