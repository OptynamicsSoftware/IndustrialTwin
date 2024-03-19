using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twin
{
    public class StorageDisplay : MonoBehaviour
    {
        public Text m_oName, m_oValue, m_oMaxValue;
        public Slider m_oSlider;
        private Button m_oReloadButton;
        public Storage m_oStorage;

        public string m_sUnit;

        private void Start()
        {
            m_oReloadButton = GetComponentInChildren<Button>();
            m_oReloadButton.onClick.AddListener(()=> { m_oStorage.Fill(); });
            m_oSlider = GetComponentInChildren<Slider>();
            m_oStorage.m_oDisplay = this;
            m_oMaxValue.text = "/ " + GetComponentInChildren<Slider>().value.ToString() + " " + m_sUnit;
        }

        public void ShowInfinite()
        {
            m_oMaxValue.text = "∞ ";
            m_oMaxValue.fontSize = 55;
            m_oValue.text = "";
        }

        public void SetValue(float val)
        {
            m_oValue.text = val.ToString();
        }

        public void SetMaxValue(float val)
        {
            m_oStorage.m_fMaxAmount = val;
            m_oMaxValue.text = "/ " + val.ToString() + " " + m_sUnit;
            if(m_oSlider.value == m_oSlider.maxValue)
            {
                m_oStorage.SetInfinite(true);
            }
            else
            {
                m_oStorage.SetInfinite(false);
                m_oMaxValue.fontSize = 22;
            }
        }
    }

}
