// ============================================ //
//                CityRPG Plus                  //
// CityRPG Plus is a standalone expansion for   //
// the CityRPG game-mode, compatible with most  //
// standard variants of CityRPG.                //
//                                              //
// Author: Lake Y (BLID: 14128)                 //
// ============================================ //

// ================================== //
// # Init #
// ================================== //
if($CityRPGPlus::Init && !$CityRPGPlus::ForceReload)
{
	echo("CityRPG Plus already loaded! Aborting...");
	return;
}
$CityRPGPlus::Init = 1;

// If CityRPG is not alreay enabled, try to enable the first one we can find.
// May execute the wrong one if there are multiple copies installed.
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_CityRPG");
if(!isObject(CityRPGData))
  forceRequiredAddOn("GameMode_CityRPG4");
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_TysCityRPG");
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_JJStormsCityRPG");

// If the game-mode still doesn't exist, abort with a catch-all message.
if(!isObject(CityRPGData)) {
  // Reset init to 0 in case a custom branch re-runs the add-on.
  $CityRPGPlus::Init = 0;

  error("Support_CityRPG_Plus - Unable to load a compatible version of CityRPG. This add-on requires CityRPG in order to run."
	NL "If you are writing a custom CityRPG branch, have your mod detect and execute CityRPG Plus (forceRequiredAddOn won't work) after the game-mode initializes. Avoid renaming the CityRPGData object.");
	return;
}

exec("./prefs.cs");

// ================================== //
// # Package #
// ================================== //
package CityRPGPlus
{
	// ================================== //
	// ## Security ##
	// ================================== //

	// This overwrites commands for known exploits.
	// Most common versions of CityRPG have these patched out. This is simply an extra precaution.

	// The following were found in the original version of /Ty's CityRPG.
	// See here: https://forum.blockland.us/index.php?topic=320447
  function serverCmdLootDrugs(%client) {
    if(isFunction(GameConnection, CRPGLog))
      %client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /lootDrugs");
  }

  function serverCmdeSet(%client) {
    if(isFunction(GameConnection, CRPGLog))
      %client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /eSet");
  }

  function serverCmdrt(%client) {
    if(isFunction(GameConnection, CRPGLog))
      %client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /rt");
  }

  function serverCmdfit(%client) {
    if(isFunction(GameConnection, CRPGLog))
      %client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /fit");
  }

	// The following was found in Iban's CityRPG, as well as several edited versions.
	// This one is overwritten by the game iteslf in newer versions of BL, but it's
	// still better to be extra safe.
	function serverCmdtogadmin(%client) {
    if(isFunction(GameConnection, CRPGLog))
      %client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /togadmin");
  }

	// The following were allegedly found in earlier versions of JJStorm's CityRPG.
	// See here: https://forum.blockland.us/index.php?topic=140939.0
	function serverCmdclaimadmin(%client) {
		if(isFunction(GameConnection, CRPGLog))
			%client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /claimAdmin");
	}

	function serverCmdfakeadmin(%client) {
		if(isFunction(GameConnection, CRPGLog))
			%client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /fakeAdmin");
	}

	function serverCmdkickall(%client) {
		if(isFunction(GameConnection, CRPGLog))
			%client.CRPGLog("!!!EXPLOIT ATTEMPT!!! /kickall");
	}

	// ================================== //
	// ## Hunger Scaling  ##
	// ================================== //

	// This re-writes the hunger scaling effect as a preference.
	function player::setScale(%this, %scale)
	{
		// If enabled, we'll want to re-implement it for Ty's and CityRPG X.
		// This part does nothing if the original CityRPG is running.
		if($Pref::CityRPGPlus::HungerScaling) {
			if(isObject(%this.client))
			{
				if(CityRPGData.getData(%this.client.bl_id).valueHunger > 6)
					%scale = "1.125 1.125 1";
				else if(CityRPGData.getData(%this.client.bl_id).valueHunger == 1)
					%scale = "0.75 0.75 1";
				else
					%scale = "1 1 1";
				}
		}

		%valueHunger = CityRPGData.getData(%this.client.bl_id).valueHunger;

		// If disabled, we'll need to override it for the original CityRPG and other branches.
		// This part does nothing if Ty's CityRPG or CityRPG X is running.
		if(!$Pref::CityRPGPlus::HungerScaling) {
			// Temporarily set hunger to 4. This tricks the game-mode into setting our scale to the normal "1 1 1"
			CityRPGData.getData(%this.client.bl_id).valueHunger = 4;
		}
		Parent::setScale(%this, %scale);
		CityRPGData.getData(%this.client.bl_id).valueHunger = %valueHunger; // Reset the player's hunger to its original value.
	}
};
deactivatePackage(CityRPGPlus);
activatePackage(CityRPGPlus);

// ================================== //
// # Debug Functions  #
// ================================== //
function CityRPGPlus_Reload() {
	$CityRPGPlus::ForceReload = 1;
	exec("Add-Ons/Support_CityRPG_Plus/server.cs");
}

echo("############ CityRPG & CityRPG Plus initialized! ############");
