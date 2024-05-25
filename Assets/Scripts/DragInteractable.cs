using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class DragEvent : UnityEvent<float> { }


public class DragInteractable : XRBaseInteractable
{
    public Transform startDragPosition = null;
    public Transform endDragPosition = null;

    [HideInInspector]
    public float dragPercentage = 0.0f; // betwwen 0 and 1

    protected XRBaseInteractor m_interactor = null;

    public UnityEvent onDragStart = new UnityEvent();
    public UnityEvent onDragEnd = new UnityEvent();
    public DragEvent onDragUpdate = new DragEvent();


    Coroutine m_drag = null;

    void StartDrag()
    {
        if (m_drag != null)
        {
            StopCoroutine(m_drag);
        }
        m_drag = StartCoroutine(CalculateDrag());
        onDragStart?.Invoke();
    }

    void EndDrag()
    {
        if(m_drag != null)
        {
            StopCoroutine(m_drag);
            m_drag = null;
            onDragEnd?.Invoke();
        }
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        // the dot product of AV and AB is the projection of AV onto AB
        return Mathf.Clamp01(Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB));
    }

    IEnumerator CalculateDrag()
    {

        // given a line between startDragPosition and endDragPosition, project the interactor position onto the line, find the percentage of the line that the interactor is at
        while(m_interactor != null)
        {
            // get a line local space
            Vector3 line = endDragPosition.localPosition - startDragPosition.localPosition;

            // get the interactor position in local space
            Vector3 interactorPosition = startDragPosition.parent.InverseTransformPoint(m_interactor.transform.position);

            // project the interactor position onto the line
            Vector3 projectedPosition = Vector3.Project(interactorPosition, line.normalized);

            // get the percentage of the line that the interactor is at
            dragPercentage = InverseLerp(startDragPosition.localPosition, endDragPosition.localPosition, projectedPosition);

            onDragUpdate?.Invoke(dragPercentage);

            yield return null;
        }
        
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        m_interactor = interactor;
        StartDrag();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        EndDrag();
        m_interactor = null;
    }
}
