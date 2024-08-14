using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace Twin
{
    public class OptimizeProductionAgent : Agent
    {
        public ProductionController m_oProductionController;
        public List< Storage> m_tStoragesToEmpty = new List<Storage>();
        public List<Belt> m_tBeltsToControl = new List<Belt>();
        public override void OnActionReceived(ActionBuffers actions)
        {
            float fMachineTime = actions.ContinuousActions[0];
            float fTruckAmout = actions.ContinuousActions[1];
            float fInputBeltWood = actions.ContinuousActions[2];
            float fInputBeltIron = actions.ContinuousActions[3];
            float fInputBeltChairs = actions.ContinuousActions[4];

            float fMinMachine = 1, fMaxMachine = 10, fMinBeltInput = 0.2f, fMaxBeltInput = 4, fMinTruckAmount = 1, fMaxTruckAmount = 9;

            m_oProductionController.m_tMachines[0].m_fTypicalTime += fMachineTime ;
            m_oProductionController.m_tMachines[0].m_fTypicalTime = Mathf.Clamp(m_oProductionController.m_tMachines[0].m_fTypicalTime, fMinMachine, fMaxMachine);
            //m_oProductionController.m_tMachines[0].m_fTypicalTime = (fMachineTime + 1) * 0.5f * (fMaxMachine - fMinMachine) + fMinMachine;


            m_oProductionController.m_tTrucks[0].m_fAmountToGet += fTruckAmout ;
            m_oProductionController.m_tTrucks[0].m_fAmountToGet = Mathf.Clamp(m_oProductionController.m_tTrucks[0].m_fAmountToGet, fMinTruckAmount, fMaxTruckAmount);
            //m_oProductionController.m_tTrucks[0].m_fAmountToGet = (fTruckAmout + 1) * 0.5f * (fMaxTruckAmount - fMinTruckAmount) + fMinTruckAmount;


            m_oProductionController.m_tBelts[0].m_fInputTimeInterval += fInputBeltWood ;
            m_oProductionController.m_tBelts[0].m_fInputTimeInterval = Mathf.Clamp(m_oProductionController.m_tBelts[0].m_fInputTimeInterval, fMinBeltInput, fMaxBeltInput);
            //m_tBeltsToControl[0].m_fInputTimeInterval = (fInputBeltWood + 1) * 0.5f * (fMaxBeltInput - fMinBeltInput) + fMinBeltInput;

            m_oProductionController.m_tBelts[1].m_fInputTimeInterval += fInputBeltIron;
            m_oProductionController.m_tBelts[1].m_fInputTimeInterval = Mathf.Clamp(m_oProductionController.m_tBelts[1].m_fInputTimeInterval, fMinBeltInput, fMaxBeltInput);
            //m_tBeltsToControl[1].m_fInputTimeInterval = (fInputBeltIron + 1) * 0.5f * (fMaxBeltInput - fMinBeltInput) + fMinBeltInput;


            m_oProductionController.m_tBelts[2].m_fInputTimeInterval += fInputBeltChairs;
            m_oProductionController.m_tBelts[2].m_fInputTimeInterval = Mathf.Clamp(m_oProductionController.m_tBelts[2].m_fInputTimeInterval, fMinBeltInput, fMaxBeltInput);
            //m_tBeltsToControl[2].m_fInputTimeInterval = (fInputBeltChairs + 1) * 0.5f * (fMaxBeltInput - fMinBeltInput) + fMinBeltInput;

        }
        public override void CollectObservations(VectorSensor sensor)
        {
            foreach (Storage oStorage in m_tStoragesToEmpty)
            {
                sensor.AddObservation(oStorage.m_fAmount);
            }
            foreach (Belt oBelt in m_oProductionController.m_tBelts)
            {
                sensor.AddObservation(oBelt.m_fInputTimeInterval);
            }
            foreach (Machine oMachine in m_oProductionController.m_tMachines)
            {
                sensor.AddObservation(oMachine.m_fTypicalTime);
            }
            foreach (OutputTruck oTruck in m_oProductionController.m_tTrucks)
            {
                sensor.AddObservation(oTruck.m_fAmountToGet);
            }
        }

        public override void OnEpisodeBegin()
        {
            if(m_oProductionController.m_bTraining)
            {
                foreach (Storage oStorage in m_tStoragesToEmpty)
                {
                    oStorage.m_fAmount = 0;
                }
                foreach (Belt oBelt in m_oProductionController.m_tBelts)
                {
                    oBelt.StopAllCoroutines();
                }
                OnBeltPiece[] tObjects = FindObjectsOfType<OnBeltPiece>();

                for (int i = tObjects.Length - 1; i >= 0; i--)
                {
                    Destroy(tObjects[i].gameObject);
                }
            }
        }
    }
}
