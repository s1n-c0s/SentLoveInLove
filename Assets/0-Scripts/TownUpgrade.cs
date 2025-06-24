using UnityEngine;

public class TownUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeVisuals;
    public bool canUpgrade = false;
    public int currentUpgradeLevel = 0;
    
    private string currentBuildingOwner = "None";
    private bool isBuildingComplete = false;
    private TileCapture associatedTile;

    private void Start()
    {
        // Find the associated tile and subscribe to ownership changes
        associatedTile = GetComponentInParent<TileCapture>();
        if (associatedTile != null)
        {
            associatedTile.OnOwnershipChanged.AddListener(OnTileOwnershipChanged);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (associatedTile != null)
        {
            associatedTile.OnOwnershipChanged.RemoveListener(OnTileOwnershipChanged);
        }
    }

    private void UpgradeTown()
    {
        upgradeVisuals[currentUpgradeLevel].SetActive(false);
        currentUpgradeLevel++;
        upgradeVisuals[currentUpgradeLevel].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Package") && canUpgrade)
        {
            if (currentUpgradeLevel < upgradeVisuals.Length - 1)
            {
                UpgradeTown();
            }
            else
            {
                canUpgrade = false; // Optional: disable further upgrades
                isBuildingComplete = true;
                CountBuildingOwner();
            }
        }
    }

    private void OnTileOwnershipChanged(string newOwner)
    {
        // Only handle ownership changes if building is complete
        if (!isBuildingComplete) return;

        // Decrement previous owner's building count
        if (currentBuildingOwner == "PersonA")
        {
            PlayerDataManager.Instance.DecrementBuildingCountA();
        }
        else if (currentBuildingOwner == "PersonB")
        {
            PlayerDataManager.Instance.DecrementBuildingCountB();
        }

        // Increment new owner's building count
        if (newOwner == "PersonA")
        {
            PlayerDataManager.Instance.IncrementBuildingCountA();
        }
        else if (newOwner == "PersonB")
        {
            PlayerDataManager.Instance.IncrementBuildingCountB();
        }

        currentBuildingOwner = newOwner;
    }

    private void CountBuildingOwner()
    {
        if (associatedTile == null) return;

        currentBuildingOwner = associatedTile.GetCurrentOwner();

        // Increment the initial owner's building count
        if (currentBuildingOwner == "PersonA")
        {
            PlayerDataManager.Instance.IncrementBuildingCountA();
        }
        else if (currentBuildingOwner == "PersonB")
        {
            PlayerDataManager.Instance.IncrementBuildingCountB();
        }
    }
}