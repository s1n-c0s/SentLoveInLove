using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _package = new List<GameObject>();

    // Spawn a package at a specific position
    public void SpawnPackageAround(Vector3 position)
    {
        if (_package.Count == 0) return;

        // Randomly pick a package from the list
        GameObject packagePrefab = _package[Random.Range(0, _package.Count)];

        // Spawn the package using Lean Pool
        GameObject spawnedPackage = LeanPool.Spawn(packagePrefab, position, Quaternion.identity);

        // Set the target for the PackageMover component
        PackageMover packageMover = spawnedPackage.GetComponent<PackageMover>();
        if (packageMover != null)
        {
            if (gameObject.CompareTag("PersonA"))
            {
                packageMover.Initialize(GameObject.FindGameObjectWithTag("PersonB"));
            }
            else if (gameObject.CompareTag("PersonB"))
            {
                packageMover.Initialize(GameObject.FindGameObjectWithTag("PersonA"));
            }
        }

        Debug.Log($"Package spawned at {position}");
    }
}
