using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Twin
{
    public class ProductLibrary : MonoBehaviour
    {
        public List<Product> m_tProducts;

        public Product GetProductByType(ProductType type)
        {
            return m_tProducts.Find(x=>x.m_oType == type);
        }
    }



}
