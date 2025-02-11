using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFXPlayer : MonoBehaviour
{
    [SerializeField] private List<GameObject> endFX;

    public void PlayAreaFX()
    {
        endFX[0].SetActive(true);
    }

    public void PlayCrownFX()
    {
        SoundFX.Instance.PlaySound("Crown");
        endFX[1].SetActive(true);
    }

    public void DisableCrownFX()
    {
        endFX[1].SetActive(false); // CrownFX
    }


    public void PlayHeartFX()
    {
        endFX[2].SetActive(true);
    }
}
