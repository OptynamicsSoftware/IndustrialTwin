using UnityEngine;

public class UI_CameraMovementBlocker : MonoBehaviour
{
    public CameraController m_oCameraController;
    public RectTransform m_oRectTransform;
    public bool m_bBlocked;
    public bool m_bEnabled;

    private void OnEnable()
    {
        m_bEnabled = true;
    }

    private void OnDisable()
    {
        m_bEnabled = false;
    }

    private void Update()
    {
        if (ExtensionMethods.PointInsideRect(Input.mousePosition, m_oRectTransform))
        {
            m_bBlocked = true;
        }
        else
        {
            m_bBlocked = false;
        }
    }
}
