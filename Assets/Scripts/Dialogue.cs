using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string script;

    public Sprite[] sprite;

    [TextArea(3,10)]
    public string[] lines;

    public List<Dictionary<string, string>> ParseFile()
    {
        bool allElements = false;
        int i = 0;
        TextAsset XmlAsset = Resources.Load<TextAsset>("Dialogue");
        var doc = XDocument.Parse(XmlAsset.text);

        var allDict = doc.Element("script").Elements(script);
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        foreach (var oneDict in allDict)
        {
            while (allElements == false)
            {
                var oneString = oneDict.Elements("string");
                XElement element = oneString.ElementAt(i);
                Debug.Log(element);
                string line = element.ToString().Replace("<string>", "").Replace("</string>", "");

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("script" + i, line);

                i += 1;

                allTextDic.Add(dic);
                if (i >= lines.Count())
                {
                    allElements = true;
                }

            }
        }
        return allTextDic;

    }

    public void assignLines()
    {
        List<Dictionary<string, string>> allTextDic = ParseFile();
        for (int i = 0; i < allTextDic.Count; i++)
        {
            Dictionary<string, string> dic = allTextDic[i];
            lines[i] = dic["script"+i];
        }
    }
}
