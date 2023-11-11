using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public string filename;
    public noWItemData blankItem;
    public List<noWItemData> itemDatabase = new List<noWItemData>();

    public void LoaditemData()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        itemDatabase.Clear();

        List<Dictionary<string, object>> data = DataReader.Read(filename);
        for (var i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i]["id"].ToString(), System.Globalization.NumberStyles.Integer);
            string wordTR = data[i]["turkish_translation"].ToString();
            string wordEn = data[i]["noun"].ToString();
            string desc = data[i]["definition"].ToString();

            AddItem(id, wordTR, wordEn, desc);
        }
        watch.Stop();
    }
    void AddItem(int id, string wordTR, string wordEn, string desc)
    {
        noWItemData tempItemp = new noWItemData(blankItem);

        tempItemp.id = id;
        tempItemp.wordTR = wordTR;
        tempItemp.wordEn = wordEn;
        tempItemp.desc = desc;

        itemDatabase.Add(tempItemp);
    }
}
