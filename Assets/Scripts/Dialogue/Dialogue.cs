using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    #region Public Fields

    [TextArea(3, 10)]
    public string[] lines;

    public string[] name;

    public string option;

    public string script;

    public Sprite[] sprite;

    #endregion Public Fields

    #region Public Methods

    public void AssignOptions()
    {
        List<Dictionary<string, string>> allTextDic = ParseFileForOptions();
        for (int i = 0; i < allTextDic.Count; i++)
        {
            Dictionary<string, string> dic = allTextDic[i];
            lines[i] = dic["script" + i];
            name[i] = dic["name" + i];
            sprite[i] = Resources.Load<Sprite>(dic["sprite" + i]) as Sprite;
        }
    }

    public void AssignSpeechLines()
    {
        List<Dictionary<string, string>> allTextDic = ParseFile();
        for (int i = 0; i < allTextDic.Count; i++)
        {
            Dictionary<string, string> dic = allTextDic[i];
            lines[i] = dic["script" + i];
            name[i] = dic["name" + i];
        }
    }

    public bool CheckForOptions()
    {
        TextAsset XmlAsset = Resources.Load<TextAsset>("Dialogue");
        var doc = XDocument.Parse(XmlAsset.text);

        var allDictOption1 = doc.Element("script").Elements(script).Elements("Option1");
        var allDictOption2 = doc.Element("script").Elements(script).Elements("Option2");
        if (allDictOption1 != null && allDictOption2 != null)
        {
            DialogueManager.Instance._optionsBox.SetActive(true);

            GameObject option1 = DialogueManager.Instance._optionsBox.transform.Find("option 1").gameObject;
            GameObject option2 = DialogueManager.Instance._optionsBox.transform.Find("option 2").gameObject;

            foreach (var oneDict in allDictOption1)
            {
                var oneString = oneDict.Elements("option");
                XElement element = oneString.ElementAt(0);
                string line = element.ToString().Replace("<option>", "").Replace("</option>", "");
                option1.GetComponentInChildren<TMPro.TMP_Text>().text = line;
            }
            foreach (var oneDict in allDictOption2)
            {
                var oneString = oneDict.Elements("option");
                XElement element = oneString.ElementAt(0);
                string line = element.ToString().Replace("<option>", "").Replace("</option>", "");
                option2.GetComponentInChildren<TMPro.TMP_Text>().text = line;
            }
            return true;
        }
        return false;
    }

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
                int index = element.ToString().IndexOf(":");
                string characterName = element.ToString().Substring(0, index).Replace("<string>", "").Replace(":", "");
                string line = element.ToString().Replace("<string>", "").Replace("</string>", "").Replace(characterName + ":", "");

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("script" + i, line);
                dic.Add("name" + i, characterName);
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

    public List<Dictionary<string, string>> ParseFileForOptions()
    {
        bool allElements = false;
        int i = 0;
        TextAsset XmlAsset = Resources.Load<TextAsset>("Dialogue");
        var doc = XDocument.Parse(XmlAsset.text);

        var allDictOption1 = doc.Element("script").Elements(script).Elements("Option1");
        var allDictOption2 = doc.Element("script").Elements(script).Elements("Option2");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        if (option == "option 1")
        {
            foreach (var oneDict in allDictOption1)
            {
                while (allElements == false)
                {
                    var optionLine = oneDict.Elements("lines");
                    XElement Optionline = optionLine.ElementAt(0);
                    string lineNum = Optionline.ToString().Replace("<lines>", "").Replace("</lines>", "");
                    int NumberOfLines = System.Convert.ToInt32(lineNum);
                    lines = new string[NumberOfLines];
                    name = new string[NumberOfLines];
                    sprite = new Sprite[NumberOfLines];

                    var oneString = oneDict.Elements("string");
                    XElement element = oneString.ElementAt(i);
                    int index = element.ToString().IndexOf(":");
                    int SpriteIndex1 = element.ToString().IndexOf("(");
                    int SpriteIndex2 = element.ToString().IndexOf(")");
                    string CurrentSprite = element.ToString().Substring(SpriteIndex1, SpriteIndex2 - SpriteIndex1).Replace("(", "").Replace(")", "");
                    string characterName = element.ToString().Substring(0, index).Replace("<string>", "").Replace(":", "");
                    string line = element.ToString().Replace("<string>", "").Replace("</string>", "").Replace(characterName + ":", "").Replace("(" + CurrentSprite + ")", "");

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("script" + i, line);
                    dic.Add("name" + i, characterName);
                    dic.Add("sprite" + i, CurrentSprite);
                    i += 1;

                    allTextDic.Add(dic);
                    if (i >= lines.Count())
                    {
                        allElements = true;
                    }
                }
            }
        }
        else if (option == "option 2")
        {
            foreach (var oneDict in allDictOption2)
            {
                while (allElements == false)
                {
                    var optionLine = oneDict.Elements("lines");
                    XElement Optionline = optionLine.ElementAt(0);
                    string lineNum = Optionline.ToString().Replace("<lines>", "").Replace("</lines>", "");
                    int NumberOfLines = System.Convert.ToInt32(lineNum);
                    lines = new string[NumberOfLines];
                    name = new string[NumberOfLines];
                    sprite = new Sprite[NumberOfLines];

                    var oneString = oneDict.Elements("string");
                    XElement element = oneString.ElementAt(i);
                    int index = element.ToString().IndexOf(":");
                    int SpriteIndex1 = element.ToString().IndexOf("(");
                    int SpriteIndex2 = element.ToString().IndexOf(")");
                    string CurrentSprite = element.ToString().Substring(SpriteIndex1, SpriteIndex2 - SpriteIndex1).Replace("(", "").Replace(")", "");
                    string characterName = element.ToString().Substring(0, index).Replace("<string>", "").Replace(":", "");
                    string line = element.ToString().Replace("<string>", "").Replace("</string>", "").Replace(characterName + ":", "").Replace("(" + CurrentSprite + ")", "");

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("script" + i, line);
                    dic.Add("name" + i, characterName);
                    dic.Add("sprite" + i, CurrentSprite);
                    i += 1;

                    allTextDic.Add(dic);
                    if (i >= lines.Count())
                    {
                        allElements = true;
                    }
                }
            }
        }
        return allTextDic;
    }

    #endregion Public Methods
}