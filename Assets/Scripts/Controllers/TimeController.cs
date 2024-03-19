using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Twin
{
    public class TimeController : MonoBehaviour
    {
        static public System.Action m_oMinuteElapsed;

        static public TimeController Instance;

        float m_fLastMinuteMoment = 0;

        public Text m_oSpeedValue;

        private void Start()
        {
            Time.timeScale = 1;
            Instance = this;
        }

        public void ChangeTimeScale()
        {
            Time.timeScale *= 2;
            if(Time.timeScale > 20)
            {
                Time.timeScale = 1;
            }
            m_oSpeedValue.text = Time.timeScale.ToString() + "x"; 
        }

        private void Update()
        {
            if(Time.time >= m_fLastMinuteMoment + 60)
            {
                m_fLastMinuteMoment = Time.time;
                m_oMinuteElapsed.Invoke();
            }
        }



    }
}