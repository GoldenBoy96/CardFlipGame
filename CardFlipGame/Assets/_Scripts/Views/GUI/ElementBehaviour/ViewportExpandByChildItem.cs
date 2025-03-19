using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewportExpandByChildItem : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] RectTransform templateContent;

    [Header("Viewport size limit")]
    [SerializeField] bool limitSize = false;
    [SerializeField] int maxChildShowAtATime;

    public void SetViewportSize()
    {
        var dropdownList = GameObject.Find("Dropdown List").GetComponent<RectTransform>();
        int itemNumber = dropdown.options.Count;
        if (limitSize == true)
        {
            if (itemNumber > maxChildShowAtATime) itemNumber = maxChildShowAtATime;
        }
        Vector2 newSize = new(
            templateContent.rect.width,
            itemNumber * templateContent.rect.height
            );
        dropdownList.sizeDelta = newSize;
    }

}
