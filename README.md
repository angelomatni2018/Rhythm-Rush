# Rhythm-Rush

The directories on this repository hold everything necessary to bring a new, empty Unity project up and running with this game's code, resources, and settings.

The basic implemented level uses mechanics I tested out earlier in the quarter. They will change very soon.

For example, movement will be restrained to beats. Also, tiles like the DeathTile will not toggle but rather move around smoothly, allowing for simpler recognition of their timing so one can react to them.

The current setup makes good, and simple use of Events in C# and Unity. Read up about them, as the game's timing will largely be driven by events, with more complicated logic to come soon.

The gameobjects present in the tester scene are very organized in the heirarchy. This is currently done manually, but the LevelController will be the place where gameobjects will soon be initialized straight from prefabs. Read up about prefabs.