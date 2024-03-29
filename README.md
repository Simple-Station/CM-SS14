<p align="center"> <img alt="Colonial Marines 14" width="880" height="300" src="https://github.com/Park-Station/CM-SS14/assets/81725545/55f12234-d5e4-4b2c-b593-b7468d5ecb5d" /></p>

Welcome to CM-SS14 a colonial marines inspired remake of the original CM-SS13 running on [Robust Toolbox](https://github.com/space-wizards/RobustToolbox).

This is the primary repo for CM-SS14 and contains everything you need to use to run you own server.

## Links

| [Discord](https://discord.gg/49KeKwXc8g) |

<!-- ## Documentation/Wiki -->

<!-- Our [docs site](https://docs.spacestation14.io/) has documentation on SS14s content, engine, game design and more. We also have lots of resources for new contributors to the project. -->

## Contributing

We are happy to accept contributions from anybody. Get in Discord if you want to help. We've got a [list of issues](https://github.com/Park-Station/CM-SS14/issues) that need to be done and anybody can pick them up. Don't be afraid to ask for help either!

Refer to [the space wizards guide](https://docs.spacestation14.io/getting-started/dev-setup) for general information about setting up a dev environment but keep in mind that CM-SS14 is a distant fork of space wizards' SS14 and not everything applies. We provide some scripts for making the job easier.

## Building

### Build dependencies

> - Git
> - .NET SDK 7.0 or higher
> - python 3.7 or higher


### Windows

> 1. Clone this repository.
> 2. Run `RUN_THIS.py` to init submodules and download the engine, or run `git submodule update --init --recursive` in a terminal.
> 3. Run the `Scripts/bat/run1buildDebug.bat`
> 4. Run the `Scripts/bat/run2configDev.bat` if you need other configurations run other config scripts.
> 5. Run both the `Scripts/bat/run3server.bat` and `Scripts/bat/run4client.bat`
> 6. Connect to localhost and play.

### Linux

> 1. Clone this repository.
> 2. Run `RUN_THIS.py` to init submodules and download the engine, or run `git submodule update --init --recursive` in a terminal.
> 3. Run the `Scripts/sh/run1buildDebug.sh`
> 4. Run the `Scripts/sh/run2configDev.sh` if you need other configurations run other config scripts.
> 5. Run both the `Scripts/sh/run3server.bat` and `scripts/sh/run4client.sh`
> 6. Connect to localhost and play.

## Todo
### Combat:
>    - [ ] Finish basic weaponry.
>        - [X] Rifle.
>        - [ ] Pistol.
>        - [ ] Shotgun.
>        - [ ] SMG.
>        - [ ] Flamethrower.
>        - [ ] Sniper?
>        - [ ] Mounted weapon.
>        - [ ] Grenades.

>    - [ ] Custom armor.
>        - [ ] Leightweight, medium, and heavy armor.
>        - [ ] Custom armor textures.

>    - [ ] Cover system, one sided barrier.

>    - [ ] Medical equipment.
>        - [ ] Pill boxes.
>        - [ ] Disposable packets of pills.
>        - [ ] Defibs.


### Xenos:
>    - [ ] Xeno eggs.
>        - [ ] Lay egg ability.
>        - [ ] Egg can be taken as ghost role.
>        - [ ] Baby xeno requires lots of food, turns into big xeno.

>    - [ ] Resin system.
>        - [ ] Xeno "mana" system.
>        - [ ] Resing seed ability.
>        - [ ] Resin seed spreads resin coating.
>        - [ ] Xenos on resin coating heal.

>    - [ ] Xeno castes
>        - [ ] Drone.
>        - [ ] Runner.
>        - [ ] Sentinel.
>        - [ ] Defender.
>        - [ ] Queen.


### Server:
>    - [ ] Mapping.
>        - [ ] Map limiter.
>        - [x] Xeno cave.
>        - [ ] Basic on-site medical.
>        - [ ] Weather system.
>        - [ ] Telecomms.

>    - [ ] Gameticking.
>        - [ ] Round system.
>        - [ ] Team seperator.
>        - [ ] Team selector.
>        - [x] Xeno roles for selection.

## License

All code for the content repository is licensed under [MIT](https://github.com/space-wizards/space-station-14/blob/master/LICENSE.TXT).

Most assets are licensed under [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless stated otherwise. Assets have their license and the copyright in the metadata file. [Example](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

Note that some assets are licensed under the non-commercial [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) or similar non-commercial licenses and will need to be removed if you wish to use this project commercially.
