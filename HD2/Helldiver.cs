using System;
using Unity;
using Duckov;
using UnityEngine;
using SodaCraft.Localizations;
using HarmonyLib;
using ItemStatsSystem;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using ItemStatsSystem.Items;
using Unity.VisualScripting;

namespace Helldiver
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static ModBehaviour Instance;

        public Harmony harmony;
        public static bool canBeThrow=false;
        public bool IsMainBind = false;
        public static String dataPath = GetModPath();
        public static Item registryBecanBall=null;
        public static CharacterMainControl mainControl;
        public static ItemAssetsCollection IAC;
        public static AssetBundle tempBeaconBallAB;
        public static AssetBundle tempUIAB;
        public static AssetBundle tempEagleOneAB;
        public static GameObject preFab;
        public static GameObject beaconLanded;
        public static GameObject stratagemsPanelPrefab;
        public static GameObject stratagemPanel;
        public static GameObject gameObjectEagle500;
        public static GameObject eagleOnePrefab;


        public static Sprite BeaconBallIcon;
        public static Sprite UpArrow;
        public static Sprite DownArrow;
        public static Sprite LeftArrow;
        public static Sprite RightArrow;
        public static Sprite StratagemsIconEageRed;
        public static Sprite Stratagems500KG;



        public Vector3 panelScale = new Vector3(2, 2, 2);
        public Vector3 panelPositon = new Vector3(200, 600, 0);


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 场景切换时保留
            }
            else
            {
                Destroy(gameObject); // 销毁重复实例
            }
        }

        protected override void OnAfterSetup()
        {
            LoadAB();
            RegistryBecanBall();
            SetupStratagemInfo();
            CreateStratagemsPanel();

            harmony = new Harmony("Helldiver");
            harmony.PatchAll();
        }

        private void SetupStratagemInfo()
        {
            gameObjectEagle500 = new GameObject();
            gameObjectEagle500.name = "Eagle500KG";

            StratagemInfo stratagemInfo = gameObjectEagle500.AddComponent<StratagemInfo>();
            stratagemInfo.stratagemArrowArray = new int[] { 0, 3, 1, 1, 1 };//↑ → ↓ ↓ ↓
            stratagemInfo.stratagemArrowCount = stratagemInfo.stratagemArrowArray.Length;
            stratagemInfo.stratagemName = "Stratagems500KG";
            stratagemInfo.stratagemDisplayName = "\"飞鹰\"500KG炸弹";
            stratagemInfo.stratagemIcon = Stratagems500KG;
            stratagemInfo.stratagemIconEage = StratagemsIconEageRed;
            stratagemInfo.stratagemArivalTime = 5.0f;
            Instantiate(gameObjectEagle500);
            DontDestroyOnLoad(gameObjectEagle500);



        }

        private static void RegistryBecanBall()
        {
            Item prefab = ItemAssetsCollection.GetPrefab(66);
            GameObject gameObject = GameObject.Instantiate<GameObject>(prefab.gameObject);


            gameObject.name = "10261026";
            GameObject.DontDestroyOnLoad(gameObject);

            registryBecanBall = gameObject.GetComponent<Item>();
            registryBecanBall.SetPrivateField("typeID", 10261026);
            registryBecanBall.SetPrivateField("displayName", "BeaconBall");
            registryBecanBall.Icon = BeaconBallIcon;
            

            //SystemLanguage systemLanguage = LocalizationManager.CurrentLanguage;
            //Todo:多语言支持
            LocalizationManager.overrideTexts.Add("BeaconBall", "信标球");
            LocalizationManager.overrideTexts.Add("BeaconBall_Desc", "一杯自由");
            registryBecanBall.SetPrivateField("weight", 0.0f);
            registryBecanBall.SetPrivateField("quality", -100);
            registryBecanBall.SetPrivateField("maxStackCount", 6);
            registryBecanBall.DisplayQuality = 0;


            Skill_Grenade oldSkill = gameObject.GetComponent<Skill_Grenade>();
            Skill_HelldiverBeaconBall newSkill = gameObject.AddComponent<Skill_HelldiverBeaconBall>();

            newSkill.hasReleaseSound = true;
            newSkill.onReleaseSound = "SFX/Combat/Explosive/throw_grenade";
            newSkill.coolDownTime = 1.0f;


            newSkill.canControlCastDistance = true;
            newSkill.delay = 5;
            newSkill.delayFromCollide = true;



            newSkill.grenadePfb = preFab.GetComponent<Grenade>();
            newSkill.landingBeacon = beaconLanded;
            SkillContext skillContext = oldSkill.SkillContext;
            skillContext.skillReadyTime = 0.1f;
            skillContext.castRange = 20f;
            skillContext.movableWhileAim = true;
            skillContext.effectRange = 10f;
            skillContext.grenageVerticleSpeed = 7f;
            skillContext.isGrenade = true;
            newSkill.SkillContext = skillContext;


            //newSkill.createPickup = oldSkill.createPickup;
            //newSkill.isLandmine = oldSkill.isLandmine;
            //newSkill.landmineTriggerRange = oldSkill.landmineTriggerRange;
            //newSkill.createExplosion = oldSkill.createExplosion;
            //newSkill.canHurtSelf = oldSkill.canHurtSelf;
            //newSkill.explosionShakeStrength = oldSkill.explosionShakeStrength;
            //newSkill.damageInfo = oldSkill.damageInfo;
            //newSkill.blastCount = oldSkill.blastCount;
            //newSkill.blastAngle = oldSkill.blastAngle;
            //newSkill.blastDelayTimeSpace = oldSkill.blastDelayTimeSpace;





            ItemSetting_Skill itemSetting_Skill = gameObject.GetComponent<ItemSetting_Skill>();
            itemSetting_Skill.Skill = newSkill;
            Destroy(oldSkill);

            bool flag = ItemAssetsCollection.AddDynamicEntry(registryBecanBall);
            Debug.Log("信标球创建结果: " + flag);
        }

        void OnDisable()
        {
            harmony.UnpatchAll("Helldiver");
            ItemAssetsCollection.RemoveDynamicEntry(registryBecanBall);
        }
        void Update()
        {
            if (CharacterMainControl.Main == null)
            {
                //Debug.Log("主控制器未加载");
                return;
            }
            if (mainControl == null)
            {
                IsMainBind = false;
            }
            if (!IsMainBind)
            {
                IsMainBind = true;
                mainControl = CharacterMainControl.Main;
                CharacterMainControl.Main.gameObject.AddComponent<AudioListener>();
                Debug.Log("主控制器已加载");
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                ItemUtilities.SendToPlayer(ItemAssetsCollection.InstantiateSync(10261026));
                Debug.Log("信标球已发送到玩家背包");

                Debug.Log(mainControl.CurrentHoldItemAgent.Item.TypeID);//测试信标球是否为当前持有物品
                Debug.Log(mainControl.skillAction.holdItemSkillKeeper.Skill.name);//测试信标球是否为当前持有物品

            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                canBeThrow = !canBeThrow;
            }
            if (Input.GetKeyDown(KeyCode.Y)) 
            {
                StratagemPanel.RegiStratagemToPanel(gameObjectEagle500);

            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Vector3 vector3 = mainControl.characterModel.transform.position;
                Debug.Log(vector3);
                Quaternion rotation = mainControl.characterModel.transform.rotation;
                Debug.Log(rotation);
                vector3.y += 5;
                GameObject temp = Instantiate(eagleOnePrefab, vector3, rotation);
                Debug.Log("实例化飞鹰成功");
                Debug.Log(temp.transform.position);
                Debug.Log(mainControl.transform.position);
                Debug.Log(mainControl.transform.localScale);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                mainControl = CharacterMainControl.Main;
                Debug.Log(mainControl.name);
            }
            if (Input.GetKeyDown(KeyCode.K)) 
            {
                //TODO:加音效，关联起UI和信标球投掷
            }

            InterceptBeaconBall(canBeThrow);

        }

        public static void InterceptBeaconBall(bool canbethrow)
        {
            if (mainControl.skillAction.holdItemSkillKeeper.Skill != null)
            {
                if (mainControl.skillAction.holdItemSkillKeeper.Skill.name == "10261026(Clone)")
                {
                    if (!canbethrow)
                    {
                        mainControl.skillAction.StopAction();

                        Debug.Log("不允许投掷！");
                    }
                    else
                    {
                        Debug.Log("允许投掷！");
                    }
                    
                }
            }
        }

        public void CreateStratagemsPanel()
        {

            Canvas canvas = FindObjectOfType<Canvas>();
            Unit.PrintAllComponents(stratagemsPanelPrefab);

            stratagemPanel.transform.SetParent(canvas.transform, false);
            stratagemPanel.AddComponent<StratagemPanel>();

            RectTransform rect = stratagemPanel.GetComponent<RectTransform>();
            rect.position=panelPositon;
            rect.localScale = panelScale;

            //stratagemPanel.GetComponent<RectTransform>().position = position;
        }
        public static String GetModPath()
        {
            string gameRoot = Path.GetDirectoryName(Application.dataPath);
            return Path.Combine(gameRoot, "Duckov_Data", "Mods", "Helldiver", "Prefab");
        }
        public static void LoadAB()
        {
            if (File.Exists(Path.Combine(dataPath, "beaconball.unity3d")))
            {
                //加载AssetBundle
                tempBeaconBallAB = AssetBundle.LoadFromFile(Path.Combine(dataPath, "beaconball.unity3d"));
                tempUIAB = AssetBundle.LoadFromFile(Path.Combine(dataPath, "ui.unity3d"));
                tempEagleOneAB = AssetBundle.LoadFromFile(Path.Combine(dataPath, "eagleone.unity3d"));
                Instantiate(tempBeaconBallAB, Vector3.zero, Quaternion.identity);
                Instantiate(tempUIAB, Vector3.zero, Quaternion.identity);

                //加载预制体
                //信标球
                preFab = tempBeaconBallAB.LoadAsset<GameObject>("BeaconBall.prefab");
                //信标球落地
                beaconLanded = tempBeaconBallAB.LoadAsset<GameObject>("BeaconBallLanding.prefab");
                //信标球UI面板
                stratagemsPanelPrefab = tempUIAB.LoadAsset<GameObject>("StratagemsPanel.prefab");
                stratagemPanel = Instantiate(stratagemsPanelPrefab, Vector3.zero, Quaternion.identity);
                //飞鹰本体
                eagleOnePrefab = tempEagleOneAB.LoadAsset<GameObject>("EagleOne.prefab");

                //UI图标
                BeaconBallIcon = tempBeaconBallAB.LoadAsset<Sprite>("Beacon_Ball_Icon.png");
                UpArrow = tempUIAB.LoadAsset<Sprite>("Arrow_Up.png");
                DownArrow = tempUIAB.LoadAsset<Sprite>("Arrow_Down.png");
                LeftArrow = tempUIAB.LoadAsset<Sprite>("Arrow_Left.png");
                RightArrow = tempUIAB.LoadAsset<Sprite>("Arrow_Right.png");
                StratagemsIconEageRed = tempUIAB.LoadAsset<Sprite>("Eagle_500kg_Bomb_Stratagem_Edge.png");
                Stratagems500KG=tempUIAB.LoadAsset<Sprite>("Eagle_500kg_Bomb_Stratagem_Icon.png");

            }
            else
            {
                Debug.Log("资源不存在!");
            }
        }
        // 协程：检测手雷是否落地在地形上
        public static IEnumerator CheckGrenadeLanding(Grenade grenade,Quaternion throwBeaconBallRotation)
        {
            while (grenade != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(origin: grenade.transform.position,direction: Vector3.down,hitInfo: out hit,maxDistance: 0.3f ))//maxDistance手雷半径
                {
                    if (hit.collider is TerrainCollider)
                    {
                        Debug.Log(throwBeaconBallRotation);
                        HandleGrenadeLanded(grenade, throwBeaconBallRotation);
                        yield break; // 退出协程
                    }
                }
                else
                {
                }
                yield return null; // 每帧检测一次
            }
        }

        private static void HandleGrenadeLanded(Grenade grenade, Quaternion throwBeaconBallRotation)//触发事件
        {
            if (grenade == null) return;

            // 1. 消除动量（清空速度、停用碰撞）
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.detectCollisions = false; // 停止碰撞检测
                rb.isKinematic = true; // 设为运动学刚体（彻底静止）
            }
            MeshRenderer meshRenderer=grenade.GetComponentInChildren<MeshRenderer>();
            meshRenderer.enabled = false;

            // 3. 创建新预制体（贴合地形表面）
            if (beaconLanded != null)
            {
                Quaternion rotation = Quaternion.identity;
                if (Physics.Raycast(grenade.transform.position,Vector3.down,out RaycastHit terrainHit,0.1f))
                {
                    rotation = Quaternion.FromToRotation(Vector3.up, terrainHit.normal);
                }
                // 实例化预制体
                GameObject beacon= Instantiate(beaconLanded,grenade.transform.position,rotation);
                StratagemReady readyTrigger = beacon.AddComponent<StratagemReady>();
                readyTrigger.stratagemArivalTime= gameObjectEagle500.GetComponent<StratagemInfo>().stratagemArivalTime;
                readyTrigger.EagleOne = eagleOnePrefab;
                Debug.Log(throwBeaconBallRotation);
                readyTrigger.throwBeaconBallRotation = throwBeaconBallRotation;
                readyTrigger.CallingEagle();

            }
        }
        
    }
}
 
