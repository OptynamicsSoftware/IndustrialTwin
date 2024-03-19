using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twin
{
    public class TruckDisplay : MonoBehaviour
    {
        public Text m_oAmountToGet;
        public OutputTruck m_oTruck;

        private void Start()
        {
            m_oTruck.m_oDisplay = this;
        }

        public void SetAmountValue(float newValue)
        {
            m_oTruck.m_fAmountToGet = newValue;
            m_oAmountToGet.text = newValue.ToString();
        } 
 
    }

}
