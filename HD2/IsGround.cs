using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Helldiver
{
    class IsGround:MonoBehaviour
    {
        public Grenade grenade;
        public Quaternion throwBeaconBallRotation;
        void Awake()
        {
            grenade = GetComponent<Grenade>();
            throwBeaconBallRotation = CharacterMainControl.Main.characterModel.transform.rotation*Quaternion.Euler(0,180,0);
            Debug.Log("监听手雷↓");
            Debug.Log(grenade);
            Debug.Log(throwBeaconBallRotation);
            StartCoroutine(Helldiver.ModBehaviour.CheckGrenadeLanding(grenade,throwBeaconBallRotation));

        }
    }
}
