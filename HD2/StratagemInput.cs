using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Helldiver
{
    class StratagemInput:MonoBehaviour
    {
        private int[] strategemArrowList;
        private List<int> inputList;
        private int maxInput;
        private int currentInputIndex = 0;
        private StratagemInfo stratagemInfo;
        private StratagemPanel stratagemPanel;
        private Image[] arrowList;


        void Awake()
        {
            
            strategemArrowList = GetComponent<StratagemInfo>().stratagemArrowArray;
            maxInput = GetComponent<StratagemInfo>().stratagemArrowCount;
            stratagemInfo = GetComponent<StratagemInfo>();
            stratagemPanel = GetComponent<StratagemPanel>();
            arrowList = stratagemInfo.arrowList;
            inputList =new List<int>();

            Debug.Log("已成功获取战略配备");
            Debug.Log(stratagemInfo);
            Debug.Log(arrowList[0].sprite);



        }
        void Update()
        {
            // 监听键盘输入
            if (Input.GetKeyDown(KeyCode.UpArrow)) InputKey(0);
            if (Input.GetKeyDown(KeyCode.DownArrow)) InputKey(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) InputKey(2);
            if (Input.GetKeyDown(KeyCode.RightArrow)) InputKey(3);
        }
        private void InputKey(int key)
        {
            inputList.Add(key); // 将按键转换为数字并添加到输入序列

            if (!CheckSequence())//匹配输入
            {
                Debug.Log("输入错误！已清空序列");
                SetArrowColorDef();
                currentInputIndex = 0;

                int randomErrorIndex = Random.Range(12, 15);
                HelldiverAudioManager.Instance.PlaySound(randomErrorIndex);
                inputList.Clear();
                return;
            }
            else if (inputList.Count == maxInput)
            {
                Debug.Log("输入正确！");
                int randomCorrectIndex = Random.Range(8, 12);
                HelldiverAudioManager.Instance.PlaySound(currentInputIndex);
                HelldiverAudioManager.Instance.PlaySound(randomCorrectIndex);
                SetArrowColorDef();
                currentInputIndex = 0;
                inputList.Clear();
                return;
            }
            HelldiverAudioManager.Instance.PlaySound(currentInputIndex);//播放对应音效
            arrowList[currentInputIndex].color = stratagemInfo.stratagemArrowInputColor;
            arrowList[currentInputIndex+1].color = stratagemInfo.stratagemArrowNextColor;
            currentInputIndex++;
            Debug.Log("当前输入: " + string.Join(", ", inputList));
            Debug.Log("目标序列: " + string.Join(", ", strategemArrowList));
        }

        private void SetArrowColorDef()
        {
            foreach (var item in arrowList)
            { 
                item.color = stratagemInfo.stratagemArrowDefColor;
            }
        }

        // 检查匹配
        private bool CheckSequence()
        {
            if (inputList.Count > maxInput) return false;

            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i] != strategemArrowList[i])
                    return false;
            }
            return true;
        }
    }
}
