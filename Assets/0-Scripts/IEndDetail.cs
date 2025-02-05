using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IEndDetail : MonoBehaviour
{
    [SerializeField] private bool isPersonA;
    private int ButtonIndex = 0;
    private int PackagesIndex = 0;
    private int TileIndex = 0;
    public TextMeshProUGUI ButtonIndexText;
    public TextMeshProUGUI PackagesIndexText;
    public TextMeshProUGUI TileIndexText;


    private void LateUpdate()
    {
        if (isPersonA)
        {
            ButtonIndex = PlayerDataManager.Instance.playerData.ButtonPressA;
            PackagesIndex = PlayerDataManager.Instance.playerData.PackageReceivedA;
            TileIndex = PlayerDataManager.Instance.playerData.TileA;
        }
        else
        {
            ButtonIndex = PlayerDataManager.Instance.playerData.ButtonPressB;
            PackagesIndex = PlayerDataManager.Instance.playerData.PackageReceivedB;
            TileIndex = PlayerDataManager.Instance.playerData.TileB;
        }

        ButtonIndexText.text = ButtonIndex.ToString();
        PackagesIndexText.text = PackagesIndex.ToString();
        TileIndexText.text = TileIndex.ToString();
    }
}

