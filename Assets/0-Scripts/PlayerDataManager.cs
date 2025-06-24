using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string _playerNameA;
    [SerializeField] private int _TileA;
    [SerializeField] private int _selectCharacterA;
    [SerializeField] private int _buttonPressA;
    [SerializeField] private int _BuildingCountA;

    [SerializeField] private string _playerNameB;
    [SerializeField] private int _TileB;
    [SerializeField] private int _selectCharacterB;
    [SerializeField] private int _buttonPressB;
    [SerializeField] private int _BuildingCountB;

    public PlayerData(string playerNameA, int tileA, int selectCharacterA, int buttonPressA, int buildingCountA,
                      string playerNameB, int tileB, int selectCharacterB, int buttonPressB, int buildingCountB)
    {
        _playerNameA = playerNameA;
        _TileA = tileA;
        _selectCharacterA = selectCharacterA;
        _buttonPressA = buttonPressA;
        _BuildingCountA = buildingCountA;

        _playerNameB = playerNameB;
        _TileB = tileB;
        _selectCharacterB = selectCharacterB;
        _buttonPressB = buttonPressB;
        _BuildingCountB = buildingCountB;
    }

    public string PlayerNameA => _playerNameA;
    public int TileA => _TileA;
    public int SelectCharacterA => _selectCharacterA;
    public int ButtonPressA => _buttonPressA;
    public int BuildingCountA => _BuildingCountA;

    public string PlayerNameB => _playerNameB;
    public int TileB => _TileB;
    public int SelectCharacterB => _selectCharacterB;
    public int ButtonPressB => _buttonPressB;
    public int BuildingCountB => _BuildingCountB;

    public void UpdateSelectCharacterA(int val) => _selectCharacterA = val;
    public void UpdateSelectCharacterB(int val) => _selectCharacterB = val;
    public void IncrementButtonPressA() => _buttonPressA++;
    public void IncrementButtonPressB() => _buttonPressB++;

    public void IncrementTileA() => _TileA++;
    public void DecrementTileA() { if (_TileA > 0) _TileA--; }

    public void IncrementTileB() => _TileB++;
    public void DecrementTileB() { if (_TileB > 0) _TileB--; }

    public void IncrementBuildingCountA() => _BuildingCountA++;
    public void DecrementBuildingCountA() { if (_BuildingCountA > 0) _BuildingCountA--; }

    public void IncrementBuildingCountB() => _BuildingCountB++;
    public void DecrementBuildingCountB() { if (_BuildingCountB > 0) _BuildingCountB--; }
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    public PlayerData playerData;
    [SerializeField] private bool firstCheck = false;

    private int lastWinner = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optional: Uncomment if you want this object to persist
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (firstCheck == true)
        {
            CheckAndUpdateFX();
        }
    }

    public void SavePlayerData(string playerNameA, int tileA, int selectCharacterA, int buttonPressA, int buildingCountA,
                               string playerNameB, int tileB, int selectCharacterB, int buttonPressB, int buildingCountB)
    {
        playerData = new PlayerData(playerNameA, tileA, selectCharacterA, buttonPressA, buildingCountA,
                                    playerNameB, tileB, selectCharacterB, buttonPressB, buildingCountB);
    }

    public int GetWinner()
    {
        int aScore = playerData.TileA + playerData.ButtonPressA;
        int bScore = playerData.TileB + playerData.ButtonPressB;

        return aScore > bScore ? 0 : (bScore > aScore ? 1 : -1); // 0 = A wins, 1 = B wins, -1 = tie
    }

    public void CheckAndUpdateFX()
    {
        EndFXPlayer[] fxPlayers = FindObjectsOfType<EndFXPlayer>();

        int aScore = playerData.TileA + playerData.ButtonPressA + playerData.BuildingCountA * 5;
        int bScore = playerData.TileB + playerData.ButtonPressB + playerData.BuildingCountB * 5;

        int currentWinner = aScore > bScore ? 0 : (bScore > aScore ? 1 : -1);

        if (currentWinner == lastWinner) return;

        foreach (var fx in fxPlayers)
        {
            fx.PlayAreaFX();

            if (currentWinner == -1)
            {
                fx.PlayCrownFX();
            }
            else if (currentWinner == 0 && fx.CompareTag("PersonA"))
            {
                fx.PlayCrownFX();
            }
            else if (currentWinner == 1 && fx.CompareTag("PersonB"))
            {
                fx.PlayCrownFX();
            }
            else
            {
                fx.DisableCrownFX();
            }
        }

        lastWinner = currentWinner;
    }

    public void UpdateSelectCharacterA(int val) => playerData?.UpdateSelectCharacterA(val);
    public void UpdateSelectCharacterB(int val) => playerData?.UpdateSelectCharacterB(val);
    public void IncrementButtonPressA() { playerData?.IncrementButtonPressA(); }
    public void IncrementButtonPressB() { playerData?.IncrementButtonPressB(); }

    public void IncrementTileA() { playerData?.IncrementTileA(); }
    public void IncrementTileB() { playerData?.IncrementTileB(); }
    public void DecrementTileA()
    {
        playerData?.DecrementTileA();
        if (firstCheck == false)
        {
            CheckAndUpdateFX();
            firstCheck = true;
        }
    }
    public void DecrementTileB() => playerData?.DecrementTileB();

    public void IncrementBuildingCountA() { playerData?.IncrementBuildingCountA(); }
    public void IncrementBuildingCountB() { playerData?.IncrementBuildingCountB(); }
    public void DecrementBuildingCountA() => playerData?.DecrementBuildingCountA();
    public void DecrementBuildingCountB() => playerData?.DecrementBuildingCountB();
}