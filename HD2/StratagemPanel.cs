using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;

namespace Helldiver
{
    class StratagemPanel:MonoBehaviour
    {
        private static Image[] arrowList;
        private static Image[] stratagemIcon;
        private static TextMeshProUGUI stratagemDisplayName;
        private static StratagemInfo stratagemInfo;
        void Awake()
        {
            arrowList = GetComponentsInChildren<Image>().Skip(4).ToArray();
            stratagemIcon = GetComponentsInChildren<Image>().Skip(1).Take(3).ToArray();
            stratagemDisplayName= GetComponentInChildren<TextMeshProUGUI>();
        }
        public static bool RegiStratagemToPanel(GameObject gameObject)
        {
            stratagemInfo = gameObject.GetComponent<StratagemInfo>();
            stratagemIcon[0].color= stratagemInfo.stratagemBackgroundColorRed;
            stratagemIcon[1].sprite = stratagemInfo.stratagemIconEage;
            stratagemIcon[1].color = stratagemInfo.stratagemIconColor;
            stratagemIcon[2].sprite = stratagemInfo.stratagemIcon;
            stratagemIcon[2].color = stratagemInfo.stratagemIconColor;
            stratagemDisplayName.text= stratagemInfo.stratagemDisplayName;
            //切换箭头
            for (int i = 0; i < stratagemInfo.stratagemArrowCount; i++)
            {
                Debug.Log(i);
                switch (stratagemInfo.stratagemArrowArray[i])
                {
                    case 0:
                        
                        arrowList[i].sprite = Helldiver.ModBehaviour.UpArrow;
                        break;
                    case 1:
                        arrowList[i].sprite = Helldiver.ModBehaviour.DownArrow;
                        break;
                    case 2:
                        arrowList[i].sprite = Helldiver.ModBehaviour.LeftArrow;
                        break;
                    case 3:
                        arrowList[i].sprite = Helldiver.ModBehaviour.RightArrow;
                        break;
                }
            }
            //隐藏多余的箭头
            for (int i = stratagemInfo.stratagemArrowCount; i < arrowList.Length; i++)
            {
                arrowList[i].enabled = false;
            }
            stratagemInfo.arrowList= arrowList;
            if (gameObject.GetComponent<StratagemInput>() == null)
            {
                gameObject.AddComponent<StratagemInput>();
            }
            return true;
            
        }
    }
}
