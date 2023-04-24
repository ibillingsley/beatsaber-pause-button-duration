# BeatSaber-PauseButtonDuration
A small Beat Saber plugin providing additional pause button press durations (other than instant and long), like short and double-tap. The current pause options in Beat Saber are either too short resulting in accidental pauses when you'd least want it (`Instant`) or simply take too long to pause (`Long`). If you feel the same, this plugin is for you! Simply select a preset and you are good to go, or alternatively, configure your own pause settings however you prefer. You will never accidentally pause again!

![Screenshot](screenshot.jpg)

## Users
### Usage
After installing the plugin, you can simply select one of the presets from the settings menu and you are good to go! Never accidentally pause again!

#### Available Mode Presets
The currently available pause button mode presets are:
- `Instant` - Press pause to immediately open the menu;
- `Short` - Hold pause for a short amount of time (250 ms) to open the menu;
- `Medium` - Hold pause for a medium amount of time (500 ms) to open the menu;
- `Long` - Hold pause for a long amount of time (750 ms) to open the menu;
- `Double Tap` - Tap pause twice in quick succession to open the menu;
- `Dual Press` - Hold pause on both controllers to open the menu;
- `Button+Trigger` - Hold pause and press the trigger to open the menu.

You can also select the `Custom` mode to configure the parameters any way you like!

## Developers
### Build from Source
To build this project, you need a modded Beat Saber installation on your machine. See the `Installation` paragraph on how to do that.

Download the source (as a `.zip` or through [Git](https://git-scm.com)) from one of these two branches:
- The `master` branch - The latest stable (released) version of the source;
- The `develop` branch - The latest in-development version of the source.

Then add a file in named `PauseButtonDuration.csproj.user` in the project's `PauseButtonDuration` directory with the following contents:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Edit this property to point to your Beat Saber installation directory. -->
    <BeatSaberDir>BEATSABER_DIRECTORY</BeatSaberDir>
  </PropertyGroup>
</Project>
```
Where `BEATSABER_DIRECTORY` must be replaced with the path to your Beat Saber installation on your machine. Something similar to: `C:\Program Files\Steam\steamapps\common\Beat Saber`.

Done! You can now compile and build this project using your favorite IDE or through the [Microsoft .NET SDK](https://dotnet.microsoft.com) with command: ```dotnet build```. Each build will create a `PauseButtonDuration.dll` file and automatically copy it to your Beat Saber `Plugins` directory for testing.

## License
BeatSaber-PauseButtonDuration is licensed under a [MIT License](https://opensource.org/licenses/MIT) (MIT).
