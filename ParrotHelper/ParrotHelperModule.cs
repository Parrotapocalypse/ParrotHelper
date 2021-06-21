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
using Celeste.Mod.ParrotHelper.Entities;

namespace Celeste.Mod.ParrotHelper
{
	public class ParrotHelperModule : EverestModule
	{

		// Only one alive module instance can exist at any given time.
		public static ParrotHelperModule Instance;

		public ParrotHelperModule()
		{
			Instance = this;
		}

		// Check the next section for more information about mod settings, save data and session.
		// Those are optional: if you don't need one of those, you can remove it from the module.

		// If you need to store settings:
		//public override Type SettingsType => typeof(ParrotHelperModuleSettings);
		//public static ParrotHelperModuleSettings Settings => (ParrotHelperModuleSettings)Instance._Settings;

		// If you need to store save data:
		//public override Type SaveDataType => typeof(ParrotHelperModuleSaveData);
		//public static ParrotHelperModuleSaveData SaveData => (ParrotHelperModuleSaveData)Instance._SaveData;

		// If you need to store session data:
		//public override Type SessionType => typeof(ParrotHelperModuleSession);
		//public static ParrotHelperModuleSession Session => (ParrotHelperModuleSession)Instance._Session;

		// Set up any hooks, event handlers and your mod in general here.
		// Load runs before Celeste itself has initialized properly.
		public override void Load()
		{
			Logger.SetLogLevel("ParrotHelper", LogLevel.Info);
			Logger.Log(LogLevel.Info, "ParrotHelper", "Working!");
		}

		// Optional, initialize anything after Celeste has initialized itself properly.
		public override void Initialize()
		{
		}

		public static SpriteBank SpriteBank;

		// Optional, do anything requiring either the Celeste or mod content here.
		public override void LoadContent(bool firstLoad)
		{
			SpriteBank = new SpriteBank(GFX.Game, "Graphics/ParrotHelper/ParrotHelperSprites.xml");
		}

		// Unload the entirety of your mod's content. Free up any native resources.
		public override void Unload()
		{
		}
	}
}
