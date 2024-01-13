using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnHold;
    bool IsHolding;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHolding = false;
    }

    void Update()
    {
        if (IsHolding)
        {
            OnHold.Invoke();
        }
    }
}
