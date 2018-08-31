// This is a script that enhances the CityRPG game-mode.
// Developed by Lake Y.

if($CityRPGPlus::Init && !$CityRPGPlus::ForceReload)
{
	echo("CityRPG Plus already loaded! Aborting...");
	return;
}
$CityRPGPlus::Init = 1;

exec("./prefs.cs");

// If CityRPG is not alreay enabled, try to enable the first one we can find.
// May execute the wrong one if there are multiple copies installed.
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_CityRPG");
if(!isObject(CityRPGData))
  forceRequiredAddOn("GameMode_CityRPG_X");
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_TysCityRPG");
if(!isObject(CityRPGData))
  forceRequiredAddOn("Gamemode_JJStormsCityRPG");

// If the game-mode still doesn't exist, abort with a catch-all message.
if(!isObject(CityRPGData)) {
  // Reset init to 0 in case a custom branch re-runs the add-on.
  $CityRPGPlus::Init = 0;

  error("Server_CityRPGPlus - Unable to load a compatible version of CityRPG. This add-on requires CityRPG in order to run."
	NL "If you are writing a custom CityRPG branch, have your mod detect and execute CityRPG Plus (forceRequiredAddOn won't work) after the game-mode initializes. Avoid renaming the CityRPGData object.");
	return;
}

// # Package # //
package CityRPGPlus
{
  // ## Security ## //
	// This overwrites commands for known exploits.
	// Most common versions of CityRPG have these patched out. This is simply an extra precaution.
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


};
deactivatePackage(CityRPGPlus);
activatePackage(CityRPGPlus);

// # Debug Functions # //
function CityRPGPlus_Reload() {
	$CityRPGPlus::ForceReload = 1;
	exec("Add-Ons/Server_CityRPGPlus/server.cs");
}

echo("############ CityRPG & CityRPG Plus initialized! ############");
