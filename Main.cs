using MelonLoader;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UIExpansionKit.API;
using System.Windows.Forms;

namespace ActiveBackground
{
    public static class BuildInfo
    {
        public const string Name = "ActiveBackground";
        public const string Author = "Korty (Lily)";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/KortyBoi/ActiveBackground";
        public const string Description = "Add Active blur to your menu backgrounds.";
    }

    public class Main : MelonMod
    {
        public static bool isDebug;
        public MelonPreferences_Category melon;
        public MelonPreferences_Entry<bool> enabled;

        private GameObject UiBackground, QMBackground, ContextUiBackground, ContextUiBackground2, ContextUiBackground3, ContextUiBackground4, InfoBarBackground;
        private Material OriginalQMMat, OriginalUiMat, OriginalContextUiMat, OriginalContextUiMat2, OriginalContextUiMat3, OriginalContextUiMat4, OriginalInfoBarMat;

        public override void OnApplicationStart() // Runs after Game Initialization.
        {
            if (MelonDebug.IsEnabled())
            {
                isDebug = true;
                MelonLogger.Msg("Debug mode is active");
            }
            
            melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            enabled = (MelonPreferences_Entry<bool>)melon.CreateEntry("enambed", true, "Apply Background Blur Effects");

            MelonLogger.Msg("Initialized!");
        }

        public override void VRChat_OnUiManagerInit()
        {
            ResourceManager.Init();

            MelonCoroutines.Start(DelayedAction());
        }

        public override void OnPreferencesSaved() { MelonCoroutines.Start(Setup()); }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            switch (buildIndex)
            {
                case 0:
                case 1:
                    break;
                default:
                    MelonCoroutines.Start(Setup());
                    break;
            }
        }

        private IEnumerator DelayedAction()
        {
            yield return new WaitForSeconds(2f);
            UiBackground = GameObject.Find("UserInterface/MenuContent/Backdrop/Backdrop/Background");
            QMBackground = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_Background/Panel");
            ContextUiBackground = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/Panel");
            ContextUiBackground2 = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/Panel");
            ContextUiBackground3 = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/_UserStatus");
            ContextUiBackground4 = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_ToolTip/Panel");
            InfoBarBackground = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar/Panel");

            OriginalQMMat = QMBackground.GetComponent<Image>().material;
            OriginalUiMat = UiBackground.GetComponent<Image>().material;
            OriginalContextUiMat = ContextUiBackground.GetComponent<Image>().material;
            OriginalContextUiMat2 = ContextUiBackground2.GetComponent<Image>().material;
            OriginalContextUiMat3 = ContextUiBackground3.GetComponent<Image>().material;
            OriginalContextUiMat4 = ContextUiBackground4.GetComponent<Image>().material;
            OriginalInfoBarMat = InfoBarBackground.GetComponent<Image>().material;
            yield break;
        }

        private IEnumerator Setup()
        {
            yield return new WaitForSeconds(1f);
            if (enabled.Value)
            {
                UiBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                QMBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                ContextUiBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                ContextUiBackground2.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                ContextUiBackground3.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                ContextUiBackground4.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                InfoBarBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
            }
            else
            {
                QMBackground.GetComponent<Image>().material = OriginalQMMat;
                UiBackground.GetComponent<Image>().material = OriginalUiMat;
                ContextUiBackground.GetComponent<Image>().material = OriginalContextUiMat;
                ContextUiBackground2.GetComponent<Image>().material = OriginalContextUiMat2;
                ContextUiBackground3.GetComponent<Image>().material = OriginalContextUiMat3;
                ContextUiBackground4.GetComponent<Image>().material = OriginalContextUiMat4;
                InfoBarBackground.GetComponent<Image>().material = OriginalInfoBarMat;
            }
            yield break;
        }
    }
}