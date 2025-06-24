using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisualProbability
{
    public GameObject visual;
    [Range(0, 100)]
    public float probability; // Probability percentage (0-100)
}

public class LandCheck : MonoBehaviour
{
    [SerializeField] private List<VisualProbability> GroundVisuals;
    [SerializeField] private List<VisualProbability> obstacleVisuals;
    [SerializeField] private TownUpgrade townUpgrade;
    [SerializeField] private Node node;
    [SerializeField] private new BoxCollider collider;

    private void Start()
    {
        node = GetComponent<Node>();
        collider = GetComponent<BoxCollider>();
        townUpgrade = GetComponent<TownUpgrade>();

        ShowGround();
        if (!node.isWalkable)
        {
            collider.size += new Vector3(2, 0, 2);
            ShowObstacle();
        }
    }

    public void ShowGround()
    {
        GameObject selectedVisual = SelectVisual(GroundVisuals);
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(true);
            transform.eulerAngles = Vector3.up * Random.Range(0, 4) * 90;
        }
    }

    public void ShowObstacle()
    {
        GameObject selectedVisual = SelectVisual(obstacleVisuals);
        if (selectedVisual != null)
        {
            // Water Tile
            if (selectedVisual == obstacleVisuals[0].visual)
            {
                HideAllGround();
                if (TryGetComponent(out TileInteract tileInteract))
                {
                    tileInteract.enabled = false;
                }
            }
            selectedVisual.SetActive(true);
            transform.eulerAngles = Vector3.up * Random.Range(0, 4) * 90;

            // Grass Tile
            if (selectedVisual == obstacleVisuals[2].visual)
            {
                townUpgrade.canUpgrade = true;
                townUpgrade.currentUpgradeLevel = 2;
            }
            // House Tile
            else if (selectedVisual == obstacleVisuals[3].visual)
            {
                townUpgrade.canUpgrade = true;
                townUpgrade.currentUpgradeLevel = 3;
            }
            // // Town
            // else if (selectedVisual == obstacleVisuals[4].visual)
            // {
            //     townUpgrade.currentUpgradeLevel = 4;
            // }
        }
    }

    private GameObject SelectVisual(List<VisualProbability> visuals)
    {
        // Filter out visuals with 0 probability
        var validVisuals = visuals.FindAll(v => v.probability > 0);
        if (validVisuals.Count == 0) return null;

        // Calculate total probability
        float totalProbability = 0f;
        foreach (var visual in validVisuals)
        {
            totalProbability += visual.probability;
        }

        // Generate random point within total probability range
        float randomPoint = Random.value * totalProbability;
        float cumulativeProbability = 0f;

        // Find which visual the random point lands on
        foreach (var visual in validVisuals)
        {
            cumulativeProbability += visual.probability;
            if (randomPoint <= cumulativeProbability)
            {
                return visual.visual;
            }
        }

        // Fallback (should rarely happen due to floating point precision)
        return validVisuals[validVisuals.Count - 1].visual;
    }

    private void HideAllGround()
    {
        foreach (var visual in GroundVisuals)
        {
            visual.visual.SetActive(false);
        }
    }

    public void HideAllObstacles()
    {
        SetActiveState(obstacleVisuals, false);
    }

    private void SetActiveState(List<VisualProbability> objects, bool state)
    {
        foreach (var obj in objects)
        {
            obj.visual.SetActive(state);
        }
    }
}
