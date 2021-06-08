using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste;
using Celeste.Mod.FriendlyHelper.Entities;

namespace Celeste.Mod.FriendlyHelper
{
    public class FriendlyHelperModule : EverestModule
    {
        public static FriendlyHelperModule Instance;

        public FriendlyHelperModule()
        {
            Instance = this;
        }
        public override void Load()
        {
            CustomHeart.Load();
        }

        public override void Unload()
        {
            CustomHeart.Unload();
        }
    }
}
