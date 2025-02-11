using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Lean.Pool;

public class BombCapture : MonoBehaviour
{
    [SerializeField] private float captureRadius = 1.5f; // Adjust based on grid size
    [SerializeField] private LayerMask tileLayer; // Set to detect only TileCapture objects
    [SerializeField] private GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PackageMover>(out var package))
        {
            GameObject fx = LeanPool.Spawn(explosionEffect, transform.position, Quaternion.identity);
            LeanPool.Despawn(fx, 2f); // Despawn after 2 seconds
            // Tween.ShakeCamera(Camera.main, 0.5f, 0.1f); // Shake camera on bomb activation
            string ownerTag = package.targetPerson.CompareTag("PersonA") ? "PersonB" : "PersonA";
            CaptureTilesAround(transform.position, ownerTag);
            LeanPool.Despawn(gameObject);
        }
    }

    private void CaptureTilesAround(Vector3 position, string ownerTag)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, captureRadius, tileLayer);

        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<TileCapture>(out var tile))
            {
                tile.SetOwner(ownerTag);
            }
        }
    }
}
