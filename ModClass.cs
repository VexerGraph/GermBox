using GermBox.Content;
using GermBox.UI;
using NeoModLoader.api;

namespace GermBox
{
    public class ModClass : BasicMod<ModClass>
    {
        protected override void OnModLoad()
        {
            var harmony = new HarmonyLib.Harmony("com.github.vexergraph.germbox");
            harmony.PatchAll();
            Icons.Init();
            Drops.Init();
            GodPowers.Init();
            TabButton.Init();
            //LogInfo(BasicMod<ModClass>.Instance.GetDeclaration().FolderPath);
        }
    }
}