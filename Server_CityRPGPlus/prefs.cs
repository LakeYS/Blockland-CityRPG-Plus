registerPreferenceAddon("Server_CityRPGPlus", "CityRPG Plus", "building_add");

new ScriptObject(Preference) {
  className      = "CityRPGPlus";

  addon          = "Server_CityRPGPlus";
  category       = "General";
  title          = "Hunger affects scale";

  type           = "bool";

  variable       = "$Pref::CityRPGPlus::HungerScaling";

  defaultValue   = "0";
};
