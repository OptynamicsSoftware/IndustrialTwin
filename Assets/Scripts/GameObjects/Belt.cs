using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twin
{
    public class Belt : MonoBehaviour //Cinta transportadora de un storage a otro
    {
        //A�adir espacio para cada producto
        public string m_sProcessName;
        public bool m_bShowProduct;
        public ProductType m_oProductType;
        private Product m_oProduct;
        public float m_fSpeed;
        public Storage m_oStorageSource; //Crear Source como clase padre de todo lugar del que coger materias?
        public Storage m_oStorageDestination;
        public bool m_bEnabled;

        private float m_fDistance;
        public float m_fInputTimeInterval;
        private float m_fMoveTotalTime;
        private float m_fInputLastTime = 0;

        public GameObject m_oProductPrefab;
        public Transform m_vOrigin, m_vEnd;

        [HideInInspector]
        public BeltDisplay m_oDisplay;
        EconomyController m_oEconomy;

        private void Awake()
        {
            m_oEconomy = FindAnyObjectByType<EconomyController>();
        }

        private void Start()
        {
            m_fDistance = Vector3.Distance(m_oStorageSource.transform.position,m_oStorageDestination.transform.position);
            if(m_oDisplay != null)
            {
                m_oDisplay.ShowStatus(m_bEnabled);
            }
        }

        public void SetEnabled(bool enabled)
        {
            m_bEnabled = enabled;
            if(m_oDisplay != null)
            {
                m_oDisplay.ShowStatus(m_bEnabled);
            }
        }

        private void Update()
        {
            if (m_bEnabled)
            {
                m_fMoveTotalTime = m_fDistance / m_fSpeed;

                if(((m_oStorageSource.m_fAmount > 0)||(m_oStorageSource.m_bInfiniteMaterial)) && Time.time > m_fInputTimeInterval + m_fInputLastTime)
                {
                    StartCoroutine(MoveProduct());
                    m_oEconomy.BuyProduct(m_oProductType);
                    m_fInputLastTime = Time.time;
                }
            }
        }

        IEnumerator MoveProduct()
        {
            m_oStorageSource.ModifyAmount(-1); //Cogemos pieza
            if(m_bShowProduct)
            {
                GameObject g = Instantiate(m_oProductPrefab, m_vOrigin.position, Quaternion.identity);
                g.GetComponent<Rigidbody>().velocity = transform.forward * m_fSpeed * 0.9f;
                yield return new WaitForSeconds(m_fMoveTotalTime);
                Destroy(g);
            }
            else
            {
                yield return new WaitForSeconds(m_fMoveTotalTime);

            }
            m_oStorageDestination.ModifyAmount(1);
        }
    }

}
