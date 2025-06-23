using System.Collections;
using System.Collections.Generic;
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
                    if (currentOwner == Owner.PersonB)
                    {
                        PlayerDataManager.Instance.DecrementTileB();
                    }
                    PlayerDataManager.Instance.IncrementTileA();
                    currentOwner = Owner.PersonA;
                    UpdateTileMaterial();
                }
            }
            else if (package.targetPerson.CompareTag("PersonA"))
            {
                if (currentOwner != Owner.PersonB)
                {
                    if (currentOwner == Owner.PersonA)
                    {
                        PlayerDataManager.Instance.DecrementTileA();
                    }
                    PlayerDataManager.Instance.IncrementTileB();
                    currentOwner = Owner.PersonB;
                    UpdateTileMaterial();
                }
            }
        }
        else if (other.CompareTag("PersonA"))
        {
            if (currentOwner != Owner.PersonA)
            {
                if (currentOwner == Owner.PersonB)
                {
                    PlayerDataManager.Instance.DecrementTileB();
                }
                PlayerDataManager.Instance.IncrementTileA();
                currentOwner = Owner.PersonA;
                UpdateTileMaterial();
            }
        }
        else if (other.CompareTag("PersonB"))
        {
            if (currentOwner != Owner.PersonB)
            {
                if (currentOwner == Owner.PersonA)
                {
                    PlayerDataManager.Instance.DecrementTileA();
                }
                PlayerDataManager.Instance.IncrementTileB();
                currentOwner = Owner.PersonB;
                UpdateTileMaterial();
            }
        }
    }

    public void SetOwner(string ownerTag)
    {
        Owner newOwner = ownerTag == "PersonA" ? Owner.PersonA : Owner.PersonB;

        if (currentOwner == newOwner) return; // Prevent unnecessary updates

        // Decrement previous owner's tile count if the tile was owned
        if (currentOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.DecrementTileA();
        }
        else if (currentOwner == Owner.PersonB)
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
        UpdateTileMaterial();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PersonA") && currentOwner == Owner.PersonA)
        {
            PlayerDataManager.Instance.DecrementTileA();
            currentOwner = Owner.None;
            UpdateTileMaterial();
        }
        else if (other.CompareTag("PersonB") && currentOwner == Owner.PersonB)
        {
            PlayerDataManager.Instance.DecrementTileB();
            currentOwner = Owner.None;
            UpdateTileMaterial();
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
}

