using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twin
{
    public class BeltDisplay : MonoBehaviour
    {
        public Text m_oSpeedValue, m_oIntervalValue;
        public Image m_oStatus;
        public ConveyerBelt m_oBelt;

        private void Start()
        {
            m_oBelt.m_oDisplay = this;
        }

        public void ShowStatus(bool status)
        {
            m_oStatus.color = status ? Color.green : Color.red;
        }


        public void SetSpeedValue(float newValue)
        {
            m_oBelt.m_fSpeed = newValue;
            m_oSpeedValue.text = newValue.ToString("F1") + " m/s";
        }
        public void SetIntervalValue(float newValue)
        {
            m_oBelt.m_fInputTimeInterval = newValue;
            m_oIntervalValue.text = newValue.ToString("F1") + " s";
        }
    }

}
