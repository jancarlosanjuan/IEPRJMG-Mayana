using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameManager gameManager;
    private void Start()
    {
        text.text = gameManager.userEmail;
    }
}
