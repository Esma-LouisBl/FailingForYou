using UnityEngine;
using System.Text;
using System.IO;

public class CSV_Writer : MonoBehaviour
{
    private string _fileName = "";

    [System.Serializable]
    public class Data
    {
        public int numberOfInputs;
        public int timePlaying;
    }

    [System.Serializable]
    public class DataList
    {
        public Data[] data;
    }

    public DataList dataList = new DataList();
    void Start()
    {
        _fileName = Application.dataPath + "/Data/playtest_data.csv";
    }

    public void WriteCSV()
    {
        if (dataList.data.Length > 0)
        {
            TextWriter tw = new StreamWriter(_fileName, false, Encoding.UTF8);
            tw.WriteLine("Inputs; TimePlaying");
            tw.Close();
            
            tw = new StreamWriter(_fileName, true);
            
            for (int i = 0; i < dataList.data.Length; i++)
            {
                tw.WriteLine(dataList.data[i].numberOfInputs + ";" + dataList.data[i].timePlaying);
            }
            tw.Close();
        }

    }
}
