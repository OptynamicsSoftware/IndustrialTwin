using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPositioner : MonoBehaviour
{
    Canvas m_oStaticInfoCanvas;
    public RectTransform m_oRectTransform;
    public GameObject m_oLinkedObject;
    public float m_fOffset;
    public float m_fMaxDistance;
    void Start()
    {
        m_oStaticInfoCanvas = GameObject.Find("CanvasIcons").GetComponent<Canvas>();
    }

    void Update()
    {
        float size = CalculateIconSize();
        transform.localScale = Vector3.one * size;
        m_oRectTransform.anchoredPosition = CanvasPositioningExtensions.WorldToCanvasPosition(m_oStaticInfoCanvas, m_oLinkedObject.transform.position + Vector3.up * m_fOffset * (1-size*0.25f));
    }

    float CalculateIconSize()
    {
        float size = m_fMaxDistance - Vector3.Distance(Camera.main.transform.position, m_oLinkedObject.transform.position);
        if (size / m_fMaxDistance <= 0.1f)
            return 0.1f;
        else
            return size / m_fMaxDistance;
    }
}
