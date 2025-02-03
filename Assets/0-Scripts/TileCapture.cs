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
    [SerializeField] private Renderer tileRenderer;

    private void Start()
    {
        // tileRenderer = GetComponentInChildren<Renderer>();
        if (tileRenderer != null)
        {
            tileRenderer.material = defaultMaterial;
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
        if (tileRenderer != null)
        {
            switch (currentOwner)
            {
                case Owner.PersonA:
                    tileRenderer.material = personAMaterial;
                    break;
                case Owner.PersonB:
                    tileRenderer.material = personBMaterial;
                    break;
                default:
                    tileRenderer.material = defaultMaterial;
                    break;
            }
        }
    }
}
