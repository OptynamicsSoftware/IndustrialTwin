using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Twin
{
    public enum ProductType { Wood, Iron, Chair}
    [System.Serializable]
    public class Product
    {
        public string m_sName;
        public ProductType m_oType;
        public List<Product> m_tComponents = new List<Product>();
        public List<float> m_tComponentAmounts = new List<float>();
        public bool m_bElemental;
    }
}
