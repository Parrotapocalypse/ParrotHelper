using Monocle;

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
		}

		// Optional, initialize anything after Celeste has initialized itself properly.
		public override void Initialize()
		{
		}

		public static SpriteBank SpriteBank;
		public static SpriteBank GuiSpriteBank;

		// Optional, do anything requiring either the Celeste or mod content here.
		public override void LoadContent(bool firstLoad)
		{
			SpriteBank = new SpriteBank(GFX.Game, "Graphics/ParrotHelper/ParrotHelperSprites.xml");
			GuiSpriteBank = new SpriteBank(GFX.Gui, "Graphics/ParrotHelper/ParrotHelperGuiSprites.xml");
		}

		// Unload the entirety of your mod's content. Free up any native resources.
		public override void Unload()
		{
		}
	}
}
