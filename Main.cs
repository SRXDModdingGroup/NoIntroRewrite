using UnityEngine;
using UnityEngine.SceneManagement;
using BepInEx;
using BepInEx.IL2CPP;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;

namespace NoIntroRewrite
{
    [BepInPlugin("NoIntroRewrite", "NoIntroRewrite", "1.0.0")]
    public class Main : BasePlugin
    {
        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(SkipPatches));
        }
        
        class SkipPatches
        {
            private static bool startedUp = false;
            
            [HarmonyPatch(typeof(StartupScene), nameof(StartupScene.Update))]
            [HarmonyPostfix]
            private static void Update_Postfix()
            {
                if (!startedUp)
                {
                    Scene activeScene = SceneManager.GetActiveScene();
                    if (activeScene.name == "XDStartupScene")
                    {
                        GameObject val = GameObject.Find("StartupObject");
                        GameObject val2 = GameObject.Find("XDTitleLogo(Clone)");
                        if (val && val2)
                        {
                            startedUp = true;
                            StartupScene component = val.GetComponent<StartupScene>();
                            val2.SetActive(false);
                            component.animationTimer = 8f;
                        }
                    }
                }
            }
        }
    }
}