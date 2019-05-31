﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image bgImg;
    private Image jsImg;
    private Vector3 inputVector;

    void Start()
    {
        bgImg = GetComponent<Image>();
        jsImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 Pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out Pos))
        {
            Pos.x = (Pos.x / bgImg.rectTransform.sizeDelta.x);
            Pos.y = (Pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(Pos.x, Pos.y, 0).normalized;

            jsImg.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2),
                                                                            inputVector.y * (bgImg.rectTransform.sizeDelta.y / 2));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        jsImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetHorVal()
    {
        return inputVector.x;
    }

    public float GetVerVal()
    {
        return inputVector.y;
    }

}
