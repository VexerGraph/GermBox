using GermBox.Content;
using GermBox.UI;
using NeoModLoader.api;

namespace GermBox
{
    public class ModClass : BasicMod<ModClass>
    {
        protected override void OnModLoad()
        {
            Drops.Init();
            GodPowers.Init();
            TabButton.Init();
        }
    }
}