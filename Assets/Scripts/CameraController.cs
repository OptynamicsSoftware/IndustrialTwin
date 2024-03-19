using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using TMPro;
using System;

public class CameraController : MonoBehaviour
{
    public Transform m_oCameraTransform;
    static private Camera m_oCamera;
    public TextMeshProUGUI[] m_tPerspectiveCameraTexts, m_tOrtogonalCameraTexts;
    Vector3 m_vTargetPos;
    Vector3 m_vSecondaryTargetPos;
    public GameObject m_oReference;
    public float m_fMinZoom, m_fMaxZoom;
    public float m_fMinxRot, m_fMaxxRot;
    public float m_fXRot, m_fYRot, m_fZPos;
    float m_fXPan, m_fYPan, m_fXPanK, m_fYPanK;
    public float m_fMoveSpeed;
    public bool m_bControlEnabled, m_bIsRotating, m_bIsMousePanning;
    public List<float> m_tZoomLevels, m_tLODZoomValues;
    public List<int> m_tLODLayerIndexes = new List<int>();
    private int m_fZoomLevel = 0;
    public float m_fShadowLODZoomValue;
    public Light m_oDirectionalLight;
    InputController m_oInputController;
    private List<UI_CameraMovementBlocker> m_tBlockers = new List<UI_CameraMovementBlocker>();

    void Start()
    {
        m_oInputController = FindObjectOfType<InputController>();
        m_oCamera = GetComponentInChildren<Camera>();
        m_oCamera.transform.LookAt(transform.position);
        m_vTargetPos = transform.position;
        m_fXRot = transform.localEulerAngles.x;
        m_fYRot = transform.localEulerAngles.y;
        m_fZPos = m_oCamera.transform.localPosition.z;


        m_oInputController.m_oRightMouseButtonHold += Orbit;
        m_oInputController.m_oRightMouseButtonUp += () => m_bIsRotating = false;
        m_oInputController.m_oLeftMouseButtonPressed += SetMousePanning;
        m_oInputController.m_oLeftMouseButtonHold += Panning;
        m_oInputController.m_oLeftMouseButtonUp += ResetMousePanning;
        m_oInputController.m_oMouseDeltaX += UpdateMouseXDelta;
        m_oInputController.m_oMouseDeltaY += UpdateMouseYDelta;
        m_oInputController.m_oHorizontalAxis += UpdateKeyboardXDelta;
        m_oInputController.m_oVerticalAxis += UpdateKeyboardYDelta;
        m_oInputController.m_oTouchPinch += DoZoom;
        m_oInputController.m_oMouseScroll += DoZoom;
        //m_oInputController.m_oDoubleClick += MovementAndZoomOnPoint;
        //m_oInputController.m_oDoubleRightClick += MovementAndZoomOutOnPoint;

        m_oReference.transform.localEulerAngles = new Vector3(-m_fXRot, 0, 0);

        foreach (UI_CameraMovementBlocker oBlocker in FindObjectsOfType<UI_CameraMovementBlocker>(true))
        {
            m_tBlockers.Add(oBlocker);
        }

    }

    public void CheckBlockStatus()
    {
        foreach (UI_CameraMovementBlocker oBlocker in m_tBlockers)
        {
            if (oBlocker.m_bEnabled && oBlocker.m_bBlocked)
            {
                m_bControlEnabled = false;
                return;
            }
        }
        m_bControlEnabled = true;
    }

    public void ForcePosition(Vector3 v)
    {
        m_vTargetPos = v;
    }

    public void ToggleLayerMask(int _iLayer) // Toggle del bit específico de la capa que queremos cambiar
    {
        m_oCamera.cullingMask ^= 1 << _iLayer;
    }

    public void RemoveLayerMask(int _iLayer)
    {
        m_oCamera.cullingMask |= 1 << _iLayer;
    }
    public void AddLayerMask(int _iLayer)
    {
        m_oCamera.cullingMask &= ~(1 << _iLayer);
    }
    public bool IsLayerRendered(int layer)
    {
        return ((m_oCamera.cullingMask & (1 << layer)) != 0);
    }

    public void Reorientation()
    {
        m_fYRot = 0;
        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
    }

    public void ForceFocusOnPoint(Vector3 pos)
    {
        m_vTargetPos = pos;
        transform.position = pos;
    }

