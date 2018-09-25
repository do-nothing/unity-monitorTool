using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Kyrios;
using DG.Tweening;

public class Device : MonoBehaviour {

    Button button;
    Text text;
    Dropdown list;
    Image icon;
    Image bgColor;
    Image netSta;
    Image cover;

    bool Poweroff = false;

    public string deviceName;
    public string nickName;
    bool onSelect = false;

    public Kyrios.DeviceType deviceType;

    public NetworkStatus networkStatus;

    float delayTime = 0;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        list = GetComponentInChildren<Dropdown>();
        icon = transform.Find("Icon").GetComponent<Image>();
        bgColor = transform.Find("Background").GetComponent<Image>();
        netSta = transform.Find("NetworkStatus").GetComponent<Image>();
        cover = transform.Find("Body/Cover").GetComponent<Image>();

        EventTriggerListener.Get(button.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(list.gameObject).onSelect = OnSelectChange;

        text.text = deviceName;

        gameObject.name = nickName;

        Sprite[] icons = Resources.LoadAll<Sprite>("Atlas/DevicesIcons");
        
        for (int i = 0; i < icons.Length; i++) {
            if (icons[i].name == deviceType.ToString()) {
                icon.sprite = icons[i];
                bgColor.color = BgColor.bgColor[i];
            }
        }


        
	}
	
	// Update is called once per frame
     public void OnButtonClick(GameObject go) {
         if (go.name == this.gameObject.name) {
             onSelect = !onSelect;
             transform.Find("Highlight").GetComponent<Image>().enabled = onSelect;            
             List<Device> terminals = RemotController.GetInstance().terminals;
             if (onSelect) {
                 terminals.Add(this);
             } else {
                 terminals.Remove(this);
             }
         }
	}


     private void OnSelectChange(GameObject go) {
         if (go.name == list.gameObject.name) {
             if (list.value != 0) {
                 //Run()
                 //print("Run" + '"' + list.options[list.value].text + '"');
             }
         }
     }


     public void Clear() {
         onSelect = !onSelect;
         transform.Find("Highlight").GetComponent<Image>().enabled = onSelect;
     }

     public void Feed() {
         delayTime = RemotController.GetInstance().delayTime;
         netSta.DOColor(new Color(0, 1, 0, 1), 2.5f).OnComplete(() => netSta.DOColor(new Color(0, 0, 0, 1), 2.5f));
     }

     public void Disable() {
         StartCoroutine(Countdown(10));
     }


     public void Update() {
         if (Poweroff) {
             netSta.DOColor(new Color(0, 0, 0, 0), 1.5f);
         } else {
             if (delayTime > 0) {
                 delayTime -= Time.deltaTime;
             } else {
                 netSta.DOColor(new Color(1, 0, 0, 1), 2.5f);
             }
         }
     }

     IEnumerator Countdown(float delay) {
         yield return new WaitForSeconds(delay);
         Poweroff = true;
         cover.DOColor(new Color(0.8f, 0.8f, 0.8f, 1), 1.5f);
     }


     
}
