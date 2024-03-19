using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twin
{
    public class OutputTruck : MonoBehaviour //Camión que se lleva mercancías
    {

        public string m_sProcessName;

        public float m_fLoadTime;

        public float m_fAmountToGet;

        public Storage m_oStorageSource; //Crear Source como clase padre de todo lugar del que coger materias?

        [HideInInspector]
        public TruckDisplay m_oDisplay;

        public bool m_bEnabled;

        private bool m_bTaking;

        Animation m_oAnim;
        public AnimationClip[] m_tClips;

        public Action<float> m_eSomethingProduced;


        private void Start()
        {
            m_oAnim = GetComponent<Animation>();
        }

        public void SetEnabled(bool enabled)
        {
            m_bEnabled = enabled;
        }



        private void Update()
        {
            if (m_bEnabled)
            {
                if (m_oStorageSource.m_fAmount >= m_fAmountToGet && !m_bTaking)
                {
                    StartCoroutine(TakeProduct());
                }
            }
        }

        IEnumerator TakeProduct()
        {
            m_bTaking = true;
            yield return new WaitForSeconds(m_fLoadTime);

            m_oStorageSource.ModifyAmount(-m_fAmountToGet); //Cogemos piezas
            m_eSomethingProduced.Invoke(m_fAmountToGet);
            m_oAnim.clip = m_tClips[1];
            m_oAnim.Play();
            yield return new WaitForSeconds(m_fLoadTime);
            
            m_oAnim.clip = m_tClips[0];
            m_oAnim.Play();
            m_bTaking = false;
        }
    }

}
