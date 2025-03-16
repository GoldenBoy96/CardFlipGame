using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] Image CardBack;

    public void FlipCard()
    {
        CardBack.gameObject.SetActive(!CardBack.gameObject.activeSelf);
    }

}
