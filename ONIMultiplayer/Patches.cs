using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace ONIMultiplayer
{
    public class Patches
    {
        //[HarmonyPatch(typeof(Db))]
        //[HarmonyPatch("Initialize")]
        //public class Db_Initialize_Patch
        //{
        //    public static void Prefix()
        //    {
        //        Debug.Log("I execute before Db.Initialize!");
        //    }

        //    public static void Postfix()
        //    {
        //        Debug.Log("I execute after Db.Initialize!");
        //    }
        //}
        
    }
    [HarmonyPatch(typeof(ProcGenGame.WorldGen), nameof(ProcGenGame.WorldGen.LoadSettings))]
    // 例子的目的是为了移除上面那段代码
    public static class ProcGenGameWorldGenLoadSettings_Patch
    {
        // 方法名是 Transpiler，表示这是一个 Transpiler，而不是一个 Patch
        // 方法签名必须返回 IEnumerable<CodeInstruction> 这是IL指令集合
        // 参数中必须包含 IEnumerable<CodeInstruction> instructions
        // 还可以通过 ILGenerator generator 注入代码生成器，或通过 MethodBase method 注入原始方法的信息
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // 用于标记是否找到了 TooBigCaravanMassUsage
            var foundMassUsageMethod = false;
            // 用于记录需要移除的代码的起始和结束索引
            var startIndex = -1;
            var endIndex = -1;

            // 通过传入的 IL 获取 ILcodes
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                // 遍历 ILcodes，并查找 ret
                if (codes[i].opcode == OpCodes.Newobj)
                {
                    // 如果找到了字符串，则 endIndex更改为当前位置
                    if (foundMassUsageMethod)
                    {
                        endIndex = i;
                        break;
                    }
                    else
                    {
                        // 如果没有找到，将startIndex设置为 ret 的下一个位置
                        startIndex = i + 1;

                        // 然后从 startIndex 开始继续查找，直到找到下一个 ret
                        for (var j = startIndex; j < codes.Count; j++)
                        {
                            if (codes[j].opcode == OpCodes.Newobj) break;

                            var strOperand = codes[j].operand as string;
                            // 判定有没有要找的字符串
                            if (strOperand == "DisplayClass98_0")
                            {
                                foundMassUsageMethod = true;
                                break;
                            }
                        }
                    }
                }
            }

            // 移除代码
            if (startIndex > -1 && endIndex > -1)
            {
                // 要删除代码不能直接删除，删除某行会让IL的编号混乱，其他跳转会混乱，因此用的是 Nop 空操作
                codes[startIndex].opcode = OpCodes.Nop;
                codes.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
            }

            // 返回修改后的指令集
            return codes.AsEnumerable();
        }
    }
}
