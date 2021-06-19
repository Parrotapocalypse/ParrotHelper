using Celeste.Mod.UI;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;
using Celeste;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste.Mod.FriendlyHelper.Entities;

namespace Celeste.Mod.FriendlyHelper
{
	public class FriendlyHelperModule : EverestModule
	{

		// Only one alive module instance can exist at any given time.
		public static FriendlyHelperModule Instance;

		public FriendlyHelperModule()
		{
			Instance = this;
		}

		// Check the next section for more information about mod settings, save data and session.
		// Those are optional: if you don't need one of those, you can remove it from the module.

		// If you need to store settings:
		//public override Type SettingsType => typeof(FriendlyHelperModuleSettings);
		//public static FriendlyHelperModuleSettings Settings => (FriendlyHelperModuleSettings)Instance._Settings;

		// If you need to store save data:
		//public override Type SaveDataType => typeof(FriendlyHelperModuleSaveData);
		//public static FriendlyHelperModuleSaveData SaveData => (FriendlyHelperModuleSaveData)Instance._SaveData;

		// If you need to store session data:
		//public override Type SessionType => typeof(FriendlyHelperModuleSession);
		//public static FriendlyHelperModuleSession Session => (FriendlyHelperModuleSession)Instance._Session;

		// Set up any hooks, event handlers and your mod in general here.
		// Load runs before Celeste itself has initialized properly.
		public override void Load()
		{
			Logger.SetLogLevel("FriendlyHelper", LogLevel.Info);
			Logger.Log(LogLevel.Info, "FriendlyHelper", "Applying gradient dust with path: ");
		}

		// Optional, initialize anything after Celeste has initialized itself properly.
		public override void Initialize()
		{
		}

		// Optional, do anything requiring either the Celeste or mod content here.
		public override void LoadContent(bool firstLoad)
		{
		}

		// Unload the entirety of your mod's content. Free up any native resources.
		public override void Unload()
		{
		}

	}
}
