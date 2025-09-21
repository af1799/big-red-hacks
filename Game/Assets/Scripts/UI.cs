using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconContainer;

    private List<GameObject> icons = new List<GameObject>();
    public int length=0;

    public void AddOneIcon()
    {
        if (length >= icons.Count)
        {
            GameObject newIcon = Instantiate(iconPrefab, iconContainer);
            icons.Add(newIcon);
        }

        icons[length].SetActive(true);
        length++;
    }

    public void RemoveOneIcon()
    {
        if (length > 0)
        {
            length--;
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].SetActive(i < length);
            }
        }
    }

}
