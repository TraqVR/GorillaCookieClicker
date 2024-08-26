using System;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab.ClientModels;
using GorillaCookieClicker.Shop;
using System.Net;
using WebSocketSharp.Net;
using Photon.Pun;
using GorillaCookieClicker.Buttons;

namespace GorillaCookieClicker
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
        public static Plugin Instance { get; set; }

		bool inRoom;
        public static AssetBundle bundle;
        public static GameObject GorillaCookieClicker;
        public static GameObject Cookie;
        public static GameObject NextPage1, BackPage1;
        public static GameObject SetActiveButton;
        public static GameObject HoverZone;
        public static TextMeshPro PlayerText;
        public string PlayerName;
        public static string assetBundleName = "cookieclicker";
        public static string parentName = "GorillaCookieClicker";

        public static GameObject Buy1, Buy2, Buy3, Buy4, Buy5, Buy6, Buy7, Buy8;

        void Awake()
        {
            Instance = this;
        }

        void Start()
		{
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
		}

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized()
		{
            

            bundle = LoadAssetBundle("GorillaCookieClicker.AssetBundles." + assetBundleName);
            GorillaCookieClicker = Instantiate(bundle.LoadAsset<GameObject>(parentName));
            GorillaCookieClicker.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);
            PlayerText = GorillaCookieClicker.transform.Find("parent/Page0/Backround2/PlayersName").GetComponent<TextMeshPro>();
            PlayerName = PhotonNetwork.LocalPlayer.NickName;
            PlayerText.text = PlayerName + "'s Bakery";

            Buy1 = GorillaCookieClicker.transform.Find("parent/Page1/Buttons/Button").gameObject;
            Buy2 = GorillaCookieClicker.transform.Find("parent/Page1/Buttons/Button2").gameObject;
            Buy3 = GorillaCookieClicker.transform.Find("parent/Page1/Buttons/Button3").gameObject;

            Buy4 = GorillaCookieClicker.transform.Find("parent/Page2/Buttons/Button").gameObject;
            Buy5 = GorillaCookieClicker.transform.Find("parent/Page1/Buttons/Button4").gameObject;
            Buy6 = GorillaCookieClicker.transform.Find("parent/Page2/Buttons/Button (2)").gameObject;
            Buy7 = GorillaCookieClicker.transform.Find("parent/Page2/Buttons/Button (3)").gameObject;
            Buy8 = GorillaCookieClicker.transform.Find("parent/Page2/Buttons/Button (1)").gameObject;

            SetActiveButton = GorillaCookieClicker.transform.Find("ToggleActive").gameObject;
            SetActiveButton.AddComponent<SetActive>();
            SetActiveButton.GetComponent<SetActive>().Object = GorillaCookieClicker.transform.Find("parent").gameObject;
            SetActiveButton.GetComponent<SetActive>().setactivetext = GorillaCookieClicker.transform.Find("ToggleActive/text").GetComponent<TextMeshPro>();
            SetActiveButton.layer = 18;

            Cookie = GorillaCookieClicker.transform.Find("parent/cookieclicker").gameObject;
            Cookie.layer = 18;
            Cookie.AddComponent<Cookie>();

            Buy1.AddComponent<BuyMultiplier>();
            Buy1.GetComponent<BuyMultiplier>().Cost = 25;
            Buy1.GetComponent<BuyMultiplier>().Award = 1;
            Buy1.GetComponent<BuyMultiplier>().CostText = GorillaCookieClicker.transform.Find("parent/Page1/Backrounds/Backround6/PlayersName").GetComponent<TextMeshPro>();
            Buy1.layer = 18;

            Buy2.AddComponent<BuyMultiplier>();
            Buy2.GetComponent<BuyMultiplier>().Cost = 100;
            Buy2.GetComponent<BuyMultiplier>().Award = 5;
            Buy2.GetComponent<BuyMultiplier>().CostText = GorillaCookieClicker.transform.Find("parent/Page1/Backrounds/Backround8/PlayersName").GetComponent<TextMeshPro>();
            Buy2.layer = 18;

            Buy3.AddComponent<BuyMultiplier>();
            Buy3.GetComponent<BuyMultiplier>().Cost = 500;
            Buy3.GetComponent<BuyMultiplier>().Award = 25;
            Buy3.GetComponent<BuyMultiplier>().CostText = GorillaCookieClicker.transform.Find("parent/Page1/Backrounds/Backround9/PlayersName").GetComponent<TextMeshPro>();
            Buy3.layer = 18;

            Buy4.AddComponent<BuyPrestige>();
            Buy4.GetComponent<BuyPrestige>().Cost = 1000;
            Buy4.GetComponent<BuyPrestige>().Award = 1;
            Buy4.GetComponent<BuyPrestige>().CostText = GorillaCookieClicker.transform.Find("parent/Page2/Backrounds/Backround6/PlayersName").GetComponent<TextMeshPro>();
            Buy4.layer = 18;

            Buy5.AddComponent<BuyMultiplier>();
            Buy5.GetComponent<BuyMultiplier>().Cost = 1500;
            Buy5.GetComponent<BuyMultiplier>().Award = 100;
            Buy5.GetComponent<BuyMultiplier>().CostText = GorillaCookieClicker.transform.Find("parent/Page1/Backrounds/Backround11/PlayersName").GetComponent<TextMeshPro>();
            Buy5.layer = 18;

            Buy6.AddComponent<BuyPrestige>();
            Buy6.GetComponent<BuyPrestige>().Cost = 4000;
            Buy6.GetComponent<BuyPrestige>().Award = 5;
            Buy6.GetComponent<BuyPrestige>().CostText = GorillaCookieClicker.transform.Find("parent/Page2/Backrounds/Backround6 (2)/PlayersName").GetComponent<TextMeshPro>();
            Buy6.layer = 18;

            Buy7.AddComponent<BuyPrestige>();
            Buy7.GetComponent<BuyPrestige>().Cost = 20000;
            Buy7.GetComponent<BuyPrestige>().Award = 25;
            Buy7.GetComponent<BuyPrestige>().CostText = GorillaCookieClicker.transform.Find("parent/Page2/Backrounds/Backround6 (3)/PlayersName").GetComponent<TextMeshPro>();
            Buy7.layer = 18;

            Buy8.AddComponent<BuyPrestige>();
            Buy8.GetComponent<BuyPrestige>().Cost = 90000;
            Buy8.GetComponent<BuyPrestige>().Award = 100;
            Buy8.GetComponent<BuyPrestige>().CostText = GorillaCookieClicker.transform.Find("parent/Page2/Backrounds/Backround6 (1)/PlayersName").GetComponent<TextMeshPro>();
            Buy8.layer = 18;

            HoverZone = GorillaCookieClicker.transform.Find("parent/HoverZone").gameObject;
            HoverZone.AddComponent<Hover>();
            HoverZone.layer = 18;
            Hover.HoverObject = Cookie;

            NextPage1 = GorillaCookieClicker.transform.Find("parent/Page1/Buttons/NextPage").gameObject;
            NextPage1.layer = 18;
            NextPage1.AddComponent<PageButton>();
            NextPage1.GetComponent<PageButton>().PageToGo = GorillaCookieClicker.transform.Find("parent/Page2").gameObject;
            NextPage1.GetComponent<PageButton>().PageToLeave = GorillaCookieClicker.transform.Find("parent/Page1").gameObject;

            BackPage1 = GorillaCookieClicker.transform.Find("parent/Page2/Buttons/NextPage").gameObject;
            BackPage1.layer = 18;
            BackPage1.AddComponent<PageButton>();
            BackPage1.GetComponent<PageButton>().PageToGo = GorillaCookieClicker.transform.Find("parent/Page1").gameObject;
            BackPage1.GetComponent<PageButton>().PageToLeave = GorillaCookieClicker.transform.Find("parent/Page2").gameObject;
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
