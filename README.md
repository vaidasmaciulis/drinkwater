[h1]Intro[/h1]

Three new stat mechanics are added to the game: Water, Food and Sleep.

To restore food and water stats you have to use consumable items, like Clang Kola and Cosmic Coffee.
5 new consumable items are added: Emergency Ration, Water, Beer and two kinds of potato chips.
Emergency Ratio and Water is producible in Emergency Ration Production Unit.
You can buy other items in the economy station, or find them in a drop pod.
You can get some sleep by sitting in a non controllable seat (slowly), laying in the bed or cryo chamber (fast) or just drinking Coffee.


[h1]For modders[/h1]

This mod uses vanilla stat mechanics, which makes it very easy to integrate with this mod. 
All you have to do in your own mod is to create consumable items that restores either "Food", "Water" or "Sleep" stats. No changes in this mod is required. 

If there are any issues integrating, please let me know.


[h1]Mods that integrates with Eat. Drink. Sleep. Repeat![/h1]

[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2570427696]Plant and Cook[/url] 
(Add above this mod in the list)

[url=https://steamcommunity.com/workshop/filedetails/?id=2626756831]HUD - Modern and Fancy (Color)[/url]
(Add above this mod in the list)

[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2683816256]E.D.S.R. Color Hud (1.199)[/url]
(Add above this mod in the list)

[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2614888731]Industrial Overhaul and Eat. Drink. Sleep.[/url]

[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2230632087]Ultimate Small Grid Conversion Pack[/url]


[h1]Configuration[/h1]

This mod can be configured per save game:

Create and save game with this mod added.
Open Storage directory of your save, i.e.: 
[code]C:\Users\<User Name>\AppData\Roaming\SpaceEngineers\Saves\<Some number>\<Save Game name>\Storage\2547246713.sbm_DrinkWater
[/code]
config_2_14.xml file should be inside. 
Open it with Notepad or other text editor
Notice there are list of values i.e:
[code]  <WATER_USAGE>0.03</WATER_USAGE>
  <FOOD_USAGE>0.015</FOOD_USAGE>
[/code]

Change values, save file, and load the save game again.
All values are per 100 ticks (roughly 1 time per second) and max value of the stat is 100.
So 0.03 water usage means that full water bar will deplete in 100/0.03 = 3333 seconds ~= 1 hour of play time.


[h2]FAQ[/h2]

- Does this work with Daily Needs?

No. This mod has entirely different implementation, no integration with Daily Needs is possible.

- Can this be integrated with <some HUD mod>?

It would be easier for HUD mod author to make changes in his mod to integrate with this mod than vise versa. Please ask the author of HUD mod which you are using.

- I get "Cant't consume more. Wait untill previous effect ends" notification when eating or drinking.

Auto-healing is probably enabled in the world settings. There is bug in game, where no consumable effects works while auto-healing. Need to either disable auto-healing in world settings or heal to 100% before consuming.

- Can not purchase items (Cola and Coffee) in dispenser block.

This is intended behavior. It is done to improve immersion, as you are just using iron/etc components to build dispenser, so there is no way for drinks to appear there realistically. You can still buy items in dispensers in Economy Stations.


[h1]Acknowledgements[/h1]

Emergency Ration Production Unit block is made by Kuvat. Check out his other mods:
https://steamcommunity.com/profiles/76561198116768722/myworkshopfiles/

3D models used in the mod:

Chips lays classic by ayaat600
https://3dexport.com/free-3dmodel-chips-lays-classic-292494.htm

Prlngles Snack Chips by Kutejnikov
https://www.turbosquid.com/FullPreview/Index.cfm/ID/1090963


[h1]License[/h1]

Eat. Drink. Sleep. Repeat! is open-source under GNU GENERAL PUBLIC LICENSE. Check LICENSE file for more details. 
TL;DR: you can use anything from this mod in your own creations, as long as they are also open-source and link to this mod is provided in description. 
However, it is highly recommended to integrate with this mod instead of copying the code.

Git repository of the mod: https://github.com/vaidasmaciulis/drinkwater