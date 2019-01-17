using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IllusionPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using System.Collections;

namespace BeatSaberTweaks
{
    class MenuBGVolume : MonoBehaviour
    {
        public static MenuBGVolume Instance;
        
        static SongPreviewPlayer player = null;

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            new GameObject("Menu BG Volume").AddComponent<MenuBGVolume>().transform.parent = parent;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            try
            {
                if (SceneUtils.isMenuScene(scene))
                {
                    StartCoroutine(WaitForLoad());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Tweaks Music Volume error: " + e);
            }
        }

        private IEnumerator WaitForLoad()
        {
            bool loaded = false;
            while (!loaded)
            {
                player = Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault();

                if (player == null)
                {
                    Plugin.Log("Music Volume player is null!", Plugin.LogLevel.DebugOnly);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    Plugin.Log("Music Volume found player!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            UpdateBGVolume();
        }

        public static void UpdateBGVolume()
        {
            Plugin.Log("Updating BG volume", Plugin.LogLevel.DebugOnly);
            if (player != null && SceneUtils.isMenuScene(SceneManager.GetActiveScene()))
            {
                float newVolume = Settings.MenuBGVolume;
                ReflectionUtil.SetPrivateField(player, "_ambientVolumeScale", newVolume);
                player.CrossfadeTo(ReflectionUtil.GetPrivateField<AudioClip>(player, "_defaultAudioClip"), 0f, -1f, newVolume);
            }
        }
    }
}
