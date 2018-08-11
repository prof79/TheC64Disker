# THEC64 Disker

## Contents

* [Description](#description)
* [Instructions](#instructions)
* [System Requirements](#systemrequirements)
* [History](#history)
* [Disclaimer](#disclaimer)
    * [Icon](#icon)
* [Creator/Maintainer](#creatormaintainer)
* [Build Status](#buildstatus)


## Description

A simple ["THEC64 Mini"](https://thec64.com) USB disk swap utility to make setting the C64 BASIC default disk image easier. The Mini requires an image with a hard-coded file name (`THEC64-drive8.d64`) up to at least firmware 1.0.8.

![Screenshot](https://github.com/prof79/TheC64Disker/raw/master/TheC64Disker/Assets/TheC64Disker_Screenshot_001.png)


## Instructions
Using the software should almost be self-explanatory.

* Prepare an USB stick according to the [THEC64 Mini preparation instructions #1](https://thec64.com/loading-other-programs/).

* Copy your `d64` disk images to the USB stick. The files do not necessarily need to be in the root because THEC64 Disker will perform a full subdirectory search for `d64` files.

* Download the latest archive from [https://github.com/prof79/TheC64Disker/releases](https://github.com/prof79/TheC64Disker/releases).

* Extract the archive to the root of your USB stick. The archive contains/will automatically create a folder `TheC64Disker` in your USB stick root.

* After launching the program (`TheC64Disker\TheC64Disker.exe`) it will show you all disk images found on the USB stick. Click `Activate` to set any image as your `THEC64-drive8.d64` image.

  * Your original `d64` file will not be changed or altered in any way :-)

  * Using hashing technology the program will tell you the currently active image if applicable. The active image will be shown in bold text.

  * If `THEC64-drive8.d64` already exists you will be asked for permission to overwrite it. There is currently no backup function. Copy/rename it manually if there is data from the THEC64 Mini BASIC mode you want to preserve.

* For an improved experience you may consider using an `AutoRun.inf` file or creating an application shortcut in the USB stick root for easy access.


## System Requirements

This application is based on Microsoft .NET and WPF technology. Due to the WPF requirement it is a Windows-only GUI tool.

The required .NET Framework version is 4.5.2.

For operating system support and pre-installed version guidance see [.NET Framework system requirements](https://docs.microsoft.com/en-us/dotnet/framework/get-started/system-requirements). It should work from **Windows Vista SP2** and **Windows 7 SP1** onwards.


## History

### v0.9.1

* Added `CBM 8250` (`.d82`) disk image support by request.

### v0.9.0

This should be stable for production.

* Much better error handling also for corner cases like media removal.

* Help screen when no disk images have been found.

* Refresh functionality.

**Known issues**

* Only tested on Windows 10 so far.

* Overwrite dialog should better be a `Yes`/`No` than `OK`/`Cancel` dialog. Requires more effort in the custom action/window template classes.

* UI may *theoretically* block due to non-asynchronous I/O. Never occurred in practice not even while testing with an optical drive. Probably not worth fixing because too complicated without proper async support in the .NET Framework I/O classes.

### v0.8.0 Pre-Release

Probably already production-ready. A lot of changes/improvements.

* Embracing MVVM: Use of Prism's `InteractionRequest` and `InteractionRequestTrigger` plus custom classes to get rid of `System.Windows.MessageBox` in the view models and mimic its behavior.

* Proper error handling for the activation/overwrite operation.

* Status bar functional/re-worked.

* New about dialog with source links and license information. Accessible via status bar.

* Debug button hidden in release versions.

* Cosmetic changes in disc image list.

**Known issues**

* UI may block due to non-asynchronous I/O.

* Overwrite dialog should better be a `Yes`/`No` than `OK`/`Cancel` dialog. Requires more effort in the custom action/window template classes.

### v0.7.0 Pre-Release

Test release. Fully working but not optimal error handling/user interaction. UI may block due to non-asynchronous I/O.


## Disclaimer
There is no warranty for the functionality and proper behavior of the software. The author(s) and contributor(s) cannot be held responsible for any damage the software might cause.

This software is licensed under the MIT license (see `LICENSE` file).

This software uses the Microsoft Unity Container and [Prism](http://prismlibrary.com) libraries.

### Icon
I, Markus M. Egger, have pixelled the application icon (`Assets\Floppy.ico`) in a quick-and-dirty fashion. You may use it for whatever you like except selling it (unlikely, but ...) or claiming you made it.


## Creator/Maintainer

Markus Michael Egger [https://markusegger.at](https://markusegger.at)


## Build Status

This project uses [AppVeyor](https://appveyor.com).

[![Build status](https://ci.appveyor.com/api/projects/status/626fgs0ml7bs8j00?svg=true)](https://ci.appveyor.com/project/prof79/thec64disker)
