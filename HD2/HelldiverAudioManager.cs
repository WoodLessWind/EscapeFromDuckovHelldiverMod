using FMOD.Studio;
using System.Collections;
using UnityEngine;
using Helldiver;
using static FMODUnity.RuntimeManager;
using System.IO;
using FMODUnity;
using System.Diagnostics.Tracing;
using System;
using FMOD;
using Debug = UnityEngine.Debug;

namespace Helldiver
{
    public class HelldiverAudioManager:MonoBehaviour
    {
        private Sound[] inputSounds;
        private Channel Channel;
        private ChannelGroup masterSfxGroup;
        public static HelldiverAudioManager Instance;//单例模式
        void Awake()
        {
            CheckInstance();
            string basePath = Path.Combine(Unit.GetModPath(), "Audio", "Input");
            Debug.Log("Loading sounds from: " + basePath);
            string[] files = Directory.GetFiles(basePath, "*.wav", SearchOption.AllDirectories);
            inputSounds = new Sound[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                Debug.Log("Found sound file: " + files[i]);
                CoreSystem.createSound(files[i], MODE.DEFAULT, out inputSounds[i]);
                Debug.Log("Loaded sound: " + files[i]);
            }


            Channel = default;
            GetBus("bus:/Master/SFX").getChannelGroup(out masterSfxGroup);
        }


        public void PlaySound(int index)
        {
            RuntimeManager.CoreSystem.playSound(inputSounds[index], masterSfxGroup, false, out Channel);
        }
        private void CheckInstance()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
