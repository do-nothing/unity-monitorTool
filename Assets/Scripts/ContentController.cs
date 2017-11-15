using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ContentController : MonoBehaviour
{

    public GameObject exButton;
    public GameObject guidButton1;
    public GameObject guidButton2;
    public GameObject map1;
    public GameObject map2;
    private int level = 0;

    public void showExhibition()
    {
        if (level > 0)
        {
            level--;
            showLevel();
        }
    }

    public void showGuidePlatform()
    {
        if (level < 2)
        {
            level++;
            showLevel();
        }
    }

    private void showLevel()
    {
        guidButton1.SetActive(false);
        guidButton2.SetActive(false);
        exButton.SetActive(false);
        exButton.SetActive(false);
        switch (level)
        {
            case 0:
                transform.DOMoveY(0, 0.5f);
                exButton.SetActive(true);        
                break;
            case 1:
                transform.DOMoveY(1600, 0.5f).OnComplete(()=>map2.SetActive(false));
                guidButton1.SetActive(true);
                map1.SetActive(true);
                break;
            case 2:
                transform.DOMoveY(3200, 0.5f).OnComplete(() => map1.SetActive(false));
                guidButton2.SetActive(true);
                map2.SetActive(true);
                break;
            default:
                break;
        }
    }
}
