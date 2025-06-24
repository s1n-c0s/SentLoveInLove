using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Lean.Pool;

public class BombCapture : MonoBehaviour
{
    [SerializeField] private float captureRadius = 10f; // Increased radius
    [SerializeField] private LayerMask tileLayer; // Set to detect only TileCapture objects
    [SerializeField] private GameObject explosionEffect;
    
    // Pre-allocated array for non-allocating overlap sphere
    private static readonly Collider[] hitBuffer = new Collider[256]; // Adjust size based on max expected tiles
    
    // Cache for string comparisons
    private const string PersonATag = "PersonA";
    private const string PersonBTag = "PersonB";
    
    // Audio source name cached
    private const string BombSoundName = "BombLove";

    private void OnTriggerEnter(Collider other)
    {
        // Early exit with cached component lookup
        if (!other.TryGetComponent<PackageMover>(out var package)) 
            return;

        HandleBombExplosion(package);
    }

    private void HandleBombExplosion(PackageMover package)
    {
        // Spawn explosion effect
        var fx = LeanPool.Spawn(explosionEffect, transform.position, Quaternion.identity);
        LeanPool.Despawn(fx, 2f);
        
        // Optimized owner tag determination
        var ownerTag = package.targetPerson.CompareTag(PersonATag) ? PersonBTag : PersonATag;
        
        // Capture tiles with optimized method
        CaptureTilesAround(transform.position, ownerTag);
        
        // Play sound effect
        SoundFX.Instance.PlaySound(BombSoundName);
        
        // Clean up bomb object
        LeanPool.Despawn(gameObject);
    }

    private void CaptureTilesAround(Vector3 position, string ownerTag)
    {
        // Use non-allocating OverlapSphereNonAlloc for better performance
        int hitCount = Physics.OverlapSphereNonAlloc(position, captureRadius, hitBuffer, tileLayer);
        
        // Process only the actual hits
        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            
            // Null check for safety (shouldn't be needed but good practice)
            if (hit != null && hit.TryGetComponent<TileCapture>(out var tile))
            {
                tile.SetOwner(ownerTag);
            }
        }
        
        // Clear references for GC (optional, but good practice for large arrays)
        if (hitCount > 0)
        {
            System.Array.Clear(hitBuffer, 0, hitCount);
        }
    }

    // Optional: Visualize the capture radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, captureRadius);
    }
}