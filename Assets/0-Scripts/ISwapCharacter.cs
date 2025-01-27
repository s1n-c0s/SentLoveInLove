using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ISwapCharacter : MonoBehaviour
{
    public bool _isPersonA;
    [SerializeField] private IComponent[] _SelectCharacter;

    [System.Serializable]
    public class IComponent
    {
        public Sprite characterSprite;
        public Sprite iconSprite;
        public Sprite boxSprite;
        public bool flipX;

        public IComponent(Sprite characterSprite, Sprite iconSprite, Sprite boxSprite, bool flipX = false)
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

    public int _characterIndex = 0;

    private void Start()
    {
        // Set the first character sprites
        if (_SelectCharacter.Length > 0)
        {
            SetSprites(_SelectCharacter[_characterIndex]);
        }

        if (gameObject.CompareTag("PersonA"))
        {
            _isPersonA = true;
        }
        else if (gameObject.CompareTag("PersonB"))
        {
            _isPersonA = false;
        }
    }

    public void ToggleCharacter()
    {
        if (_SelectCharacter == null || _SelectCharacter.Length == 0)
        {
            Debug.LogError("No components assigned.");
            return;
        }

        // Move to next character
        _characterIndex = (_characterIndex + 1) % _SelectCharacter.Length;

        // Set the sprites for the next character
        SetSprites(_SelectCharacter[_characterIndex]);
    }

    private void SetSprites(IComponent component)
    {
        characterImage.sprite = component.characterSprite;
        iconImage.sprite = component.iconSprite;
        boxImage.sprite = component.boxSprite;

        // Apply random rotation within ±5 degrees to characterImage only
        float randomRotation = Random.Range(-5f, 5f);
        characterImage.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        // Reset rotation for boxImage
        boxImage.transform.rotation = Quaternion.identity;

        // Apply flip settings while maintaining the original size set in the inspector
        Vector3 originalCharacterScale = characterImage.rectTransform.localScale;
        characterImage.rectTransform.localScale = new Vector3(component.flipX ? -Mathf.Abs(originalCharacterScale.x) : Mathf.Abs(originalCharacterScale.x), originalCharacterScale.y, originalCharacterScale.z);

        Vector3 originalIconScale = iconImage.rectTransform.localScale;
        iconImage.rectTransform.localScale = new Vector3(component.flipX ? -Mathf.Abs(originalIconScale.x) : Mathf.Abs(originalIconScale.x), originalIconScale.y, originalIconScale.z);
    }

    // Method to set the _SelectCharacter array
    public void SetComponents(IComponent[] components)
    {
        _SelectCharacter = components;

        // Set the first character sprites
        if (_SelectCharacter.Length > 0)
        {
            SetSprites(_SelectCharacter[_characterIndex]);
        }
    }
}
