using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Panels = new List<GameObject>();

    public void ShowPanel(string panelName)
    {
        HideAllPanels();
        foreach (GameObject panel in Panels)
        {
            if (panel.name == panelName)
            {
                panel.SetActive(true);
            }
        }
    }

    public void HideAllPanels()
    {
        foreach (GameObject panel in Panels)
        {
            panel.SetActive(false);
        }
    }

    public string GetCurrentPanel()
    {
        foreach (GameObject panel in Panels)
        {
            if (panel.activeSelf)
            {
                return panel.name;
            }
        }
        return null;
    }
}
