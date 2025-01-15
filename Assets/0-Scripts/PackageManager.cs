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
        GameObject package = _package[Random.Range(0, _package.Count)];
        if (gameObject.CompareTag("PersonA"))
        {
            package.GetComponent<PackageMover>().targetPerson = GameObject.FindGameObjectWithTag("PersonB");
        }
        else if (gameObject.CompareTag("PersonB"))
        {
            package.GetComponent<PackageMover>().targetPerson = GameObject.FindGameObjectWithTag("PersonA");
        }
        // Use LeanPool to spawn the package
        LeanPool.Spawn(package, position, Quaternion.identity);

        Debug.Log($"Package spawned at {position}");
    }
}
