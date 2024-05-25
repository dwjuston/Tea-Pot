using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButtonColorScript : MonoBehaviour
{
    MeshRenderer m_meshRendered = null;

    private void Awake()
    {
        m_meshRendered = GetComponent<MeshRenderer>();
    }

    public void ChangeColor()
    {
        m_meshRendered.material.SetColor("_Color", Random.ColorHSV());
    }

}
