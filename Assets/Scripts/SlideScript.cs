using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideScript : MonoBehaviour
{
    public Transform startPosition = null;
    public Transform endPosition = null;
    public GameObject targetObject = null;

    MeshRenderer meshRenderer = null;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnSlideStrat()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void OnSlideStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void UpdateSlider(float percentage)
    {
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, percentage);

        // Change opacity of target object and all its child components
        if (targetObject != null)
        {
            ChangeOpacity(targetObject, percentage);
        }
    }

    private void ChangeOpacity(GameObject target, float percentage)
    {
        // change child components' Canvas Group alpha
        CanvasGroup[] canvasGroups = target.GetComponentsInChildren<CanvasGroup>();
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = 1 - percentage;
        }
    }
}
