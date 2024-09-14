using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace GorillaCookieClicker
{
    public class Cookie : MonoBehaviour
    {
        public int Multipier = 1;
        public int Cookies;
        public int Prestiges;
        public int FakePrestiges;

        public static ParticleSystem CookieParticles;
        public static TMP_Text ClicksText;
        public static TMP_Text MultiplierText;
        public static TMP_Text PrestigesText;
        public bool CanClick = true;

        void Start()
        {
            Cookies = PlayerPrefs.GetInt("Cookies", 0);
            Multipier = PlayerPrefs.GetInt("Multiplier", 1);
            Prestiges = PlayerPrefs.GetInt("Multiplier", 0);

            CookieParticles = Plugin.GorillaCookieClicker.transform.Find("parent/cookieclicker/ClickParticle").GetComponent<ParticleSystem>();
            ClicksText = Plugin.GorillaCookieClicker.transform.Find("parent/Page0/Backround3/ClickCount").GetComponent<TMP_Text>();
            MultiplierText = Plugin.GorillaCookieClicker.transform.Find("parent/Page0/Backround4/Multipliers").GetComponent<TMP_Text>();
            PrestigesText = Plugin.GorillaCookieClicker.transform.Find("parent/Page0/Backround5/Prestiges").GetComponent<TMP_Text>();

            StartCoroutine(CookieClick());
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CanClick)
            {
                if (other.name == "RightHandTriggerCollider")
                {
                    StartCoroutine(CookieClick());
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, false, 0.1f);
                    GorillaTagger.Instance.StartVibration(false, .1f, 0.001f);
                }
                else if (other.name == "LeftHandTriggerCollider")
                {
                    StartCoroutine(CookieClick());
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, true, 0.1f);
                    GorillaTagger.Instance.StartVibration(true, .1f, 0.001f);
                }
            }
        }

        IEnumerator CookieClick()
        {
            CanClick = false;

            //FakePrestiges = Prestiges + 1;

            if (Prestiges == 0)
            {
                Cookies += Multipier;
            }
            else
            {
                Cookies += Multipier * (Prestiges + 1);
            }


            if (CookieParticles.isPlaying)
            {
                CookieParticles.Stop();
                CookieParticles.Play();
            }
            else if (!CookieParticles.isPlaying)
            {
                CookieParticles.Play();
            }

            UpdateText();

            yield return new WaitForSeconds(0.1f);
            CanClick = true;
        }

        public void UpdateText()
        {
            ClicksText.text = "Cookies: " + Cookies.ToString();
            MultiplierText.text = "Multipier: " + Multipier.ToString();
            PrestigesText.text = "Prestiges: " + Prestiges.ToString();
            PlayerPrefs.SetInt("Cookies", Cookies);
            PlayerPrefs.SetInt("Multiplier", Multipier);
            PlayerPrefs.SetInt("Prestiges", Prestiges);
        }
    }
}
