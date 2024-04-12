using BepInEx;
using MyBox;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoreBanner
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class StoreBanner : BaseUnityPlugin
    {
        private bool enabled = true;
        private Texture2D bannerTex;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (!Directory.Exists(@"BepInEx\plugins\StoreBanner"))
            {
                Logger.LogError("StoreBanner folder not found. You have not installed all files correctly, the mod will not function correctly.");
                enabled = false;
                return;
            }

            if (!File.Exists(@"BepInEx\plugins\StoreBanner\texBanner.png"))
            {
                Logger.LogError("texBanner.png not found. You have not installed all files correctly, the mod will not function correctly.");
                enabled = false;
                return;
            }

            Texture2D tex2D = new Texture2D(800, 100);
            byte[] imgBytes = File.ReadAllBytes(@"BepInEx\plugins\StoreBanner\texBanner.png");
            ImageConversion.LoadImage(tex2D, imgBytes);
            bannerTex = tex2D;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForFullyInit());
        }

        IEnumerator WaitForFullyInit()
        {
            GameObject signCanvas = null;
            
            yield return new WaitUntil(() =>
            {
                try
                {
                    signCanvas = GameObject.Find("---GAME---").transform.Find("Store").transform.Find("Store").transform.Find("Sections").transform.Find("Section 2").transform.Find("--- To be enabled ---  ").transform.Find("Table Canvas").gameObject;
                }
                catch (NullReferenceException ex)
                {
                    return false;
                }

                return true;
            }); 
            

            if (enabled)
            {
                UnityEngine.UI.Image uiImage = signCanvas.transform.Find("BG").GetComponent<UnityEngine.UI.Image>();
                uiImage.sprite = Sprite.Create(bannerTex, new Rect(0, 0, 800, 100), new Vector2(400, 50));

                signCanvas.transform.Find("Title").gameObject.SetActive(false); // Disable text
            }
        }

        
    }
}
