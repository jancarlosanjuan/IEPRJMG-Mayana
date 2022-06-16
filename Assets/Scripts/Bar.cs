using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public IntEvent onChangeValue;

    private void Start() => onChangeValue.Attach(SetHealth);
    private void OnDisable() => onChangeValue.Detach(SetHealth);
    public void SetHealth(int health) => slider.value = health;
    
}
