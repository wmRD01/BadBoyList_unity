using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
public class GoogleSheetTest : MonoBehaviour
{
    public Text text_Content;
    public Text text_Remark;

    //List<string> mList = new List<string>();
    public Dictionary<int, GoogleData> data = new Dictionary<int, GoogleData>();
    string url = "https://script.google.com/macros/s/AKfycbyJbce1n2I4XkJ4UYb-gdtB3QYkf-YwyEWXEn_2o-4SbZsZYfHCAK_Jo1s33s7VcXJK/exec";
    void Start()
    {
        StartCoroutine(getContent());
    }
    public void RandomText()
    {
        string content = "";
        GoogleData getDtat = new GoogleData();
        while (content == "")
        {
            int getIndex = Random.Range(1, data.Count);
            getDtat = data[getIndex];
            content = getDtat.Content;
        }
        text_Content.text = content;
        string remark = getDtat.Remark;
        if (remark == "") remark = "未命名製作者";
        text_Remark.text = "-by \r\n    " + remark;
    }
    IEnumerator AddContent()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "write");//新增資料
        form.AddField("ID", "myData");//新增資料

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //獲得東西
                print(www.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }
    IEnumerator getContent()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "read");//新增資料
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //獲得東西
                print(www.downloadHandler.text);
                string[] split = www.downloadHandler.text.Split(",");
                GoogleData temp = new GoogleData();
                for (int i = 0; i < split.Length; i++)
                {
                    int count = i % 6;
                    switch (count)
                    {
                        case 0:
                            temp = new GoogleData();
                            temp.ID = int.Parse(split[i]);
                            break;
                        case 1:
                            temp.Content = split[i];
                            break;
                        case 2:
                            if (split[i] != "")
                                temp.Type = int.Parse(split[i]);
                            break;
                        case 3:
                            temp.Remark = split[i];
                            break;
                        case 4:
                            temp.Level = split[i];
                            break;
                        case 5:
                            temp.CreateTime = split[i];
                            data.Add(temp.ID, temp);
                            break;
                    }
                }
                foreach (var item in data)
                {
                    Debug.Log($"ID:{item.Value.ID}," +
                        $"Content:{item.Value.Content}," +
                        $"Type:{item.Value.Type}," +
                        $"Remark:{item.Value.Remark}," +
                        $"Level:{item.Value.Level}," +
                        $"CreateTime:{item.Value.CreateTime},");
                }
                //var get = JsonUtility.FromJson<Object>(www.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }




}



public class GoogleData
{
    public int ID;
    public string Content;
    public int Type;
    public string Remark;
    public string Level;
    public string CreateTime;

}