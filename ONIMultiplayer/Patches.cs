using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Reflection;
using static DistributionPlatform;
namespace ONIMultiplayer.Game.UI
{
    namespace ONIMultiplayer
    {

        public class ONIMultiplayerMod : UserMod2 {

            public override void OnLoad(Harmony harmony)
            {
                // do some stuff before patching
                Harmony.DEBUG = true;
                Debug.Log("OnLoad: Before patches!");

                // let the game patch everything
                base.OnLoad(harmony);

                // do some stuff after patching
                Debug.Log("OnLoad: After patches!");
            }
            public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
            {
                foreach (var mod in mods)
                {
                    // do some stuff
                    Debug.Log("found mod: " + mod.title);
                }
            }
        }
    }
}