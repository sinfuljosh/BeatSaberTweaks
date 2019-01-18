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
    class PreviewVolume : MonoBehaviour
    {
        public static PreviewVolume Instance;

        static SongPreviewPlayer songPreviewPlayer = null;

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            new GameObject("Preview Volume").AddComponent<PreviewVolume>().transform.parent = parent;
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
        
        public static void UpdatePreviewVolume()
        {
            Plugin.Log("Updating music volume", Plugin.LogLevel.DebugOnly);
            if (songPreviewPlayer != null && SceneUtils.isMenuScene(SceneManager.GetActiveScene()))
            {
                float newVolume = Settings.PreviewVolume;
                songPreviewPlayer.volume = newVolume;
            
                // While this is the corssade source, this is also the menu background music volume!
                //ReflectionUtil.SetPrivateField(songPreviewPlayer, "_ambientVolumeScale", Settings.PreviewVolume);
                Plugin.Log("Setting preview volume to " + newVolume, Plugin.LogLevel.DebugOnly);
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
                Console.WriteLine("Tweaks Preview Volume error: " + e);
            }
        }

        private IEnumerator WaitForLoad()
        {
            bool loaded = false;
            while (!loaded)
            {
                songPreviewPlayer = Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault();

                if (songPreviewPlayer == null)
                {
                    Plugin.Log("Preview Volume songPreviewPlayer is null!", Plugin.LogLevel.DebugOnly);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    Plugin.Log("Preview Volume found songPreviewPlayer!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            UpdatePreviewVolume();
        }
    }
}