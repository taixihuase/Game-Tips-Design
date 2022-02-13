using Core;
using RedScarf.EasyCSV;
using System.Collections.Generic;
using UnityEngine;

namespace CsvManager
{
    public abstract class AbstractCfgManager
    {
        public Dictionary<int, T> CreateCsv<T>(string csvName) where T : new()
        {
            CsvHelper.Init();
            string path = "CSV/" + csvName + ".csv";
            var text = AssetLoader.LoadAsset<TextAsset>(path);
            CsvHelper.Create(text.name, text.text);
            return CsvHelper.PaddingData<T>(text.name);
        }
    }
}
