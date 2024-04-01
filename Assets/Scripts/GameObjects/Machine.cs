using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Twin
{

    public class Machine : MonoBehaviour
    {
        public string m_sProcessName;
        public ProductType m_oProductType;
        private Product m_oProduct;
        public float m_fTypicalExitAmount;
        public float m_fTypicalTime;

        public string m_sMachineName;
        public List<Storage> m_oSourceStorages = new List<Storage>();
        public Storage m_oTargetStorage;
        public float m_fOutputAmount = 0;
        public bool m_bDoing;
        public bool m_bEnabled;
        public float m_fCheckTimeInterval;

        private float m_fBuiltTimeUsed = 0;

        public GameObject m_oProductSample;

        public MeshRenderer m_oActionDisplay;

        [HideInInspector]
        public MachineDisplay m_oDisplay;

        ProductLibrary m_oLibrary;

        private void Start()
        {
            m_oLibrary = FindObjectOfType<ProductLibrary>();
            m_oProduct = m_oLibrary.GetProductByType(m_oProductType);
            m_oDisplay.m_oLinkedMachine = this;
            m_oDisplay.ShowStatus(m_bEnabled);
            m_fTypicalTime = m_oDisplay.m_oSlider.value;
            m_oDisplay.SetProductionTime(m_fTypicalTime);
            StartCoroutine(CheckInputs());
        }

        public void SetEnabled(bool enabled)
        {
            m_bEnabled = enabled;
            m_oDisplay.ShowStatus(m_bEnabled);
                
        }

        private void Update()
        {
            if(m_bDoing && m_bEnabled)
            {
                m_fBuiltTimeUsed += Time.deltaTime;
                if (m_fBuiltTimeUsed >= m_fTypicalTime)
                {
                    m_fOutputAmount ++;
                    m_oProductSample.SetActive(false);
                    m_bDoing = false;
                    m_oTargetStorage.ModifyAmount(1);
                }
                else
                {
                    float fPercentage = m_fBuiltTimeUsed / m_fTypicalTime;
                    m_oProductSample.transform.localPosition = Vector3.up * fPercentage - Vector3.up * 0.3f;
                    m_oDisplay.SetValue(fPercentage);
                }
            }
        }

        IEnumerator CheckInputs()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_fCheckTimeInterval);
                if(!m_bDoing && m_bEnabled)
                {
                    bool bOk = true;
                    for (int i = 0; i < m_oProduct.m_tComponents.Count; i++)
                    {
                        if (bOk && m_oSourceStorages[i].m_fAmount >= m_oProduct.m_tComponentAmounts[i]) //Suficiente material
                        {
                            bOk = true;
                        }
                        else
                        {
                            bOk = false;
                        }
                    }
                    if (bOk)
                    {
                        StartProduction();
                        for (int i = 0; i < m_oSourceStorages.Count; i++)
                        {
                            m_oSourceStorages[i].ModifyAmount(-m_oProduct.m_tComponentAmounts[i]); 
                        }
                    }
                    else
                    { 
                        StopProduction();
                    }
                }
            }
        }

        void StartProduction()
        {
            m_bDoing = true;
            m_fBuiltTimeUsed = 0;
            m_oDisplay.SetValue(0);
            m_oDisplay.TurnOn();
            m_oProductSample.SetActive(true);
            m_oProductSample.transform.localPosition = - Vector3.up * 0.3f;
            Debug.Log("Empieza la producción");
        }
        void StopProduction()
        {
            m_bDoing = false;
            m_oDisplay.TurnOff();
            Debug.Log("Se para la producción");
        }
    }

}
