using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeChecker : MonoBehaviour
{
    [SerializeField]
    private TriggerEvent onCheckCostumes;
    [SerializeField]
    private CostumeData data;
    [SerializeField]
    private GameObject button;

    private void Start()
    {
        button.SetActive(false);
    }


    private void Awake()
    {
        onCheckCostumes.Attach(CheckForCostumes);
    }

    private void OnDisable()
    {
        onCheckCostumes.Detach(CheckForCostumes);
    }

    public void CheckForCostumes()
    {
        button.gameObject.SetActive(false);
        if (data.list.Count > 0)
            button.gameObject.SetActive(true);
    }
}
