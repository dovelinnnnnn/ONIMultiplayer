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
    // ���ӵ�Ŀ����Ϊ���Ƴ������Ƕδ���
    public static class ProcGenGameWorldGenLoadSettings_Patch
    {
        // �������� Transpiler����ʾ����һ�� Transpiler��������һ�� Patch
        // ����ǩ�����뷵�� IEnumerable<CodeInstruction> ����ILָ���
        // �����б������ IEnumerable<CodeInstruction> instructions
        // ������ͨ�� ILGenerator generator ע���������������ͨ�� MethodBase method ע��ԭʼ��������Ϣ
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // ���ڱ���Ƿ��ҵ��� TooBigCaravanMassUsage
            var foundMassUsageMethod = false;
            // ���ڼ�¼��Ҫ�Ƴ��Ĵ������ʼ�ͽ�������
            var startIndex = -1;
            var endIndex = -1;

            // ͨ������� IL ��ȡ ILcodes
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                // ���� ILcodes�������� ret
                if (codes[i].opcode == OpCodes.Newobj)
                {
                    // ����ҵ����ַ������� endIndex����Ϊ��ǰλ��
                    if (foundMassUsageMethod)
                    {
                        endIndex = i;
                        break;
                    }
                    else
                    {
                        // ���û���ҵ�����startIndex����Ϊ ret ����һ��λ��
                        startIndex = i + 1;

                        // Ȼ��� startIndex ��ʼ�������ң�ֱ���ҵ���һ�� ret
                        for (var j = startIndex; j < codes.Count; j++)
                        {
                            if (codes[j].opcode == OpCodes.Newobj) break;

                            var strOperand = codes[j].operand as string;
                            // �ж���û��Ҫ�ҵ��ַ���
                            if (strOperand == "DisplayClass98_0")
                            {
                                foundMassUsageMethod = true;
                                break;
                            }
                        }
                    }
                }
            }

            // �Ƴ�����
            if (startIndex > -1 && endIndex > -1)
            {
                // Ҫɾ�����벻��ֱ��ɾ����ɾ��ĳ�л���IL�ı�Ż��ң�������ת����ң�����õ��� Nop �ղ���
                codes[startIndex].opcode = OpCodes.Nop;
                codes.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
            }

            // �����޸ĺ��ָ�
            return codes.AsEnumerable();
        }
    }
}
