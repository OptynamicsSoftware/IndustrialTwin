using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twin
{
    public class ActivationButton : MonoBehaviour
    {

        Vector3 m_oOriginalPos;
        Vector3 m_oActivatedPos;

        public bool m_bStatus;

        public ProcessMachine m_oLinkedMachine;
        public ConveyerBelt m_oLinkedBelt;

        [HideInInspector]
        public Material m_oGreenMat, m_oRedMat;
        void Start()
        {
            m_oGreenMat = Resources.Load<Material>("Materials/Green");
            m_oRedMat = Resources.Load<Material>("Materials/Red");
            m_oOriginalPos = transform.position;
            m_oActivatedPos = transform.position - Vector3.up*0.02f;
            if (m_oLinkedMachine != null)
                m_bStatus = m_oLinkedMachine.m_bEnabled;
            else
                m_bStatus = m_oLinkedBelt.m_bEnabled;
            ConfigState(m_bStatus);
        }

        void ConfigState(bool state)
        {
            m_bStatus = state;
            DisplayState();
        }

        void DisplayState()
        {
            transform.position = m_bStatus ? m_oActivatedPos : m_oOriginalPos;
            GetComponent<MeshRenderer>().material = m_bStatus ? m_oGreenMat : m_oRedMat;
        }

        void ExecuteState()
        {
            if (m_oLinkedMachine != null)
                m_oLinkedMachine.SetEnabled(m_bStatus);
            else
                m_oLinkedBelt.SetEnabled(m_bStatus);
            DisplayState();
        }

        public void Activate()
        {
            m_bStatus = true;
            ExecuteState();
        }
        
        public void Deactivate()
        {
            m_bStatus = false;
            ExecuteState();
        }


        private void OnMouseDown()
        {
            m_bStatus = !m_bStatus;
            ExecuteState();
        }
    }

}
