using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string _playerNameA;
    [SerializeField] private int _TileA;
    [SerializeField] private int _selectCharacterA;
    [SerializeField] private int _buttonPressA;
    [SerializeField] private int _PackageReceivedA;

    [SerializeField] private string _playerNameB;
    [SerializeField] private int _TileB;
    [SerializeField] private int _selectCharacterB;
    [SerializeField] private int _buttonPressB;
    [SerializeField] private int _PackageReceivedB;

    public PlayerData(string playerNameA, int tileA, int selectCharacterA, int buttonPressA, int packageReceivedA, string playerNameB, int tileB, int selectCharacterB, int buttonPressB, int packageReceivedB)
    {
        this._playerNameA = playerNameA;
        this._TileA = tileA;
        this._selectCharacterA = selectCharacterA;
        this._buttonPressA = buttonPressA;
        this._PackageReceivedA = packageReceivedA;

        this._playerNameB = playerNameB;
        this._TileB = tileB;
        this._selectCharacterB = selectCharacterB;
        this._buttonPressB = buttonPressB;
        this._PackageReceivedB = packageReceivedB;
    }

    public string PlayerNameA { get { return _playerNameA; } }
    public int TileA { get { return _TileA; } }
    public int SelectCharacterA { get { return _selectCharacterA; } }
    public int ButtonPressA { get { return _buttonPressA; } }
    public int PackageReceivedA { get { return _PackageReceivedA; } }

    public string PlayerNameB { get { return _playerNameB; } }
    public int TileB { get { return _TileB; } }
    public int SelectCharacterB { get { return _selectCharacterB; } }
    public int ButtonPressB { get { return _buttonPressB; } }
    public int PackageReceivedB { get { return _PackageReceivedB; } }

    public void UpdateSelectCharacterA(int selectCharacterA)
    {
        _selectCharacterA = selectCharacterA;
    }

    public void UpdateSelectCharacterB(int selectCharacterB)
    {
        _selectCharacterB = selectCharacterB;
    }

    public void IncrementButtonPressA()
    {
        _buttonPressA++;
    }

    public void IncrementButtonPressB()
    {
        _buttonPressB++;
    }

    public void IncrementPackageReceivedA()
    {
        _PackageReceivedA++;
    }

    public void IncrementPackageReceivedB()
    {
        _PackageReceivedB++;
    }

    public void IncrementTileA()
    {
        _TileA++;
    }

    public void IncrementTileB()
    {
        _TileB++;
    }

    public void DecrementTileA()
    {
        if (_TileA > 0)
        {
            _TileA--;
        }
    }

    public void DecrementTileB()
    {
        if (_TileB > 0)
        {
            _TileB--;
        }
    }
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData(string playerNameA, int tileA, int selectCharacterA, int buttonPressA, int packageReceivedA, string playerNameB, int tileB, int selectCharacterB, int buttonPressB, int packageReceivedB)
    {
        playerData = new PlayerData(playerNameA, tileA, selectCharacterA, buttonPressA, packageReceivedA, playerNameB, tileB, selectCharacterB, buttonPressB, packageReceivedB);
        // Implement save logic here (e.g., PlayerPrefs, file, etc.)
    }

    public void LoadPlayerData()
    {
        // Implement load logic here (e.g., PlayerPrefs, file, etc.)
        // Example:
        // playerData = new PlayerData(loadedNameA, loadedTileA, loadedCharacterA, loadedButtonPressA, loadedPackageReceivedA, loadedNameB, loadedTileB, loadedCharacterB, loadedButtonPressB, loadedPackageReceivedB);
    }

    public void UpdateSelectCharacterA(int selectCharacterA)
    {
        if (playerData != null)
        {
            playerData.UpdateSelectCharacterA(selectCharacterA);
        }
    }

    public void UpdateSelectCharacterB(int selectCharacterB)
    {
        if (playerData != null)
        {
            playerData.UpdateSelectCharacterB(selectCharacterB);
        }
    }

    public void IncrementButtonPressA()
    {
        if (playerData != null)
        {
            playerData.IncrementButtonPressA();
        }
    }

    public void IncrementButtonPressB()
    {
        if (playerData != null)
        {
            playerData.IncrementButtonPressB();
        }
    }

    public void IncrementPackageReceivedA()
    {
        if (playerData != null)
        {
            playerData.IncrementPackageReceivedA();
        }
    }

    public void IncrementPackageReceivedB()
    {
        if (playerData != null)
        {
            playerData.IncrementPackageReceivedB();
        }
    }

    public void IncrementTileA()
    {
        if (playerData != null)
        {
            playerData.IncrementTileA();
        }
    }

    public void IncrementTileB()
    {
        if (playerData != null)
        {
            playerData.IncrementTileB();
        }
    }

    public void DecrementTileA()
    {
        if (playerData != null)
        {
            playerData.DecrementTileA();
        }
    }

    public void DecrementTileB()
    {
        if (playerData != null)
        {
            playerData.DecrementTileB();
        }
    }
}
