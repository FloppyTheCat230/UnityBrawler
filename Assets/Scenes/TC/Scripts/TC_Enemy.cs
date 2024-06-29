using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_Enemy : MonoBehaviour
{
    public SkinnedMeshRenderer bodyMeshRenderer;

    [SerializeField]
    private Color targetedColor;
    private Color originalColor;

    private void Awake()
    {
        originalColor = bodyMeshRenderer.material.color;
    }

    public void ToggleHighlight(bool choice)
    {
        bodyMeshRenderer.material.color = choice ? targetedColor : originalColor;
    }
}
