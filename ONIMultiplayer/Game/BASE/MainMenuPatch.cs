using HarmonyLib;
using System;
using JetBrains.Annotations;
using System.Reflection;
using System.Web.UI.WebControls;
using MonoMod.Utils;
using System.Linq;

namespace ONIMultiplayer.Game.BASE

{
    //[UsedImplicitly]
    [HarmonyPatch(typeof(MainMenu))]
    public static class MainMenuPatch
    {
        [HarmonyPatch(typeof(MainMenu), "MakeButton")]
        class MakeButtonPatch
        {
            public static Boolean Prefix(MainMenu __instance,object info)
            {
                FieldInfo Fields = info.GetType().GetField("text",BindingFlags.Instance | BindingFlags.Public);
                Debug.Log("Fields" + Fields.ToString());
                var value = Fields.GetValue(info).ToString();
                Debug.Log("value:"+ value.ToString());
                if (value == STRINGS.UI.FRONTEND.MAINMENU.LOADGAME)
                {
                    Debug.Log("Before LoadGame");
                    Type TMainMenu = AccessTools.TypeByName("MainMenu");
                    Type TButtonInfo = AccessTools.Inner(TMainMenu,"ButtonInfo");
                    Debug.Log(TButtonInfo.ToString());
                    MethodInfo MIMakeButton = AccessTools.Method(MainMenu.Instance.GetType(), "MakeButton");
                    Debug.Log(MIMakeButton.ToString());
                    var normalButtonStyle = TMainMenu.GetField("normalButtonStyle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(__instance);
                    Debug.Log("normalButtonStyle:"+normalButtonStyle.ToString());
                    Type[] NewMultiplay = [typeof(LocString), typeof(System.Action), typeof(int), typeof(ColorStyleSetting)];
                    Type[] LoadMultiplay = [typeof(LocString), typeof(System.Action), typeof(int), typeof(ColorStyleSetting)];
                    Type[] JoinMultiplay = [typeof(LocString), typeof(System.Action), typeof(int), typeof(ColorStyleSetting)];
                    ConstructorInfo CINewMultiplayTParam = TButtonInfo.GetConstructor(NewMultiplay);
                    Debug.Log("CINewMultiplayTParam:" + CINewMultiplayTParam.ToString());
                    ConstructorInfo CILoadMultiplayTParam = TButtonInfo.GetConstructor(LoadMultiplay);
                    Debug.Log("CILoadMultiplayTParam:" + CINewMultiplayTParam.ToString());
                    ConstructorInfo CIJoinMultiplay = TButtonInfo.GetConstructor(JoinMultiplay);
                    Debug.Log("CIJoinMultiplay:" + CINewMultiplayTParam.ToString());
                    object[] NewMultiplayobj = [MODSTRINGS.UI.FRONTEND.MAINMENU.NEWMULTIPLAY, null, 22, normalButtonStyle];
                    object[] LoadMultiplayobj = [MODSTRINGS.UI.FRONTEND.MAINMENU.LOADMULTIPLAY, null, 22, normalButtonStyle];
                    object[] JoinMultiplayobj = [MODSTRINGS.UI.FRONTEND.MAINMENU.JOINMULTIPLAY, null, 22, normalButtonStyle];
                    object NewMultiplayButtonInfo = CINewMultiplayTParam.Invoke(NewMultiplayobj);
                    object LoadMultiplayButtonInfo = CILoadMultiplayTParam.Invoke(LoadMultiplayobj);
                    object JoinMultiplayButtonInfo = CIJoinMultiplay.Invoke(JoinMultiplayobj);
                    Debug.Log("Constructor finished");
                    MIMakeButton.Invoke(__instance,[NewMultiplayButtonInfo]);
                    MIMakeButton.Invoke(__instance, [NewMultiplayButtonInfo]);
                    MIMakeButton.Invoke(__instance, [NewMultiplayButtonInfo]);
                };
                return true;
            }
        }
        
    }
}
