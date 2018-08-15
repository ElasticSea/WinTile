# WinTile
Customizable Windows tiling manager

## Guide
* Install `setup.exe`
* The default profile has 2x2 grid just like in windows. Take a look at the keyboard shortcuts in the _Actions_ section in right panel.
* Click _Start_ in the lower right corners. The application starts running. You can minimize the window.
* You can now move and expand focused windows with those shortcuts mentioned previously.

## Description
WinTile is easy to use, customizable windows tiling manager. Move and expand windows easily with hotkeys. It should work on windows that have .NET 4.5 and higher.

## Motivation
Original windows tiling (Areo Snap) and similar implementations were limited, they lacked keyboard binding and custom layouts.

## How does it work
WinTile is a simple WPF app, that wraps around native windows `user32.dll` API. It can start with windows and bind hotkeys at start. Hotkeys are registered globally with `User32.RegisterHotKey` and `User32.UnregisterHotKey`. Each action uses `User32.SetWindowPos` in some sense and couple other methods.
