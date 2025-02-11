using UnityEngine;

public class TileCapture : MonoBehaviour
{
    private enum Owner
    {
        None,
        PersonA,
        PersonB
    }

    [SerializeField] private Owner currentOwner = Owner.None;
    [SerializeField] private Material personAMaterial;
    [SerializeField] private Material personBMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Renderer[] tileRenderers;

    private void Start()
    {
        tileRenderers = GetComponentsInChildren<Renderer>(true);
        // UpdateTileMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PackageMover>(out var package))
        {
            SetOwner(package.targetPerson.CompareTag("PersonA") ? "PersonA" : "PersonB");
        }
        else if (other.CompareTag("PersonA") || other.CompareTag("PersonB"))
        {
            SetOwner(other.tag);
        }
    }

    public void SetOwner(string ownerTag)
    {
        if (ownerTag == "PersonA" && currentOwner != Owner.PersonA)
        {
            UpdateOwnership(Owner.PersonA, Owner.PersonB, personAMaterial);
        }
        else if (ownerTag == "PersonB" && currentOwner != Owner.PersonB)
        {
            UpdateOwnership(Owner.PersonB, Owner.PersonA, personBMaterial);
        }
    }

    private void UpdateOwnership(Owner newOwner, Owner previousOwner, Material newMaterial)
    {
        // Only decrement if the current owner is different from the previous owner
        if (previousOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.DecrementTileA();
        }
        else if (previousOwner == Owner.PersonB)
        {
            PlayerDataManager.Instance.DecrementTileB();
        }

        // Increment the new owner's tile count
        if (newOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.IncrementTileA();
        }
        else if (newOwner == Owner.PersonB)
        {
            PlayerDataManager.Instance.IncrementTileB();
        }

        currentOwner = newOwner;
        ApplyMaterial(newMaterial);
    }

    private void ApplyMaterial(Material material)
    {
        foreach (var renderer in tileRenderers)
        {
            renderer.material = material;
        }
    }

    private void UpdateTileMaterial()
    {
        switch (currentOwner)
        {
            case Owner.PersonA:
                ApplyMaterial(personAMaterial);
                break;
            case Owner.PersonB:
                ApplyMaterial(personBMaterial);
                break;
            default:
                ApplyMaterial(defaultMaterial);
                break;
        }
    }
}
