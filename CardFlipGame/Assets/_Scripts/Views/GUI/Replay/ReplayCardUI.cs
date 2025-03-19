using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayCardUI : MonoBehaviour
{
    [SerializeField] Image cardFront;
    [SerializeField] Image cardBack;
    [SerializeField] Transform cardHolder;
    [SerializeField] float flipCardTime;

    bool isDown;
    Coordinate coord;

    private void OnEnable()
    {
        cardHolder.gameObject.SetActive(true);

    }
    private ReplayingUIManager cardUIManager;

    public float FlipCardTime { get => flipCardTime; }

    public void SetUpCard(Sprite sprite, Coordinate coord, ReplayingUIManager cardUIManager)
    {
        cardFront.sprite = sprite;
        cardFront.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(true);
        this.cardUIManager = cardUIManager;
        this.coord = coord;
        isDown = true;
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
    }

    public void FlipCard()
    {
        cardHolder.DORotate(new Vector3(0, 90, 0), FlipCardTime / 2f, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);
        StartCoroutine(WaitFlipHalfThenSetCardBackActive(!cardBack.gameObject.activeSelf));
    }

    public void FlipCardUp()
    {
        cardHolder.DORotate(new Vector3(0, 90, 0), FlipCardTime / 2f, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);
        StartCoroutine(WaitFlipHalfThenSetCardBackActive(true));
    }
    public void FlipCardDown()
    {
        cardHolder.DORotate(new Vector3(0, 90, 0), (FlipCardTime / 2f), RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);
        StartCoroutine(WaitFlipHalfThenSetCardBackActive(false));
    }

    public void DisactiveCard()
    {
        cardHolder.gameObject.SetActive(false);
    }

    IEnumerator WaitFlipHalfThenSetCardBackActive(bool isCardBackActive)
    {
        yield return new WaitUntil(() => cardHolder.localRotation.eulerAngles.y == 90);
        cardHolder.DORotate(new Vector3(0, -90, 0), (FlipCardTime / 2f), RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);

        cardFront.gameObject.SetActive(isCardBackActive);
        cardBack.gameObject.SetActive(!isCardBackActive);
    }

    IEnumerator WaitThenDo(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    public float SetHeightReturnWidth(float height)
    {
        var scale = height / cardHolder.GetComponent<RectTransform>().rect.height;
        var width = cardHolder.GetComponent<RectTransform>().rect.width * scale;
        cardHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        return cardHolder.GetComponent<RectTransform>().rect.width;
    }
}
