using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GuideController : MonoBehaviour
{

    private ConnController conn;
    private Material material;
    private Text text;

    private Dictionary<string, GuideData> players = new Dictionary<string, GuideData>();

    void Start()
    {
        conn = RemotController.GetInstance().conn;
        conn.addMessageListener(messageHandler);
        material = GetComponent<Image>().material;
        text = GameObject.Find("Guide/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 1;
        text.text = "";
        long time = DateTime.Now.Ticks;

        lock (players)
        {
            foreach (var item in players)
            {
                if (i++ > 5)
                {
                    return;
                }

                string id = item.Key;
                if (time - item.Value.time > 50000000)
                {
                    players.Remove(id);
                    material.SetVector("_Color" + i, Vector4.zero);
                    return;
                }

                Vector4 player = item.Value.player;
                string guideInfo = item.Value.guideInfo;
                Color color = new Color();
                ColorUtility.TryParseHtmlString("#" + id, out color);
                material.SetVector("_Target" + i, player);
                material.SetVector("_Color" + i, color);
                text.text += "<color=#" + id + ">" + guideInfo + "</color>\n";
            }
        }
    }

    private void messageHandler(JsonData json)
    {
        if (json["contentBean"]["command"].ToString() == "processGuideInfo")
        {
            string id = json["id"].ToString();
            processGuideInfo(id, json["contentBean"]["args"][0]);
        }

    }

    private void processGuideInfo(string id, JsonData info)
    {
        Vector4 player = Vector4.zero;
        player.x = getFloatValue(info["x"]);
        player.y = getFloatValue(info["y"]);
        player.z = getFloatValue(info["z"]);
        player.w = getFloatValue(info["w"]);

        StringBuilder stringBuilder = new StringBuilder(id);
        stringBuilder.Append(" x:" + player.x);
        stringBuilder.Append(" y:" + player.y);
        stringBuilder.Append(" 精度:" + player.z);
        stringBuilder.Append(" 方向:" + player.w);
        stringBuilder.Append(" 状态:" + info["status"]);
        stringBuilder.Append(" 工作:" + info["work"]);
        string guideInfo = stringBuilder.ToString();

        GuideData data = new GuideData();
        data.player = player;
        data.guideInfo = guideInfo;
        data.time = DateTime.Now.Ticks;

        lock (players)
        {
            if (players.ContainsKey(id))
            {
                players[id] = data;
            }
            else
            {
                players.Add(id, data);
            }
        }
        //print(id + " --> " + info.ToJson());
    }

    private float getFloatValue(JsonData data)
    {
        float value = 0;

        if (data.IsDouble)
        {
            value = (float)(double)data;
        }
        else if (data.IsInt)
        {
            value = (int)data;
        }

        return value;
    }

    public void sendCommand(){
        string command = "play:Voice/park/道路施工";
        foreach(string id in players.Keys){
            conn.sendGuideCommand(id, command);
        }
    }

    private class GuideData
    {
        internal string guideInfo;
        internal Vector4 player;
        internal long time;
    }
}
