using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twin
{
    public class Storage : MonoBehaviour
    {
        public GameObject m_oLevel;
        public string m_sName;
        public ProductType m_oType;

        public float m_fAmount;
        public float m_fLostAmount;
        public float m_fMaxAmount;

        public bool m_bInfiniteMaterial;

        [HideInInspector]
        public StorageDisplay m_oDisplay;
        private void Start()
        {
            m_oDisplay.SetValue(m_fAmount);
        }

        public void ModifyAmount(float amountToAdd)
        {
            if(!m_bInfiniteMaterial)
            {
                if(m_fAmount + amountToAdd <= m_fMaxAmount)
                {
                    m_fAmount += amountToAdd;
                }
                else
                {
                    m_fAmount = m_fMaxAmount;
                    m_fLostAmount ++;
                }
                UpdateLevel();
            }
        }

        private void UpdateLevel()
        {
            float fLevel = -0.4f + 0.8f * (m_fAmount / m_fMaxAmount);
            m_oLevel.transform.localPosition = new Vector3(0, fLevel, 0);
            m_oDisplay.SetValue(m_fAmount);
        }

        public void SetInfinite(bool b)
        {
            m_bInfiniteMaterial = b;
            if(b)
            {
                m_oDisplay.ShowInfinite();
            }
        }

        public void Fill()
        {
            m_fAmount = m_fMaxAmount;
            UpdateLevel();
        }
    }
}

