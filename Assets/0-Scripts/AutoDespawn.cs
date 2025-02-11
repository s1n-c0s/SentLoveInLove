using UnityEngine;
using Lean.Pool; // Import Lean Pool

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] private GameObject DespawnFX;
    [SerializeField] private float lifetime = 6f;
    [SerializeField] private float randomVariation = 0f; // Allows random variation (e.g., ±0.5s)   

    private void OnEnable()
    {
        float finalLifetime = lifetime + Random.Range(-randomVariation, randomVariation);
        Invoke(nameof(Despawn), Mathf.Max(0.1f, finalLifetime)); // Prevents negative lifetime
    }

    private void Despawn()
    {
        SoundFX.Instance.PlaySound("Despawn");
        Vector3 position = transform.position;
        GameObject fx = LeanPool.Spawn(DespawnFX, position, transform.rotation);
        LeanPool.Despawn(fx, 2f); // Despawn after 1 second
        LeanPool.Despawn(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Despawn)); // Prevents lingering invoke calls
    }
}
