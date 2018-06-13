# THEC64 Disker

A simple ["THEC64 Mini"](https://thec64.com) USB disk swap utility to make setting the C64 BASIC default disk image easier. The Mini requires an image with a hard-coded file name (`THEC64-drive8.d64`) up to at least firmware 1.0.8.

## Instructions
Using the software should almost be self-explanatory.

* Prepare an USB stick according to the [THEC64 Mini preparation instructions #1](https://thec64.com/loading-other-programs/).

* Copy your `d64` disk images to the USB stick. The files do not necessarily need to be in the root because THEC64 Disker will perform a full subdirectory search for `d64` files.

* For the time being (no release bundle) create a folder in the root of your USB stick eg. "TheC64Disker". Copy all program files from a Release or Debug build there (`exe`, `config`, `dll`).

* Remove the `ForceRootPath` setting line from the `TheC64Disker.exe.config` file.

* For an improved experience you may consider using an `AutoRun.inf` file or creating an application shortcut in the USB stick root for easy access.

* After launching the program it will show you all disk images found on the USB stick. Click `Activate` to set any image as your `THEC64-drive8.d64` image.

	* Your original `d64` file will not be changed or altered in any way :-)

	* Using hashing technology the program will tell you the currently active image if applicable. The active image will be shown in bold text.

## System Requirements

This application is based on Microsoft .NET and WPF technology. Due to the WPF requirement it is a Windows-only GUI tool.

The required .NET Framework version is 4.5.2.

For operating system support and pre-installed version guidance see [.NET Framework system requirements](https://docs.microsoft.com/en-us/dotnet/framework/get-started/system-requirements). It should work from Windows Vista SP2 and Windows 7 SP1 onwards.

## Disclaimer
There is no warranty for the functionality and proper behavior of the software. The author(s) and contributor(s) cannot be held responsible for any damage the software might cause.

This software is licensed under the MIT license (see LICENSE file).

This software uses the Microsoft Unity Container and [Prism](http://prismlibrary.com) libraries.

## Icon
I (Markus M. Egger) have pixelled the application icon (Assets\Floppy.ico) in a quick-and-dirty fashion. You may use it for whatever you like except selling it (unlikely, but ...) or claiming you made it.

## Creator/Maintainer

Markus Michael Egger [https://markusegger.at](https://markusegger.at)
