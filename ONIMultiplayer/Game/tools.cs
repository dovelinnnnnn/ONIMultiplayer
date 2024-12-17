using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ONIMultiplayer.Game
{
    internal class Tools
    {
        public static void MainMenuOnPrefabInit_Prefix(ref object __instance, int number, ref int __result)
        {
            __instance.base.OnPrefabInit();
            MainMenu._instance = this;
            this.Button_NewGame = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.NEWGAME, new System.Action(this.NewGame), 22, this.topButtonStyle));
            this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOADGAME, new System.Action(this.LoadGame), 22, this.normalButtonStyle));
            this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.RETIREDCOLONIES, delegate ()
            {
                MainMenu.ActivateRetiredColoniesScreen(this.transform.gameObject, "");
            }, 14, this.normalButtonStyle));
            this.lockerButton = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOCKERMENU, delegate ()
            {
                MainMenu.ActivateLockerMenu();
            }, 14, this.normalButtonStyle));
            if (DistributionPlatform.Initialized)
            {
                this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.TRANSLATIONS, new System.Action(this.Translations), 14, this.normalButtonStyle));
                this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MODS.TITLE, new System.Action(this.Mods), 14, this.normalButtonStyle));
            }
            this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.OPTIONS, new System.Action(this.Options), 14, this.normalButtonStyle));
            this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.QUITTODESKTOP, new System.Action(this.QuitGame), 14, this.normalButtonStyle));
            this.RefreshResumeButton(false);
            this.Button_ResumeGame.onClick += this.ResumeGame;
            this.SpawnVideoScreen();
            this.StartFEAudio();
            this.CheckPlayerPrefsCorruption();
            if (PatchNotesScreen.ShouldShowScreen())
            {
                global::Util.KInstantiateUI(this.patchNotesScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
            }
            this.CheckDoubleBoundKeys();
            bool flag = DistributionPlatform.Inst.IsDLCPurchased("EXPANSION1_ID");
            this.expansion1Toggle.gameObject.SetActive(flag);
            if (this.expansion1Ad != null)
            {
                this.expansion1Ad.gameObject.SetActive(!flag);
            }
            this.RefreshDLCLogos();
            this.motd.Setup();
            if (DistributionPlatform.Initialized && DistributionPlatform.Inst.IsPreviousVersionBranch)
            {
                UnityEngine.Object.Instantiate<GameObject>(ScreenPrefabs.Instance.OldVersionWarningScreen, this.uiCanvas.transform);
            }
            string targetExpansion1AdURL = "";
            Sprite sprite = Assets.GetSprite("expansionPromo_en");
            if (DistributionPlatform.Initialized && this.expansion1Ad != null)
            {
                string name = DistributionPlatform.Inst.Name;
                if (!(name == "Steam"))
                {
                    if (!(name == "Epic"))
                    {
                        if (name == "Rail")
                        {
                            targetExpansion1AdURL = "https://www.wegame.com.cn/store/2001539/";
                            sprite = Assets.GetSprite("expansionPromo_cn");
                        }
                    }
                    else
                    {
                        targetExpansion1AdURL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
                    }
                }
                else
                {
                    targetExpansion1AdURL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
                }
                this.expansion1Ad.GetComponentInChildren<KButton>().onClick += delegate ()
                {
                    App.OpenWebURL(targetExpansion1AdURL);
                };
                this.expansion1Ad.GetComponent<HierarchyReferences>().GetReference<Image>("Image").sprite = sprite;
            }
            this.activateOnSpawn = true;
        }
    }
}
