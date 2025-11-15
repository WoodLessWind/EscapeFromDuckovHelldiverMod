using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Helldiver
{
    internal class StratagemReady:MonoBehaviour
    {
        public float stratagemArivalTime;
        public GameObject stratagemArivalSpeed;
        public  GameObject EagleOne;
        public  Quaternion throwBeaconBallRotation;
        public  Vector3 stratagemPosition;

        private Text countdownText;

        private float remainingTime;
        public void CallingEagle()
        {
            Debug.Log(EagleOne);
            Debug.Log(transform.position);
            Debug.Log(throwBeaconBallRotation);
            GameObject temp= Instantiate(EagleOne, transform.position, throwBeaconBallRotation);
            Debug.Log(temp);
            Debug.Log(temp.transform.position);
            Debug.Log(temp.transform.rotation);
        }
    }
}