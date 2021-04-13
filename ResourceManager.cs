using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using MelonLoader;
using System.Collections;
using System.IO;
using System.Reflection;

namespace ActiveBackground
{
    class ResourceManager
    {
		private static AssetBundle Bundle;
        public static Material BlurGoBrrr, BlurLite;

        private static Material LoadMateral(string assetToLoad)
        {
            Material loadedMaterial = Bundle.LoadAsset_Internal(assetToLoad, Il2CppType.Of<Material>()).Cast<Material>();
            loadedMaterial.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return loadedMaterial;
        }

        public static void Init() { MelonCoroutines.Start(LoadResources()); }

		private static IEnumerator LoadResources()
        {
            // Came from UIExpansionKit (https://github.com/knah/VRCMods/blob/master/UIExpansionKit)
            MelonLogger.Msg("Loading AssetBundle...");
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ActiveBackground.activebackground.ab"))
            {
                using (var memoryStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(memoryStream);
                    Bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
                    Bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    try { BlurGoBrrr = LoadMateral("BlurGoBrr.mat"); } catch { MelonLogger.Error("Custom Material failed"); }
                    try { BlurLite = LoadMateral("Blur Lite.mat"); } catch { MelonLogger.Error("Alt Custom Material failed"); }
                }
            }

            if (Main.isDebug)
                MelonLogger.Msg("Finihsed with Asset Bundle Resource Managment");
            yield break;
        }
	}
}
