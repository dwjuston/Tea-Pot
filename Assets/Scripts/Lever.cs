using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform startOrientation = null;
    public Transform endOrientation = null;
    public GameObject targetObject = null;

    MeshRenderer meshRenderer = null;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void OnLeverStart()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void OnLeverStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void UpdateLever(float percentage)
    {
        transform.rotation = Quaternion.Slerp(startOrientation.rotation, endOrientation.rotation, percentage);

        MoveIt(targetObject, percentage);

    }

    private void MoveIt(GameObject target, float percentage)
    {
        int y_min = 6;
        int y_max = 10;
        // change the y position of the target object
        target.transform.position = new Vector3(target.transform.position.x, Mathf.Lerp(y_min, y_max, percentage), target.transform.position.z);

        
    }
}
