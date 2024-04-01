using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Twin
{
    public class ProductionController : MonoBehaviour
    {
        private List<Machine> m_tMachines = new List<Machine>();
        private List<Belt> m_tBelts = new List<Belt>();
        private List<OutputTruck> m_tTrucks = new List<OutputTruck>();

        private List<ActivationButton> m_tActivationButtons = new List<ActivationButton>();

        public float m_fTotalAmount = 0;
        public float m_fAmountLastMinute = 0;

        public Text m_oAmountLastMinuteDisplay;

        private void Start()
        {
            m_tMachines = FindObjectsOfType<Machine>().ToList();
            m_tBelts = FindObjectsOfType<Belt>().ToList();
            m_tTrucks = FindObjectsOfType<OutputTruck>().ToList();
            m_tActivationButtons = FindObjectsOfType<ActivationButton>().ToList();

            foreach (OutputTruck o in m_tTrucks)
            {
                o.m_eSomethingProduced += AddValue;
            }
            
            TimeController.m_oMinuteElapsed += MinuteElapsed;
        }

        public List<float> m_tValues = new List<float>();

        public void AddValue(float f)
        {
            m_fAmountLastMinute += f;
        }

        public void StartSimulation()
        {
            foreach (ActivationButton act in m_tActivationButtons)
            {
                act.Activate();
            }
        }

        public void StopSimulation()
        {
            foreach (ActivationButton act in m_tActivationButtons)
            {
                act.Deactivate();
            }
        }

        void MinuteElapsed()
        {
            m_tValues.Add(m_fAmountLastMinute);
            m_oAmountLastMinuteDisplay.text = m_fAmountLastMinute.ToString();
            m_fAmountLastMinute = 0;
        }

    }

}
