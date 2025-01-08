using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteract : MonoBehaviour
{
    public float hoverHeight = 0.1f;
    public float returnSpeed = 2.0f;

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHovered())
        {
            transform.position = originalPosition + Vector3.up * hoverHeight;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }

    bool IsHovered()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) // Corrected this line
        {
            return hit.transform == transform && hit.distance > hoverHeight;
        }

        return false;
    }
}
