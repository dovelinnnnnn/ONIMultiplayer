using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace ONIMultiplayer.Game.UI
{
[HarmonyPatch]
   
    internal class MainMenuPatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(MainMenu), "OnPrefabInit")]
        public static void MainMenuOnPrefabInit_Transpiler(object __instance,ref int __result)
        {

            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = Transpilers.Manipulator(instructions,
                    item => item.opcode == OpCodes.Ldarg_1,
                    item => item.opcode = OpCodes.Ldarg_0
                ).ToList();
                var mJoin = SymbolExtensions.GetMethodInfo(() => string.Join(null, null));
                var idx = list.FindIndex(item => item.opcode == OpCodes.Call && item.operand as MethodInfo == mJoin);
                list.RemoveRange(idx + 1, list.Count - (idx + 1));
                return list.AsEnumerable();
            }

        }
    }
}
