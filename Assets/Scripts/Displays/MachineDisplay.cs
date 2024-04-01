using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twin
{
    public class MachineDisplay : MonoBehaviour
    {
        public Text m_oName, m_oProgressValue, m_oInfoText, m_oTimeValue;
        public Image m_oFill, m_oBackground, m_oStatus;
        public Slider m_oSlider;
        public Machine m_oLinkedMachine;

        private void Start()
        {
            m_oLinkedMachine.m_oDisplay = this;
            m_oSlider = GetComponentInChildren<Slider>(); 
        }

        public void SetValue(float val)
        {
            m_oProgressValue.text = ((int)(val*100)).ToString() + " %";
            m_oFill.fillAmount = val;
        }

        public void SetProductionTime(float time)
        {
            m_oLinkedMachine.m_fTypicalTime = time;
            m_oTimeValue.text = time.ToString("F1") + " s";
        }

        public void ShowStatus(bool s)
        {
            if (s)
            {
                m_oInfoText.enabled = true;
                m_oStatus.color = Color.green;
            }
            else
            {
                m_oStatus.color = Color.red;
                m_oInfoText.enabled = false;
            }
        }

        public void TurnOn()
        {
            m_oBackground.gameObject.SetActive(true);
            m_oFill.gameObject.SetActive(true);
            m_oInfoText.text = "Produciendo silla";
        }
        public void TurnOff()
        {
            m_oBackground.gameObject.SetActive(false);
            m_oFill.gameObject.SetActive(false);
            m_oInfoText.text = "Esperando material";
            m_oProgressValue.text ="";
        }
    }

}
