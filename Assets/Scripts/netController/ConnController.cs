using LitJson;
using Microwise.Guide.NetConn;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnController : MonoBehaviour
{

    private Messenger messenger;
    private ProcessMessage heartbeatHandler;
    private ProcessMessage messageHandler;

    public void init()
    {
        messenger = UdpMessenger.getInstance("121.42.196.133", 5555);
        //messenger = UdpMessenger.getInstance("127.0.0.1", 5555);
        messenger.addMessageListener(processMessage);
    }

    public void close()
    {
        messenger.destroy();
    }


    public void resstart(string target, string appName, string version = "")
    {

        string str = "{\"id\":\"monitor\",\"target\":\"kiosk001\",\"logType\":\"monitor control\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"restart\",\"args\":[\"guanniao\",\"v2.0\"]}}";
        JsonData json = JsonMapper.ToObject(str);
        json["contentBean"]["args"][0] = appName;
        json["contentBean"]["args"][1] = version;

        json["target"] = target;
        messenger.sendMessage(json);
    }

    public void install(string target, string appName, string version = "")
    {

        string str = "{\"id\":\"monitor\",\"target\":\"kiosk001\",\"logType\":\"monitor control\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"install\",\"args\":[\"guanniao\",\"v2.0\"]}}";
        JsonData json = JsonMapper.ToObject(str);
        json["contentBean"]["args"][0] = appName;
        json["contentBean"]["args"][1] = version;

        json["target"] = target;
        messenger.sendMessage(json);
    }

    public void powerOff(string target)
    {
        string str = "{\"id\":\"monitor\",\"target\":\"kiosk_001\",\"logType\":\"monitor control\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"shutdown\",\"args\":[\"元智展厅集中管理平台已发出关机指令，本机将在1分钟后关闭。\"]}}";
        JsonData json = JsonMapper.ToObject(str);

        json["target"] = target;
        messenger.sendMessage(json);
    }

    public void sendGuideCommand(string target, string command)
    {
        string str = "{\"id\":\"monitor\",\"target\":\"guide_001\",\"logType\":\"monitor control\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"addtoTasklist\",\"args\":[\"\"]}}";
        /*JsonData json = JsonMapper.ToObject(str);

        json["target"] = target;
        json["contentBean"]["args"][0] = command;
        messenger.sendMessage(json);*/
        MessageBean mb = MessageBean.CreateFromJSON(str);
        mb.target = target;
        mb.contentBean.args = new string[] { command };
        messenger.sendMessage(mb);
    }

    [Obsolete] 
    public void sendPlayerLocation(Vector2 location)
    {
        string str = "{\"id\":\"monitor\",\"target\":\"indoordemo\",\"logType\":\"monitor control\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"placePlayer\",\"args\":[0, 0]}}";
        JsonData json = JsonMapper.ToObject(str);
        json["contentBean"]["args"][0] = location.x;
        json["contentBean"]["args"][1] = location.y;
        messenger.sendMessage(json);
    }

    public void setAnchors(double[][] Anchors)
    {
        string str = "{\"id\":\"monitor\",\"target\":\"indoordemo\",\"logType\":\"monitor control\",\"strategy\":\"trilateration\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"setAnchors\",\"args\":[]}}";
        JsonData json = JsonMapper.ToObject(str);
        json["contentBean"]["args"] = JsonMapper.ToObject(JsonMapper.ToJson(Anchors));
        messenger.sendMessage(json);
    }

    public void setDistances(double[] distances)
    {
        string str = "{\"id\":\"monitor\",\"target\":\"indoordemo\",\"logType\":\"monitor control\",\"strategy\":\"trilateration\",\"quality\":0,\"timestamp\":1494825498577," +
                "\"contentBean\":{\"command\":\"setDistances\",\"args\":[]}}";
        JsonData json = JsonMapper.ToObject(str);
        json["contentBean"]["args"] = JsonMapper.ToObject(JsonMapper.ToJson(distances));
        messenger.sendMessage(json);
    }

    public void turnLight(bool flag)
    {
        String str = "{\"id\":\"monitor\",\"target\":\"JY05SfZdGcM0WDdO\",\"logType\":\"path\",\"strategy\":\"relay\",\"quality\":0,\"timestamp\":1494825498577," +
            "\"contentBean\":{\"command\":\"setStatus\",\"args\":[2, 1]}}";
        JsonData json = JsonMapper.ToObject(str);
        if (flag)
            json["contentBean"]["args"][1] = 1;
        else
            json["contentBean"]["args"][1] = 0;
        messenger.sendMessage(json);
        messenger.sendMessage(json);
        messenger.sendMessage(json);
    }

    public void addHeartbeatListener(ProcessMessage processMessage)
    {
        heartbeatHandler = processMessage;
    }

    public void addMessageListener(ProcessMessage processMessage)
    {
        messageHandler += processMessage;
    }

    private void processMessage(JsonData json)
    {
        if (json["strategy"].ToString() == "heartbeat")
        {
            heartbeatHandler(json);
        }
        else
        {
            messageHandler(json);
        }
    }
}
