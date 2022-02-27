using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UploadScript : MonoBehaviour
{
    public InputField inputCode;
    public GameObject screenView;
    public GameObject screenItemPrefab;
    public List<ScreenDataSaved> screenData;
    public List<GameObject> screenDataGo;
    void Start()
    {
        for (int i = 0; i < screenView.GetComponent<RectTransform>().childCount; i++)
        {
            GameObject.Destroy(screenView.GetComponent<RectTransform>().GetChild(i));
        }
    }
    public void EnterItem()
    {
        int itemInd = 0;
        while (itemInd < inputCode.text.Length)
        {
            string name = "";
            string data = "";
            bool hitEnd = false;
            for (int i = itemInd; i < inputCode.text.Length; i++)
            {
                itemInd++;
                if (inputCode.text[i].ToString() == ":")
                {
                    break;
                }
                if (inputCode.text[i].ToString() == ";")
                {
                    hitEnd = true;
                }
                else
                {
                    if (hitEnd == false)
                    {
                        name = name + inputCode.text[i];
                    }
                    else
                    {
                        data = data + inputCode.text[i];
                    }
                }
            }
            ScreenDataSaved temp = new ScreenDataSaved();
            temp.name = name;
            temp.data = data;
            screenData.Add(temp);
            GameObject created = GameObject.Instantiate(screenItemPrefab, screenView.GetComponent<RectTransform>());
            screenDataGo.Add(created);
            created.GetComponent<ScreenEntryScript>().myId = screenDataGo.Count - 1;
            created.GetComponent<ScreenEntryScript>().parentScript = this;
            created.GetComponentInChildren<Text>().text = name;
        }
    }
    public void RemoveItem(int id)
    {
        screenData.RemoveAt(id);
        GameObject.Destroy(screenDataGo[id]);
        screenDataGo.RemoveAt(id);
        foreach (var item in screenDataGo)
        {
            if (item.GetComponent<ScreenEntryScript>().myId > id)
            {
                item.GetComponent<ScreenEntryScript>().myId--;
            }
        }
    }
    
}
[System.Serializable]
public struct ScreenDataSaved
{
    public string name;
    public string data;
}