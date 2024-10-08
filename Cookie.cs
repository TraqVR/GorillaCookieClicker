﻿using System;
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
            CookieParticles = GameObject.Find("GorillaCookieClicker(Clone)/parent/cookieclicker/ClickParticle").GetComponent<ParticleSystem>();
            ClicksText = GameObject.Find("GorillaCookieClicker(Clone)/parent/Page0/Backround3/ClickCount").GetComponent<TMP_Text>();
            MultiplierText = GameObject.Find("GorillaCookieClicker(Clone)/parent/Page0/Backround4/Multipliers").GetComponent<TMP_Text>();
            PrestigesText = GameObject.Find("GorillaCookieClicker(Clone)/parent/Page0/Backround5/Prestiges").GetComponent<TMP_Text>();
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
        }
    }
}
