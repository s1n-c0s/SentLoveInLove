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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PackageMover>(out var package))
        {
            if (package.targetPerson.CompareTag("PersonB")) // -> Person A
            {
                if (currentOwner != Owner.PersonA)
                {
                    if (currentOwner == Owner.PersonB)
                    {
                        PlayerDataManager.Instance.DecrementTileB();
                    }
                    PlayerDataManager.Instance.IncrementTileA();
                    currentOwner = Owner.PersonA;
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
            }
        }
    }
}
