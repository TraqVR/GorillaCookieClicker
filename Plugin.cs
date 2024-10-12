using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using Utilla;
using TMPro;
using Photon.Pun;

namespace GorillaCookieClicker
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static AssetBundle bundle;
        public static GameObject GorillaCookieClicker;
        public static GameObject Cookie;
        public static GameObject SetActiveButton;
        public static GameObject HoverZone;
        public static TextMeshPro PlayerText;
        public string PlayerName;
        public static string assetBundleName = "cookieclicker";
        public static string parentName = "GorillaCookieClicker";

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            instance = this;
            bundle = LoadAssetBundle("GorillaCookieClicker.AssetBundles." + assetBundleName);
            GorillaCookieClicker = Instantiate(bundle.LoadAsset<GameObject>(parentName));
            GorillaCookieClicker.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);

            PlayerName = PhotonNetwork.LocalPlayer.NickName;
            PlayerText = GameObject.Find("GorillaCookieClicker(Clone)/parent/Page0/Backround2/PlayersName").GetComponent<TextMeshPro>();
            PlayerText.text = PlayerName + "'s Bakery";

            SetActiveButton = GameObject.Find("GorillaCookieClicker(Clone)/ToggleActive");
            var setActiveComponent = SetActiveButton.AddComponent<SetActive>();
            setActiveComponent.Object = GameObject.Find("GorillaCookieClicker(Clone)/parent");
            setActiveComponent.setactivetext = GameObject.Find("GorillaCookieClicker(Clone)/ToggleActive/text").GetComponent<TextMeshPro>();
            SetActiveButton.layer = 18;

            Cookie = GameObject.Find("GorillaCookieClicker(Clone)/parent/cookieclicker");
            Cookie.layer = 18;
            Cookie.AddComponent<Cookie>();

            HoverZone = GameObject.Find("GorillaCookieClicker(Clone)/parent/HoverZone");
            HoverZone.layer = 18;
            var hoverComponent = HoverZone.AddComponent<Hover>();
            Hover.HoverObject = Cookie;

            SetupPageButton("GorillaCookieClicker(Clone)/parent/Page1/Buttons/NextPage", "GorillaCookieClicker(Clone)/parent/Page2", "GorillaCookieClicker(Clone)/parent/Page1");
            SetupPageButton("GorillaCookieClicker(Clone)/parent/Page2/Buttons/NextPage", "GorillaCookieClicker(Clone)/parent/Page1", "GorillaCookieClicker(Clone)/parent/Page2");

            var buyButtons = new List<BuyButtonInfo>
            {
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page1/Buttons/Button",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page1/Backrounds/Backround6/PlayersName",
                    Cost = 25,
                    Award = 1,
                    IsPrestige = false
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page1/Buttons/Button2",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page1/Backrounds/Backround8/PlayersName",
                    Cost = 100,
                    Award = 5,
                    IsPrestige = false
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page1/Buttons/Button3",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page1/Backrounds/Backround9/PlayersName",
                    Cost = 500,
                    Award = 25,
                    IsPrestige = false
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page2/Buttons/Button",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page2/Backrounds/Backround6/PlayersName",
                    Cost = 1000,
                    Award = 1,
                    IsPrestige = true
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page1/Buttons/Button4",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page1/Backrounds/Backround11/PlayersName",
                    Cost = 1500,
                    Award = 100,
                    IsPrestige = false
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page2/Buttons/Button (2)",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page2/Backrounds/Backround6 (2)/PlayersName",
                    Cost = 4000,
                    Award = 5,
                    IsPrestige = true
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page2/Buttons/Button (3)",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page2/Backrounds/Backround6 (3)/PlayersName",
                    Cost = 20000,
                    Award = 25,
                    IsPrestige = true
                },
                new BuyButtonInfo
                {
                    ButtonPath = "GorillaCookieClicker(Clone)/parent/Page2/Buttons/Button (1)",
                    CostTextPath = "GorillaCookieClicker(Clone)/parent/Page2/Backrounds/Backround6 (1)/PlayersName",
                    Cost = 90000,
                    Award = 100,
                    IsPrestige = true
                }
            };

            foreach (var buyButtonInfo in buyButtons)
            {
                GameObject button = GameObject.Find(buyButtonInfo.ButtonPath);
                button.layer = 18;

                if (buyButtonInfo.IsPrestige)
                {
                    var buyPrestige = button.AddComponent<BuyPrestige>();
                    buyPrestige.Cost = buyButtonInfo.Cost;
                    buyPrestige.Award = buyButtonInfo.Award;
                    buyPrestige.CostText = GameObject.Find(buyButtonInfo.CostTextPath).GetComponent<TextMeshPro>();
                }
                else
                {
                    var buyMultiplier = button.AddComponent<BuyMultiplier>();
                    buyMultiplier.Cost = buyButtonInfo.Cost;
                    buyMultiplier.Award = buyButtonInfo.Award;
                    buyMultiplier.CostText = GameObject.Find(buyButtonInfo.CostTextPath).GetComponent<TextMeshPro>();
                }
            }
        }

        private void SetupPageButton(string buttonPath, string pageToGoPath, string pageToLeavePath)
        {
            GameObject pageButton = GameObject.Find(buttonPath);
            pageButton.layer = 18;
            var pageButtonComponent = pageButton.AddComponent<PageButton>();
            pageButtonComponent.PageToGo = GameObject.Find(pageToGoPath);
            pageButtonComponent.PageToLeave = GameObject.Find(pageToLeavePath);
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                return AssetBundle.LoadFromStream(stream);
            }
        }

        class BuyButtonInfo
        {
            public string ButtonPath;
            public string CostTextPath;
            public int Cost;
            public int Award;
            public bool IsPrestige;
        }
    }
}
