using MelonLoader;
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;
using UIExpansionKit.API;
using System.Windows.Forms;
using UnityEngine.XR;
using VR = UnityEngine.XR.XRDevice;

namespace ActiveBackground
{
    public static class BuildInfo
    {
        public const string Name = "ActiveBackground";
        public const string Author = "Lily";
        public const string Company = null;
        public const string Version = "1.0.1";
        public const string DownloadLink = "https://github.com/MintLily/ActiveBackground";
        public const string Description = "Add an Active Blur to your menu's background.";
    }

    public class Main : MelonMod
    {
        public static bool isDebug;
        public MelonPreferences_Category melon;
        public MelonPreferences_Entry<bool> enabled, altBlur;

        private GameObject UiBackground, QMBackground, ContextUiBackground, ContextUiBackground2, ContextUiBackground3, InfoBarBackground;
        private Material OriginalQMMat, OriginalUiMat, OriginalContextUiMat, OriginalContextUiMat2, OriginalContextUiMat3, OriginalInfoBarMat;

        public override void OnApplicationStart() // Runs after Game Initialization.
        {
            if (MelonDebug.IsEnabled() || Environment.CommandLine.Contains("--ab.debug"))
            {
                isDebug = true;
                MelonLogger.Msg("Debug mode is active");
            }

            if (typeof(MelonMod).GetMethod("VRChat_OnUiManagerInit") == null)
                MelonCoroutines.Start(GetAssembly());

            melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            enabled = (MelonPreferences_Entry<bool>)melon.CreateEntry("enabled", true, "Apply Background Blur Effects");
            altBlur = (MelonPreferences_Entry<bool>)melon.CreateEntry("useAlternativeBlur", false, "Use alternate blur (VR Blur Shader)\n(No affect in VR)");
            ResourceManager.Init();
            MelonLogger.Msg("Initialized!");
        }

        private void OnUiManagerInit() => MelonCoroutines.Start(DelayedAction());

        public override void OnPreferencesSaved() => MelonCoroutines.Start(Setup());

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
            ContextUiBackground3 = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_ToolTip/Panel");
            InfoBarBackground = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar/Panel");

            OriginalQMMat = QMBackground.GetComponent<Image>().material;
            OriginalUiMat = UiBackground.GetComponent<Image>().material;
            OriginalContextUiMat = ContextUiBackground.GetComponent<Image>().material;
            OriginalContextUiMat2 = ContextUiBackground2.GetComponent<Image>().material;
            OriginalContextUiMat3 = ContextUiBackground3.GetComponent<Image>().material;
            OriginalInfoBarMat = InfoBarBackground.GetComponent<Image>().material;
            yield break;
        }
        
        private IEnumerator Setup()
        {
            yield return new WaitForSeconds(0.5f);
            if (enabled.Value) {
                if (altBlur.Value || VR.isPresent) {
                    UiBackground.GetComponent<Image>().material = ResourceManager.BlurLite;
                    QMBackground.GetComponent<Image>().material = ResourceManager.BlurLite;
                    ContextUiBackground.GetComponent<Image>().material = ResourceManager.BlurLite;
                    ContextUiBackground2.GetComponent<Image>().material = ResourceManager.BlurLite;
                    ContextUiBackground3.GetComponent<Image>().material = ResourceManager.BlurLite;
                    InfoBarBackground.GetComponent<Image>().material = ResourceManager.BlurLite;
                }
                else {
                    UiBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                    QMBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                    ContextUiBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                    ContextUiBackground2.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                    ContextUiBackground3.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                    InfoBarBackground.GetComponent<Image>().material = ResourceManager.BlurGoBrrr;
                }
            }
            else {
                QMBackground.GetComponent<Image>().material = OriginalQMMat;
                UiBackground.GetComponent<Image>().material = OriginalUiMat;
                ContextUiBackground.GetComponent<Image>().material = OriginalContextUiMat;
                ContextUiBackground2.GetComponent<Image>().material = OriginalContextUiMat2;
                ContextUiBackground3.GetComponent<Image>().material = OriginalContextUiMat3;
                InfoBarBackground.GetComponent<Image>().material = OriginalInfoBarMat;
            }
            yield break;
        }

        private IEnumerator GetAssembly()
        {
            Assembly assemblyCSharp = null;
            while (true) {
                assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
                if (assemblyCSharp == null)
                    yield return null;
                else
                    break;
            }

            MelonCoroutines.Start(WaitForUiManagerInit(assemblyCSharp));
        }

        private IEnumerator WaitForUiManagerInit(Assembly assemblyCSharp)
        {
            Type vrcUiManager = assemblyCSharp.GetType("VRCUiManager");
            PropertyInfo uiManagerSingleton = vrcUiManager.GetProperties().First(pi => pi.PropertyType == vrcUiManager);
            while (uiManagerSingleton.GetValue(null) == null)
                yield return null;
            OnUiManagerInit();
        }
    }
}