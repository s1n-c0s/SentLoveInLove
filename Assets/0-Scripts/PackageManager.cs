using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PackageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Package = new List<GameObject>();

    public void SpawnPackageAt(Vector3 position)
    {
        if (_Package.Count == 0) return;

        // Randomly pick a package from the list
        GameObject package = _Package[Random.Range(0, _Package.Count)];
        LeanPool.Spawn(package, position, Quaternion.identity);
        Debug.Log($"{package.name} spawned at {position}");
    }
}
