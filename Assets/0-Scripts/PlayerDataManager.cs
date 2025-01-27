using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string _playerNameA;
    [SerializeField] private int _playerScoreA;
    [SerializeField] private int _selectCharacterA;

    [SerializeField] private string _playerNameB;
    [SerializeField] private int _playerScoreB;
    [SerializeField] private int _selectCharacterB;

    public PlayerData(string playerNameA, int playerScoreA, int selectCharacterA, string playerNameB, int playerScoreB, int selectCharacterB)
    {
        this._playerNameA = playerNameA;
        this._playerScoreA = playerScoreA;
        this._selectCharacterA = selectCharacterA;
        this._playerNameB = playerNameB;
        this._playerScoreB = playerScoreB;
        this._selectCharacterB = selectCharacterB;
    }

    public string PlayerNameA { get { return _playerNameA; } }
    public int PlayerScoreA { get { return _playerScoreA; } }
    public int SelectCharacterA { get { return _selectCharacterA; } }

    public string PlayerNameB { get { return _playerNameB; } }
    public int PlayerScoreB { get { return _playerScoreB; } }
    public int SelectCharacterB { get { return _selectCharacterB; } }
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

    public void SavePlayerData(string playerNameA, int playerScoreA, int selectCharacterA, string playerNameB, int playerScoreB, int selectCharacterB)
    {
        playerData = new PlayerData(playerNameA, playerScoreA, selectCharacterA, playerNameB, playerScoreB, selectCharacterB);
        // Implement save logic here (e.g., PlayerPrefs, file, etc.)
    }

    public void LoadPlayerData()
    {
        // Implement load logic here (e.g., PlayerPrefs, file, etc.)
        // Example:
        // playerData = new PlayerData(loadedNameA, loadedScoreA, loadedCharacterA, loadedNameB, loadedScoreB, loadedCharacterB);
    }

    public void UpdateSelectCharacterA(int selectCharacterA)
    {
        if (playerData != null)
        {
            playerData = new PlayerData(playerData.PlayerNameA, playerData.PlayerScoreA, selectCharacterA, playerData.PlayerNameB, playerData.PlayerScoreB, playerData.SelectCharacterB);
        }
    }

    public void UpdateSelectCharacterB(int selectCharacterB)
    {
        if (playerData != null)
        {
            playerData = new PlayerData(playerData.PlayerNameA, playerData.PlayerScoreA, playerData.SelectCharacterA, playerData.PlayerNameB, playerData.PlayerScoreB, selectCharacterB);
        }
    }
}
