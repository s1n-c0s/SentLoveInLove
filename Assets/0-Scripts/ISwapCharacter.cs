using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ISwapCharacter : MonoBehaviour
{
    public bool isPersonA;
    [SerializeField] private CharacterComponent[] characterComponents;

    [System.Serializable]
    public class CharacterComponent
    {
        public Sprite characterSprite;
        public Sprite iconSprite;
        public Sprite boxSprite;
        public bool flipX;

        public CharacterComponent(Sprite characterSprite, Sprite iconSprite, Sprite boxSprite, bool flipX = false)
        {
            this.characterSprite = characterSprite;
            this.iconSprite = iconSprite;
            this.boxSprite = boxSprite;
            this.flipX = flipX;
        }
    }

    [SerializeField] private Image characterImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image boxImage;

    private int characterIndex = 0;

    private void Start()
    {
        if (isPersonA)
        {
            characterIndex = PlayerDataManager.Instance.playerData.SelectCharacterA;
        }
        else
        {
            characterIndex = PlayerDataManager.Instance.playerData.SelectCharacterB;
        }

        if (characterComponents.Length > 0)
        {
            SetSprites(characterComponents[characterIndex]);
        }
    }

    public void ToggleCharacter()
    {
        if (characterComponents == null || characterComponents.Length == 0)
        {
            Debug.LogError("No components assigned.");
            return;
        }

        characterIndex = (characterIndex + 1) % characterComponents.Length;

        SetSprites(characterComponents[characterIndex]);
        SwapCharacter(characterIndex);
    }

    private void SetSprites(CharacterComponent component)
    {
        if (component.characterSprite != null)
        {
            characterImage.sprite = component.characterSprite;

            float randomRotation = Random.Range(-5f, 5f);
            characterImage.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
            ApplyFlipSettings(characterImage.rectTransform, component.flipX);
        }

        if (component.iconSprite != null)
        {
            iconImage.sprite = component.iconSprite;
            ApplyFlipSettings(iconImage.rectTransform, component.flipX);
        }

        if (component.boxSprite != null && boxImage != null)
        {
            boxImage.sprite = component.boxSprite;
            boxImage.transform.rotation = Quaternion.identity;
        }
    }

    private void ApplyFlipSettings(RectTransform rectTransform, bool flipX)
    {
        Vector3 originalScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(flipX ? -Mathf.Abs(originalScale.x) : Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void SetComponents(CharacterComponent[] components)
    {
        characterComponents = components;

        if (characterComponents.Length > 0)
        {
            SetSprites(characterComponents[characterIndex]);
        }
    }

    public void SwapCharacter(int index)
    {
        if (isPersonA)
        {
            PlayerDataManager.Instance.UpdateSelectCharacterA(index);
        }
        else
        {
            PlayerDataManager.Instance.UpdateSelectCharacterB(index);
        }

        UpdateVisuals(index);
    }

    private void UpdateVisuals(int index)
    {
        characterImage.sprite = characterComponents[index].characterSprite;
        iconImage.sprite = characterComponents[index].iconSprite;
    }
}
