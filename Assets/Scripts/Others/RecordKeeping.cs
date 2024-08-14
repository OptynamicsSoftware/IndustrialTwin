using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Twin
{
    public class RecordKeeping
    {
        public static string m_sPath = "";
        private static float m_fRecord = Mathf.NegativeInfinity;
        static public void RecordSettings(ProductionController _oProdControl)
        {

            string sDate = System.DateTime.Now.ToString();
            sDate = sDate.Replace("/", "-");
            sDate = sDate.Replace(" ", "-");
            sDate = sDate.Replace(":", "-");
            if (m_sPath == "")
            {
                m_sPath = Application.persistentDataPath + "/Records/" + sDate;
                if (!Directory.Exists(m_sPath))
                {
                    Directory.CreateDirectory(m_sPath);
                }
            }

            string sPath = m_sPath + "/" + sDate + ".txt";


            StreamWriter oWriter = new StreamWriter(sPath);

            oWriter.WriteLine("Profit: " + _oProdControl.m_oEconomyController.m_fMoneyMadeLastMinute);
            oWriter.WriteLine("Last minute production: " + _oProdControl.m_fAmountLastMinute);
            oWriter.WriteLine("Wood belt input: " + _oProdControl.m_tBelts[0].m_fInputTimeInterval);
            oWriter.WriteLine("Iron belt input: " + _oProdControl.m_tBelts[1].m_fInputTimeInterval);
            oWriter.WriteLine("Chair belt input: " + _oProdControl.m_tBelts[2].m_fInputTimeInterval);
            oWriter.WriteLine("Machine time: " + _oProdControl.m_tMachines[0].m_fTypicalTime);
            oWriter.WriteLine("Truck amount: " + _oProdControl.m_tTrucks[0].m_fAmountToGet);

            oWriter.Close();
        }
        public static float GetRecord()
        {
            return m_fRecord;
        }
        public static void SetRecord(float _fAmount)
        {
            m_fRecord = _fAmount;
        }
    }

}
