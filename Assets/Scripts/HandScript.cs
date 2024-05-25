using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public enum HandType
{
    Left,
    Right
}

public class HandScript : MonoBehaviour
{
    public HandType type = HandType.Left;
    public bool isHidden { get; private set; } = false;

    public InputAction trackedAction = null;

    private bool m_isCurrentlyTracked = false;

    List<Renderer> m_currentRenderers = new List<Renderer>();

    Collider[] m_colliders = null;

    public bool isCollisionEnabled { get; private set; } = false;

    public XRBaseInteractor interactor = null;

    private void Awake()
    {
        if (interactor == null)
        {
            interactor = GetComponentInParent<XRBaseInteractor>();
        }
    }

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);

    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnGrab);
        interactor.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs arg0)
    {
        
        HandControlScript ctrl = arg0.interactable.GetComponent<HandControlScript>();
        if (ctrl != null)
        {
            if (ctrl.hideHand)
            {
                Show();
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        HandControlScript ctrl = arg0.interactable.GetComponent<HandControlScript>();
        if (ctrl != null)
        {
            if (ctrl.hideHand)
            {
                Hide();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(x => !x.isTrigger).ToArray();
        trackedAction.Enable();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        float isTracked = trackedAction.ReadValue<float>();
        if(isTracked == 1.0f && !m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = true;
            Show();
        }
        else if (isTracked == 0 && m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = false;
            Hide();
        }
        
    }

    public void Show()
    {
        foreach(Renderer renderer in m_currentRenderers)
        {
            renderer.enabled = true;
        }
        isHidden = false;
        EnableCollisions(true);
    }

    public void Hide()
    {
        m_currentRenderers.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
            m_currentRenderers.Add(renderer);
        }
        isHidden = true;
        EnableCollisions(false);
    }

    public void EnableCollisions(bool enabled)
    {
        if(isCollisionEnabled == enabled)
        {
            return;
        }

        isCollisionEnabled = enabled;
        foreach (Collider collider in m_colliders)
        {
            collider.enabled = isCollisionEnabled;
        }
    }
}
