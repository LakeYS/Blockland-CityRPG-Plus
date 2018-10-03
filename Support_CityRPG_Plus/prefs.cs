if(!isFunction(registerPreferenceAddon)) {
  return;
}

registerPreferenceAddon("Support_CityRPG_Plus", "CityRPG Plus", "building_add");

new ScriptObject(Preference) {
  className      = "CityRPGPlus";

  addon          = "Support_CityRPG_Plus";
  category       = "General";
  title          = "Hunger affects scale";

  type           = "bool";

  variable       = "$Pref::CityRPGPlus::HungerScaling";

  defaultValue   = "0";
};