    void MovementAndZoomOnPoint(Vector2 v)
    {
        if (!m_bControlEnabled)
            return;

        RaycastHit oHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(v), out oHit, 1000);
        m_vTargetPos = oHit.point;
        m_vSecondaryTargetPos = new Vector3(oHit.point.x, m_vSecondaryTargetPos.y, oHit.point.z);
        StartCoroutine(DoMovementAndZoom(true));
    }

    void MovementAndZoomOutOnPoint(Vector2 v)
    {
        if (!m_bControlEnabled)
            return;

        RaycastHit oHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(v), out oHit, 1000);
        m_vTargetPos = oHit.point;
        m_vSecondaryTargetPos = new Vector3(oHit.point.x, m_vSecondaryTargetPos.y, oHit.point.z);
        StartCoroutine(DoMovementAndZoom(false));
    }

    public void FocusOn(Vector3 _oPoint)
    {
        m_vTargetPos = _oPoint;
    }

    IEnumerator DoMovementAndZoom(bool zoomIn)
    {
        if (zoomIn)
            m_fZPos /= 2f;
        else
            m_fZPos *= 2f;

        if (m_fZPos > -20)
            m_fZPos = -20;
        else if (m_fZPos < -1200)
            m_fZPos = -1200;


        while (Vector3.Distance(transform.position, m_vTargetPos) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.Lerp(transform.position, m_vTargetPos, Time.deltaTime * 2);
            UpdateZoom();
        }
    }

    void ResetMousePanning()
    {
        m_bIsMousePanning = false;
        m_fXPan = 0;
        m_fYPan = 0;
    }

    void SetMousePanning()
    {
        m_bIsMousePanning = true;
    }

    void UpdateKeyboardXDelta(float f)
    {
        m_fXPanK = f;
        Panning();
    }
    void UpdateKeyboardYDelta(float f)
    {
        m_fYPanK = f;
        Panning();
    }
    void UpdateMouseXDelta(float f)
    {
        if (m_bIsRotating)
            m_fYRot += f;
        if (m_bIsMousePanning)
            m_fXPan = -f;
    }
    void UpdateMouseYDelta(float f)
    {
        if (m_bIsRotating)
            m_fXRot -= f;
        if (m_bIsMousePanning)
            m_fYPan = -f;
    }

    void Orbit()
    {
        if (!m_bIsRotating)
            m_bIsRotating = true;

        if (m_fXRot > m_fMaxxRot) m_fXRot = m_fMaxxRot;
        if (m_fXRot < m_fMinxRot) m_fXRot = m_fMinxRot;

        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
        m_oReference.transform.localEulerAngles = new Vector3(-m_fXRot, 0, 0);
    }

    public void Rotate()
    {
        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
    }


    void Panning()
    {

        if (!m_bControlEnabled)
            return;

        float fFinalMoveSpeed = 0;

        if ((m_fXPanK != 0 || m_fYPanK != 0) || m_fXPan != 0 || m_fYPan != 0)
        {

            float fFactor = ZoomFactor() / (float)200;
            if (Screen.height > Screen.width)
            {
                fFactor *= 0.2f;
            }
            fFinalMoveSpeed = m_fMoveSpeed * fFactor;
        }

        m_vTargetPos = m_vTargetPos + fFinalMoveSpeed * (m_fXPanK + m_fXPan) * m_oReference.transform.right + fFinalMoveSpeed * (m_fYPanK + m_fYPan) * m_oReference.transform.forward;
        transform.position = Vector3.Lerp(transform.position, m_vTargetPos, Time.deltaTime * 10);


    }

    void DoZoom(float f)
    {


        float fDelta = f * ZoomFactor();

        if ((fDelta > 0 && m_fZPos < -m_fMinZoom) || fDelta < 0 && m_fZPos > -m_fMaxZoom)
        {
            m_fZPos = m_fZPos + fDelta;

            if (m_fZPos < -m_fMaxZoom)
                m_fZPos = -m_fMaxZoom;
            if (m_fZPos > -m_fMinZoom)
                m_fZPos = -m_fMinZoom;

            for (int i = m_tZoomLevels.Count - 1; i >= 0; i--)
            {
                if (m_fZPos < m_tZoomLevels[i])
                {
                    m_fZoomLevel = i - 1;
                }
            }
            if (m_fZoomLevel < 0)
                m_fZoomLevel = 0;
        }
        UpdateZoom();
    }

    float ZoomFactor()
    {
        return (-m_oCamera.transform.localPosition.z);
    }

    public void UpdateZoom()
    {
        float fZSmooth = Mathf.Lerp(m_oCamera.transform.localPosition.z, m_fZPos, Time.deltaTime * 5);
        Vector3 vNewPos = m_oCamera.transform.localPosition;
        vNewPos.z = fZSmooth;
        m_oCamera.transform.localPosition = vNewPos;
    }

    void ChangeZoomDirectly(float m_fZPos)
    {
        Vector3 vNewPos = m_oCamera.transform.localPosition;
        vNewPos.z = m_fZPos;
        m_oCamera.transform.localPosition = vNewPos;
    }

    private void Update()
    {
        CheckBlockStatus();
    }

}
