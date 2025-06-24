using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // Event for when ownership changes
    public UnityEvent<string> OnOwnershipChanged;

    private void Start()
    {
        tileRenderers = GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in tileRenderers)
        {
            renderer.material = defaultMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PackageMover>(out var package))
        {
            if (package.targetPerson.CompareTag("PersonB"))
            {
                if (currentOwner != Owner.PersonA)
                {
                    ChangeOwnership(Owner.PersonA);
                }
            }
            else if (package.targetPerson.CompareTag("PersonA"))
            {
                if (currentOwner != Owner.PersonB)
                {
                    ChangeOwnership(Owner.PersonB);
                }
            }
        }
        else if (other.CompareTag("PersonA"))
        {
            if (currentOwner != Owner.PersonA)
            {
                ChangeOwnership(Owner.PersonA);
            }
        }
        else if (other.CompareTag("PersonB"))
        {
            if (currentOwner != Owner.PersonB)
            {
                ChangeOwnership(Owner.PersonB);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PersonA") && currentOwner == Owner.PersonA)
        {
            ChangeOwnership(Owner.None);
        }
        else if (other.CompareTag("PersonB") && currentOwner == Owner.PersonB)
        {
            ChangeOwnership(Owner.None);
        }
    }

    private void ChangeOwnership(Owner newOwner)
    {
        // Decrement previous owner's tile count
        if (currentOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.DecrementTileA();
        }
        else if (currentOwner == Owner.PersonB)
        {
            PlayerDataManager.Instance.DecrementTileB();
        }

        // Increment new owner's tile count
        if (newOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.IncrementTileA();
        }
        else if (newOwner == Owner.PersonB)
        {
            PlayerDataManager.Instance.IncrementTileB();
        }

        currentOwner = newOwner;
        UpdateTileMaterial();

        // Notify listeners about ownership change
        OnOwnershipChanged?.Invoke(currentOwner.ToString());
    }

    public void SetOwner(string ownerTag)
    {
        Owner newOwner = ownerTag == "PersonA" ? Owner.PersonA : Owner.PersonB;

        if (currentOwner != newOwner)
        {
            ChangeOwnership(newOwner);
        }
    }

    private void UpdateTileMaterial()
    {
        foreach (var renderer in tileRenderers)
        {
            switch (currentOwner)
            {
                case Owner.PersonA:
                    renderer.material = personAMaterial;
                    break;
                case Owner.PersonB:
                    renderer.material = personBMaterial;
                    break;
                default:
                    renderer.material = defaultMaterial;
                    break;
            }
        }
    }

    // Public method to get current owner (for external scripts)
    public string GetCurrentOwner()
    {
        return currentOwner.ToString();
    }
}