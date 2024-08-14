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
        public bool m_bAutoFillLists = true, m_bBeltsShowProducts = false;
        public List<float> m_tValues = new List<float>();
        public List<Machine> m_tMachines = new List<Machine>();
        public List<Belt> m_tBelts = new List<Belt>();
        public List<OutputTruck> m_tTrucks = new List<OutputTruck>();
        public List<ActivationButton> m_tActivationButtons = new List<ActivationButton>();

        public bool m_bStartInProgress, m_bTraining, m_bRecording = true;

        public float m_fTotalAmount = 0;
        public float m_fAmountSold = 0;
        public float m_fAmountLastMinute = 0;

        int m_iMinutesPerEpisode = 3, m_iMinutesElapsed = 0;

        public Text m_oAmountLastMinuteDisplay, m_oEarningsLastMinute;

        public OptimizeProductionAgent m_oAgent;
        public EconomyController m_oEconomyController;

        private void Start()
        {
            if(m_bAutoFillLists)
            {
                m_tMachines = FindObjectsOfType<Machine>().ToList();
                m_tBelts = FindObjectsOfType<Belt>().ToList();
                m_tTrucks = FindObjectsOfType<OutputTruck>().ToList();
                m_tActivationButtons = FindObjectsOfType<ActivationButton>().ToList();

                foreach (Belt o in m_tBelts)
                {
                    o.m_bShowProduct = m_bBeltsShowProducts;
                }
            }

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

        public void AddValue(float f)
        {
            m_fAmountLastMinute += f;
            m_fTotalAmount += f;

            //if (m_bTraining)
            //{
            //    m_oAgent.AddReward(f);
            //}
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
            m_oEconomyController.m_tValues.Add(m_oEconomyController.m_fMoneyMadeLastMinute);
            if(m_bRecording && m_oEconomyController.m_fMoneyMadeLastMinute > RecordKeeping.GetRecord())
            {
                RecordKeeping.SetRecord(m_oEconomyController.m_fMoneyMadeLastMinute);
                RecordKeeping.RecordSettings(this);
            }
            if(m_bTraining)
            {
                m_iMinutesElapsed++;
                if (m_iMinutesElapsed % m_iMinutesPerEpisode == 0)
                {
                    m_oAgent.EndEpisode();                    
                }
            }
            m_oAmountLastMinuteDisplay.text = m_fAmountLastMinute.ToString();
            m_oEarningsLastMinute.text = m_oEconomyController.m_fMoneyMadeLastMinute.ToString();
            m_oEconomyController.m_fTotalDemand += m_oEconomyController.m_fDemandMinute;
            m_fAmountLastMinute = 0;
            m_oEconomyController.m_fMoneyMadeLastMinute = 0;
            
        }

    }

}
