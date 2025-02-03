using UnityEngine;
using UnityEngine.UI;

public class ICaptureBar : MonoBehaviour
{
    [SerializeField] private Slider captureBar; // Assign UI Slider
    private PlayerDataManager playerDataManager;

    void Start()
    {
        playerDataManager = PlayerDataManager.Instance;
        captureBar.value = 0.5f; // Center the capture bar
    }

    void Update()
    {
        if (playerDataManager != null && playerDataManager.playerData != null)
        {
            float totalTiles = playerDataManager.playerData.TileA + playerDataManager.playerData.TileB;
            if (totalTiles > 0)
            {
                // Normalize capture progress (0 = full B, 1 = full A)
                captureBar.value = (float)playerDataManager.playerData.TileA / totalTiles;
            }
        }
    }
}
