using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacleVisuals;
    [SerializeField] private Node node;

    private void Start()
    {
        node = GetComponent<Node>();

        if (!node.isWalkable)
        {
            ShowObstacle();
        }
    }

    public void ShowObstacle()
    {
        int randomIndex = Random.Range(0, obstacleVisuals.Count);
        obstacleVisuals[randomIndex].SetActive(true);

        int rotateDegree = Random.Range(0, 4) * 90;
        transform.eulerAngles = new Vector3(0, rotateDegree, 0);
    }

    public void HideAllObstacles()
    {
        foreach (GameObject obstacle in obstacleVisuals)
        {
            obstacle.SetActive(false);
        }
    }
}


