using System.Linq;
using UnityEngine;

public class TownUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeVisuals; // Array of upgrade visuals
    public bool canUpgrade = false; // Flag to check if the town can be upgraded
    public int currentUpgradeLevel = 0; // Current upgrade level


    public void UpgradeTown()
    {
        upgradeVisuals[currentUpgradeLevel].SetActive(false);
        currentUpgradeLevel++;
        upgradeVisuals[currentUpgradeLevel].SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Package") && canUpgrade)
        {
            if (currentUpgradeLevel > upgradeVisuals.Length - 2)
            {
                canUpgrade = false;
            }
            else
            {
                UpgradeTown();
            }
        }
    }
}
