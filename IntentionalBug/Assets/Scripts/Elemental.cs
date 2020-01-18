using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elemental : MonoBehaviour,  IElemental
{
    Elements currentElement;
    [SerializeField] ElementalEventController elementController;
    [SerializeField] UnityEvent<Elements> onSwitch;
    public Elements Element
    {
        get => currentElement;
        private set
        {
            currentElement = value;
        }
    }

    public void SwitchElement(Elements element)
    {
        Element = element;
        onSwitch.Invoke(element);
    }


    private void OnEnable()
    {
        elementController.RegisterElmentSwitch(this);
    }

    private void OnDisable()
    {
        elementController.DeregisterElmentSwitch(this);
    }

}
