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
    [SerializeField] private Node node;

    private void Start()
    {
        node = GetComponent<Node>();

        ShowGround();
        if (!node.isWalkable)
        {
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
        }
    }

    private GameObject SelectVisual(List<VisualProbability> visuals)
    {
        float totalProbability = 0f;
        foreach (var visual in visuals)
        {
            totalProbability += visual.probability;
        }

        float randomPoint = Random.value * totalProbability;

        foreach (var visual in visuals)
        {
            if (randomPoint < visual.probability)
            {
                return visual.visual;
            }
            else
            {
                randomPoint -= visual.probability;
            }
        }

        return null;
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
