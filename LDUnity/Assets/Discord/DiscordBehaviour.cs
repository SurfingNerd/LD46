using System;
using UnityEngine;

public class DiscordBehaviour : ManagerBase {
	//private Discord.Discord discord;
	public override void InitManager() {
		//base.InitManager();
		//discord = new Discord.Discord(701762155967545355, (UInt64)Discord.CreateFlags.NoRequireDiscord);//Default
		//var activity = new Discord.Activity {
		//	Details = "The things we do for love...",
		//	Assets = { LargeImage = "icon" },
		//	Instance = true
		//};

		//discord?.GetActivityManager()?.UpdateActivity(activity, (result) =>
		//{
		//	if (result == Discord.Result.Ok) Debug.Log("Discord: Success!");
		//	else Debug.Log("Discord: Failed");
		//});
	}
	void Update(){
		//discord?.RunCallbacks();
	}
	void OnApplicationQuit(){
		//discord?.Dispose();
	}
}
