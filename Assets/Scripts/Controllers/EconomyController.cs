using System.Collections;
using System.Collections.Generic;
using Twin;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    public float m_fDemandMinute = 20;
    public float m_fTotalDemand = 0;
    public float m_fProductPrice = 20;
    public float m_fMoneyMadeLastMinute = 0;
    public List<float> m_tValues = new List<float>();
    public ProductionController m_oProductionControl;
    public Dictionary<ProductType, float> m_tProductPrices = new Dictionary<ProductType, float> { { ProductType.Wood, 1 }, { ProductType.Iron, 2 } };

    private void Awake()
    {
        ProductionController oProductionControl = GetComponent<ProductionController>();
        if(oProductionControl != null && oProductionControl.m_bTraining)
        {
            m_oProductionControl = oProductionControl;
            m_oProductionControl.m_oEconomyController = this;
        }
    }
    void Update()
    {
        if (m_fTotalDemand > 0 && m_oProductionControl.m_fTotalAmount > 0)
        {
            m_fTotalDemand--;
            m_oProductionControl.m_fTotalAmount--;
            m_fMoneyMadeLastMinute += m_fProductPrice; 
            if (m_oProductionControl.m_bTraining)
            {
                m_oProductionControl.m_oAgent.AddReward(m_fProductPrice);
            }
        }
    }
    public void BuyProduct(ProductType _eType)
    {
        if(m_tProductPrices.ContainsKey(_eType))
        {
            float fAmount = m_tProductPrices[_eType];
            m_fMoneyMadeLastMinute -= fAmount;
            if (m_oProductionControl.m_bTraining)
            {
                m_oProductionControl.m_oAgent.AddReward(-fAmount);
            }
        }
    }
}
