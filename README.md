# YookaDebug
Modification for Yooka-Laylee (v1.1.0) which adds an DebugMode. Usefull for analyzing the game and practice.

# How to install
Go to your ../steam/steamapps/common/YookaLaylee/YookaLaylee64_Data/Managed directory and replace the Assembly-CSharp.dll with the modded one. Don't forget to make a Back-up of your original .dll, so you don't have to reload it if you wish to disable the DebugMode.

# How to use
- Open the DebugMenu with NUMPAD_0 or F10
- Click on the options you want to activate (Cursor get's enabled in this state)
- In the auto generated file ../steam/steamapps/common/YookaLaylee/xNyuDebug/key_settings.txt you can manually set the hotkeys and additional parameters. After edited this file, go back ingame and click the "Reload Hotkey Settings" option

# Create your own Assembly
Download DnSpy https://github.com/dnSpy/dnSpy and add the class inside of mod.cs in the original Assembly-CSharp.dll. You will need to change the StartUp class aswell and create a new GameObject of the xNyuDebug class. After this you can save the module and install it.
It's possible that you want the mod in another version than 1.1.0, so consider this as an option. Mod should work for different versions too, I guess.

# TAS Tutorial
- Create and Save scripts:
  In the ../steam/steamapps/common/YookaLaylee/xNyuDebug/TAS directory is the place where to save your TAS scripts. Every script has to be an `.ylt` file, otherwise it
  will not be found from the debugger.
- Run scripts:
  If you have a script (Try the default `TestScript.ylt`), go to hotkeys option and select it from the TAS row. You can run the script with this hotkey now.
- Debug scripts:
  With the `TAS.OpenConsole()` and `TAS.CloseConsole()` hotkey, you can open/close and commandline, which gets opened as additional window. If you have syntax errors
  etc., you will get a feedback there.
- Program scripts:
  With `frame{}` you define what player functions are called with each frame. With `wait{}` you define to wait a specific amaounts of frames. This code has *not* to be
  case-sensitive, type as you want. *IMPORTANT* however is, that each frame/wait has a own line, so press new line for a new command (There is no delimiter yet).
  There are no options for comments either. It's thinkful that you get sometimes an error, because there is a space to much or what ever. You will see if something
  is wrong in the debug window. Here is the full list of commands
  
  Fly()
  Context()
  Crouch()
  Wheel()
  BasicAttack()
  SwimUnderWater()
  SwimUnderWaterAlt()
  SonarBlastAttack()
  SonarBoomAttack()
  SonarShieldAttack()
  Aiming()
  ShootEatenItem()
  ToungeedibleItem()
  Invisibility()
  Wheelspin()
  Groundpound()
  Fartbubble()
  EmoteHappy()
  EmoteTaunt()
  EmoteDisappointed()
  EmoteAngry()
  Walk(float x, float y, float z) -> This is a Vector3 parameter, so you need to pass 3 float values here
  Camera(float x, float y, float z) -> This is a Vector3 parameter, so you need to pass 3 float values here
  
  In the source code, there are Inputcontrollers, which can be triggered by manipulating their booleans. The list above, represents each Inputcontroller existing,
  even by their original name. The Camera command isn't directly like this in game. This one manipulates just the camera angle to what you set.
  
  -Example:
  //Set camera and press jump in frame 1
  frame{Jump();Camera(100,258,0)}
  //Hold jump for 2 more frames pressed
  frame{Jump()}
  frame{Jump()}
  //Wait for exact 30 frames
  wait{30}
  //Hold Crouch for the last 1 frame
  frame{Crouch()}
  
  Now save this to a .ytl file (edit with notepad++ or what IDE you like) and put the file in the TAS folder. You are ready to execute some TAS magic now!
 
