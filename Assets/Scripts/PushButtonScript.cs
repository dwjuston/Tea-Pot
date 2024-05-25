using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class PushButtonScript : MonoBehaviour
{
    public UnityEvent onPressed = new UnityEvent();
    public UnityEvent onReset = new UnityEvent();

    public UnityEvent onInteractionStart = new UnityEvent();
    public UnityEvent onInteractionEnd = new UnityEvent();

    [Min(0.01f)]
    public float depressionDepth = 0.015f;

    [Min(0.01f)]
    public float returnSpeed = 1.0f;

    float m_currentPressedDepth = 0.0f;
    float m_yMax = 0.0f; // resting position
    float m_yMin = 0.0f; // pressed position
    bool m_wasPressed = false;

    List<Collider> m_currentColliders = new List<Collider>();

    XRBaseInteractor m_interactor = null;

    // Utility Functions
    void SetHeight(float newHeight)
    {
        // Clamp the new height to the min and max values
        float x = transform.localPosition.x;
        float y = Mathf.Clamp(newHeight, m_yMin, m_yMax);
        float z = transform.localPosition.z;
        transform.localPosition = new Vector3(x, y, z);
    }

    bool ApproximatelyEqual(float val, float other, float epsilon=0.001f)
    {
        return val >= other - epsilon && val <= other + epsilon;
    }

    bool IsPressed()
    {
        float currentHeight = transform.localPosition.y;
        return ApproximatelyEqual(currentHeight, m_yMin);
    }
    bool IsReset()
    {
        float currentHeight = transform.localPosition.y;
        return ApproximatelyEqual(currentHeight, m_yMax);
    }

    float GetPressDepth(Vector3 interactorWorldPosition)
    {
        Vector3 relativePosition = this.transform.parent.InverseTransformPoint(interactorWorldPosition);
        return relativePosition.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_yMax = transform.localPosition.y;
        m_yMin = m_yMax - depressionDepth;
    }

    // Update is called once per frame
    void Update()
    {
        // we are currently being pressed
        if(m_interactor != null)
        {
            float newPressHeight = GetPressDepth(m_interactor.transform.position);
            float deltaHeight = m_currentPressedDepth - newPressHeight; 
            float newPressedPosition = transform.localPosition.y - deltaHeight;
            SetHeight(newPressedPosition);

            // If we were not pressed and now we are, and it has been pressed to the max depth
            if (!m_wasPressed && IsPressed())
            {
                // We pressed the button
                onPressed?.Invoke();
                m_wasPressed = true;

            }

            m_currentPressedDepth = newPressHeight;
        }
        // not being pressed
        else
        {
            // If we are not at the resting position, move back to it
            if(!Mathf.Approximately(transform.localPosition.y, m_yMax))
            {
                // Naturally push back to the resting position
                float returnHeight = Mathf.MoveTowards(transform.localPosition.y, m_yMax, Time.deltaTime * returnSpeed);
                SetHeight(returnHeight);
            }
        }

        // If we were pressed and now we are not, and we are the reset yet, reset the button
        if(m_wasPressed && IsReset())
        {
            onReset?.Invoke();
            m_wasPressed = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        // Cololider with interactor, and it is not a trigger - it is a hand
        if(interactor != null && !other.isTrigger)
        {
            m_currentColliders.Add(other); // one hand may have multiple colliders
            if(m_interactor == null) // if we are not already being pressed
            {
                m_interactor = interactor;
                m_currentPressedDepth = GetPressDepth(m_interactor.transform.position);
                onInteractionStart?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // We want to wait until all colliders have exited
        if(m_currentColliders.Contains(other))
        {
            m_currentColliders.Remove(other);
            if(m_currentColliders.Count == 0)
            {
                onInteractionEnd?.Invoke();
                m_currentPressedDepth = 0.0f;
                m_interactor = null;
            }
        }
    }


}
