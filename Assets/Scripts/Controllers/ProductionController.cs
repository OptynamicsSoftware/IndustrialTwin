using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Twin
{
    public class ProductionController : MonoBehaviour
    {
        public List<Machine> m_tMachines = new List<Machine>();
        public List<Belt> m_tBelts = new List<Belt>();
        public List<OutputTruck> m_tTrucks = new List<OutputTruck>();
        public List<ActivationButton> m_tActivationButtons = new List<ActivationButton>();

        public bool m_bStartInProgress, m_bTraining, m_bRecording = true;

        public float m_fTotalAmount = 0;
        public float m_fAmountLastMinute = 0;
        public float m_fDemandMinute = 0;

        int m_iMinutesPerEpisode = 3, m_iMinutesElapsed = 0;

        public Text m_oAmountLastMinuteDisplay;

        public OptimizeProductionAgent m_oAgent;

        private void Start()
        {
            //m_tMachines = FindObjectsOfType<Machine>().ToList();
            //m_tBelts = FindObjectsOfType<Belt>().ToList();
            //m_tTrucks = FindObjectsOfType<OutputTruck>().ToList();
            //m_tActivationButtons = FindObjectsOfType<ActivationButton>().ToList();

            foreach (OutputTruck o in m_tTrucks)
            {
                o.m_eSomethingProduced += AddValue;
            }
            
            TimeController.m_oMinuteElapsed += MinuteElapsed;
            if(m_bStartInProgress)
            {
                StartSimulation();
            }
        }

        public List<float> m_tValues = new List<float>();

        public void AddValue(float f)
        {
            m_fAmountLastMinute += f;
            if(m_bTraining)
            {
                m_oAgent.SetReward(f);
            }
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
            if(m_bRecording && m_fAmountLastMinute > RecordKeeping.m_fRecord)
            {
                RecordKeeping.m_fRecord = m_fAmountLastMinute;
                RecordKeeping.RecordSettings(this);
            }
            if(m_bTraining)
            {
                m_iMinutesElapsed++;
                if (m_iMinutesElapsed % m_iMinutesPerEpisode == 0)
                {
                    m_oAgent.EndEpisode();
                    //m_oAgent.SetReward(m_fAmountLastMinute);
                }
            }
            m_oAmountLastMinuteDisplay.text = m_fAmountLastMinute.ToString();
            m_fAmountLastMinute = 0;
        }

    }

}
