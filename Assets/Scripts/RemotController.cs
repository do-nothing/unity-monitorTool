using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kyrios;
using LitJson;

public class RemotController : MonoBehaviour
{
    private string lightFlag;
    public ConnController conn;

    List<string> targets = new List<string>();

    public List<Device> terminals = new List<Device>();

    public float delayTime = 10.0f;


    public Device[] devices;


    protected static RemotController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        conn.init();
        devices = Object.FindObjectsOfType<Device>();
        conn.addHeartbeatListener(heartbeatHandler);
        conn.addMessageListener(messageHandler);

        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
        if (lightFlag == "1")
            light.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        else
            light.GetComponent<Image>().color = new Color(0, 0, 0, 1);
    }

    void OnDestroy()
    {
        conn.close();
    }

    private void heartbeatHandler(JsonData json)
    {
        //print("received a heartbeat packet from --> " + json.ToJson());

        for (int i = 0; i < devices.Length; i++)
        {
            //print(devices[i].nickName + "|" + json["id"].ToString());
            if (devices[i].nickName == json["id"].ToString())
            {
                devices[i].Feed();

            }
        }
    }

    private void messageHandler(JsonData json)
    {

        if (json["monitorId"] == null)
        {
            json["monitorId"] = "";
        }
        if (json["id"].ToString() == "JY05SfZdGcM0WDdO")
        {
            lightFlag = json["contentBean"]["args"][0].ToString().Substring(1, 1);
        }
    }

    public static RemotController GetInstance()
    {
        return instance;
    }


    public void Shutdown()
    {

        foreach (Device terminal in terminals)
        {
            conn.powerOff(terminal.nickName);
            terminal.Clear();
            terminal.Disable();
        }
        terminals.Clear();
    }

    public void Run()
    {
        foreach (Device terminal in terminals)
        {

            targets.Add(terminal.nickName);
            int value = terminal.transform.Find("Dropdown").GetComponent<Dropdown>().value;
            AppInfo appInfo = GetAppInfo(value);
            conn.resstart(terminal.nickName, appInfo.name, appInfo.version);
            terminal.Clear();
        }
        targets.Clear();
        terminals.Clear();
    }

    public void Install()
    {
        foreach (Device terminal in terminals)
        {

            targets.Add(terminal.nickName);
            int value = terminal.transform.Find("Dropdown").GetComponent<Dropdown>().value;
            AppInfo appInfo = GetAppInfo(value);
            conn.install(terminal.nickName, appInfo.name, appInfo.version);
            terminal.Clear();
        }
        targets.Clear();
        terminals.Clear();
    }


    public AppInfo GetAppInfo(int i)
    {
        AppInfo info;
        switch (i)
        {
            case 0:
                info = new AppInfo("", "");
                break;
            case 1:
                info = new AppInfo("quanxi", "v1.0");
                break;
            case 2:
                info = new AppInfo("guanniao", "v1.0");
                break;
            case 3:
                info = new AppInfo("guanniao", "v2.0");
                break;
            case 4:
                info = new AppInfo("maqiu", "");
                break;
            case 5:
                info = new AppInfo("hanzi", "");
                break;
            case 6:
                info = new AppInfo("chadao", "");
                break;
            case 7:
                info = new AppInfo("bmy", "");
                break;
            default:
                info = new AppInfo("", "");
                break;
        }
        return info;
    }

    public GameObject light;
    public void turnLight(Toggle toggle)
    {
        conn.turnLight(toggle.isOn);

    }
}
