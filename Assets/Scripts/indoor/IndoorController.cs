using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndoorController : MonoBehaviour {
    public GameObject nailed;
    public GameObject unNail;
    public RemotController remotController;
    private MapController mc;
    private bool isNail = false;
    private Vector2 lastPosition;
    private float scale = 30f / 500f;
    private Vector2 offset = Vector2.zero;//new Vector2(1.2f, 19.2f);
    private Material material;
    private float timer;
    private Vector3[] anchors;
    private Vector3 anchorOffset;

    IEnumerator Start()
    {
        mc = GetComponent<MapController>();
        lastPosition = offset;
        material = GetComponent<Image>().material;

        anchorOffset = new Vector3(10.7f, 8.5f, 0);
        anchors = new Vector3[]{
            new Vector3(0, 0, 1.2f),
            new Vector3(5.3f, 0, 1.2f),
            new Vector3(-0.758f, 4.991f, 1.2f),
            new Vector3(5.3f, 4.991f, 1.5f)
        };
        yield return 0;
        setAnchors();
    }

    void Update()
    {
        if (isNail && Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < 180)
            {
                return;
            }
            Vector2 mp = mc.calcMousePosition(Input.mousePosition) * scale - offset;
            float distance = (mp - lastPosition).magnitude;
            if (distance >= 0.1)
            {
                lastPosition = mp;
                placePlyer();
            }
        }
    }


    public void showButton(bool isNail)
    {
        this.isNail = isNail;
        if (isNail)
        {
            nailed.SetActive(true);
            unNail.SetActive(false);
            mc.enabled = false;
        }
        else
        {
            nailed.SetActive(false);
            unNail.SetActive(true);
            mc.enabled = true;
        }
    }

    private void placePlyer()
    {
        if (Time.time - timer < 0.2)
        {
            return;
        }
        timer = Time.time;
        Vector2 point = (lastPosition + offset) / scale;
        print(lastPosition + offset);
        material.SetVector("_Target2", new Vector4(point.x, point.y, 0, 0));
        //remotController.conn.sendPlayerLocation(lastPosition + offset);
        setDistances(lastPosition + offset);
    }

    private void setAnchors()
    {
        double[][] anchorArray = new double[anchors.Length][];
        for (int i = 0; i < anchors.Length; i++)
        {
            anchors[i] += anchorOffset;
            anchorArray[i] = new double[] { anchors[i].x, anchors[i].y, anchors[i].z };
        }
        remotController.conn.setAnchors(anchorArray);
    }

    private void setDistances(Vector3 target)
    {
        target.z = 1.2f;
        double[] distances = new double[anchors.Length];
        for (int i = 0; i < anchors.Length; i++)
        {
            Vector3 distance = target - anchors[i];
            distances[i] = distance.magnitude;
        }
        remotController.conn.setDistances(distances);
    }
}
