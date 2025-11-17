using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helldiver
{
    class StratagemInfo : MonoBehaviour
    {
        public int[] stratagemArrowArray;
        public int stratagemArrowCount;

        public float stratagemArivalTime;

        public String stratagemName;
        public String stratagemDisplayName;

        public Image[] arrowList;
        public Sprite stratagemIcon;
        public Sprite stratagemIconEage;
        

        public Color stratagemBackgroundColorRed=new Color(0.29f,0.16f,0.16f,0.7f);
        public Color stratagemIconColor = new Color(1.0f,1.0f,1.0f, 0.7f);
        public Color stratagemArrowInputColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        public Color stratagemArrowNextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public Color stratagemArrowDefColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

        enum StratagemArrow
        {
            Up=0,
            Down=1,
            Left=2,
            Right=3
        }
    }
}
