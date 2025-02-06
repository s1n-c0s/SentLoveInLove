using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DetectTargets : MonoBehaviour
{
    private List<GameObject> targetGroup;
    public CinemachineTargetGroup cinemachineTargetGroup;

    // Start is called before the first frame update
    void Start()
    {
        targetGroup = new List<GameObject>();
        AddToTargetGroup("PersonA");
        AddToTargetGroup("PersonB");
    }

    void AddToTargetGroup(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in taggedObjects)
        {
            targetGroup.Add(obj);
            CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target
            {
                target = obj.transform,
                weight = 1f,
                radius = 1f
            };
            cinemachineTargetGroup.AddMember(target.target, target.weight, target.radius);
        }
    }
}
