using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

public class xNyuDebug : MonoBehaviour
{
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool AllocConsole();

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool FreeConsole();

    [DllImport("msvcrt.dll")]
    public static extern int system(string cmd);

	[DllImport("User32.dll")]
	public static extern short GetAsyncKeyState(int vKeys);

	public void OnGUI(){

		//Debug Menu is open
		if(DebugMenuActivated){

			//Draw Black Background
			GUI.color = Color.black;
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.color = new Color(1f, 1f, 1f, 1f);



			if(!DebugMenuHotkey){
				//Display Title
				GUI.Label(new Rect(1420 * scr_scale_w, 200 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Yooka Debugger v1.1", StyleTitle);
				GUI.Label(new Rect(1520 * scr_scale_w, 340 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Display values, call methods and more!", StyleAbout);
				GUI.Label(new Rect(1500 * scr_scale_w, 390 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Click on the Options you want to activate", StyleAbout);
				GUI.Label(new Rect(3340 * scr_scale_w, 2080 * scr_scale_h, 2000 * scr_scale_w, 800 * scr_scale_h), "Developed by xNyu", StyleAbout);
				GUI.Label(new Rect(2340 * scr_scale_w, 2080 * scr_scale_h, 2000 * scr_scale_w, 800 * scr_scale_h), debugstring, StyleAbout);

				//Option Details
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h, 960 * scr_scale_w, 1300 * scr_scale_h), "Activate Details", (OptionDetailsActivated ? StyleNormalCyan : StyleNormalWhite));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 1) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Player Position", ( OptionDetailsActivated ? ( OptionDetailsToggles[0] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 2) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Player Gravity", ( OptionDetailsActivated ? ( OptionDetailsToggles[1] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 3) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Player Data", ( OptionDetailsActivated ? ( OptionDetailsToggles[2] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 4) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Player Movement", ( OptionDetailsActivated ? ( OptionDetailsToggles[3] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 5) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Camera Position", ( OptionDetailsActivated ? ( OptionDetailsToggles[4] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 6) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Show Camera Details", ( OptionDetailsActivated ? ( OptionDetailsToggles[5] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 7) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Blank", ( OptionDetailsActivated ? ( OptionDetailsToggles[6] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 8) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Blank", ( OptionDetailsActivated ? ( OptionDetailsToggles[7] ? StyleNormalGreen : StyleNormalRed ) : StyleNormalGray ));

				//Option Hotkeys
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h, 960 * scr_scale_w, 40 * scr_scale_h), "Activate Hotkeys", (OptionHotkeysActivated ? StyleNormalCyan : StyleNormalWhite));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 1) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_1: " + OptionHotkeyToggles[0], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 2) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_2: " + OptionHotkeyToggles[1], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 3) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_3: " + OptionHotkeyToggles[2], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 4) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_4: " + OptionHotkeyToggles[3], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 5) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_5: " + OptionHotkeyToggles[4], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 6) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_6: " + OptionHotkeyToggles[5], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 7) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_7: " + OptionHotkeyToggles[6], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 8) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_8: " + OptionHotkeyToggles[7], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 9) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_9: " + OptionHotkeyToggles[8], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 10) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F1:    " + OptionHotkeyToggles[9], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 11) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F2:    " + OptionHotkeyToggles[10], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 12) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F3:    " + OptionHotkeyToggles[11], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 13) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F4:    " + OptionHotkeyToggles[12], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 14) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F5:    " + OptionHotkeyToggles[13], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 15) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F6:    " + OptionHotkeyToggles[14], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 16) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F7:    " + OptionHotkeyToggles[15], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 17) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F8:    " + OptionHotkeyToggles[16], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 19) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Reload Hotkeysettings from file", (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
			}else{
				//Hotkey Menu
				int d_row = 0;
				int d_line = 0;
				List<string> d_commands = new List<string>();

				d_commands = Funcs_Player;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_Camera;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_Blank3;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_TAS;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}
				
				d_commands = Funcs_Blank5;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_Blank6;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_Special;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

				d_commands = Funcs_Scripts;
				for(int i = 0; i < d_commands.Count; i++){
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++;if(d_row == 4){d_line++;d_row = 0;}

			}

		}else{
			//Menu is Deactivated

				//Drawing details is activated
				if(OptionDetailsActivated){

					float details_box_height = 25f;
					if(OptionDetailsData.Count > 24){
						details_box_height = 25f;
					}else{
						details_box_height = (float)OptionDetailsData.Count;
					}
					float details_box_width = (float)Math.Floor(((float)OptionDetailsData.Count / 25f) - 0.0001f) + 1f;

					if(OptionDetailsData.Count > 0){
						//Draw scaled box background
						GUI.color = new Color(1f, 1f, 1f, 0.85f);
						GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w)*details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
						GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w)*details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
						GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w)*details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
						GUI.color = new Color(1f, 1f, 1f, 1f);

						//Draw details
						float row = 0f;
						float line = 0f;
						for(int i = 0; i < OptionDetailsData.Count; i++){
							if(i > 0 && i % 25 == 0){
								row = row + 1f;
								line = 0;
							}
							string color_tenery = OptionDetailsData[i];
							try{GUI.Label(new Rect(20f + ((500f * scr_scale_w) * row), 20f + ((40f * scr_scale_h) * line), 400f * scr_scale_w, 40f * scr_scale_h), OptionDetailsData[i], (color_tenery[0] == '-' && color_tenery[color_tenery.Length - 1] == '-') ? StyleSmallCyan : StyleSmallWhite);}catch{}
							line++;
						}

					}

				}

		}

	}

	public void Update(){

		//Need to be Up-To-Date Aspect Ratio
		width = (float)Screen.width;
		height = (float)Screen.height;
		scr_scale_w = width / fixed_size_width;
		scr_scale_h = height / fixed_size_height;



		//Activate Debug Menu
		if(Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.F10)){

			if(!DebugMenuHotkey){
				if(DebugMenuActivated){
					DebugMenuActivated = false;
					ResumeGame();
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;

				}else{
					DebugMenuActivated = true;
					SuspendGame();
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;

				}
			}else{
				DebugMenuHotkey = false;
			}

		}

		//If Debug Menu is active (Start Click Routine)
		if(DebugMenuActivated){

			float mouse_x = Input.mousePosition.x;
			float mouse_y = height - Input.mousePosition.y;

			//Mouse Click
			if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)){

				if(!DebugMenuHotkey){
					//Click Options Details
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h && mouse_y < 800 * scr_scale_h + ((50 * 1) * scr_scale_h)) {OptionDetailsActivated = !OptionDetailsActivated;}
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 1) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[0] = !OptionDetailsToggles[0];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[1] = !OptionDetailsToggles[1];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[2] = !OptionDetailsToggles[2];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[3] = !OptionDetailsToggles[3];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[4] = !OptionDetailsToggles[4];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[5] = !OptionDetailsToggles[5];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[6] = !OptionDetailsToggles[6];
					if(mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[7] = !OptionDetailsToggles[7];

					//Click Options Details
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h && mouse_y < 800 * scr_scale_h + ((50 * 1) * scr_scale_h)) {OptionHotkeysActivated = !OptionHotkeysActivated;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 1) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 0; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 1; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 2; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 3; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 4; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 5; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 6; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 7; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 10) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 8; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 10) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 11) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 9; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 11) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 12) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 10; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 12) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 13) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 11; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 13) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 14) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 12; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 14) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 15) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 13; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 15) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 16) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 14; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 16) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 17) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 15; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 17) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 18) * scr_scale_h) && OptionHotkeysActivated) {OptionHotkeySlot = 16; LoadTASScripts(); DebugMenuHotkey = true;}
					if(mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 19) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 20) * scr_scale_h) && OptionHotkeysActivated) {
						//Reload Keysettings
						string[] key_settings_lines = File.ReadAllLines(settings_file);
						for(int i = 0; i < key_settings_lines.Length; i++){
							OptionHotkeyToggles[i] = key_settings_lines[i].Split(':')[1];
						}
					}
				}else{
					//Hotkey Menu
					int d_row = 0;
					int d_line = 0;
					List<string> d_commands = new List<string>();

					d_commands = Funcs_Player;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_Camera;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_Blank3;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_TAS;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}
				
					d_commands = Funcs_Blank5;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_Blank6;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_Special;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}

					d_commands = Funcs_Scripts;
					for(int i = 0; i < d_commands.Count; i++){
						if(i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row+1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i+1)) * scr_scale_h)){OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false;}
					}
					d_row++;if(d_row == 4){d_line++;d_row = 0;}
					
					//Save new settings
					string[] key_settings_lines = new string[17];
					key_settings_lines[0] = "NUM_1:" + (OptionHotkeyToggles[0] != "" && OptionHotkeyToggles[0].Length > 4 ? OptionHotkeyToggles[0] : "None");
					key_settings_lines[1] = "NUM_2:" + (OptionHotkeyToggles[1] != "" && OptionHotkeyToggles[1].Length > 4 ? OptionHotkeyToggles[1] : "None");
					key_settings_lines[2] = "NUM_3:" + (OptionHotkeyToggles[2] != "" && OptionHotkeyToggles[2].Length > 4 ? OptionHotkeyToggles[2] : "None");
					key_settings_lines[3] = "NUM_4:" + (OptionHotkeyToggles[3] != "" && OptionHotkeyToggles[3].Length > 4 ? OptionHotkeyToggles[3] : "None");
					key_settings_lines[4] = "NUM_5:" + (OptionHotkeyToggles[4] != "" && OptionHotkeyToggles[4].Length > 4 ? OptionHotkeyToggles[4] : "None");
					key_settings_lines[5] = "NUM_6:" + (OptionHotkeyToggles[5] != "" && OptionHotkeyToggles[5].Length > 4 ? OptionHotkeyToggles[5] : "None");
					key_settings_lines[6] = "NUM_7:" + (OptionHotkeyToggles[6] != "" && OptionHotkeyToggles[6].Length > 4 ? OptionHotkeyToggles[6] : "None");
					key_settings_lines[7] = "NUM_8:" + (OptionHotkeyToggles[7] != "" && OptionHotkeyToggles[7].Length > 4 ? OptionHotkeyToggles[7] : "None");
					key_settings_lines[8] = "NUM_9:" + (OptionHotkeyToggles[8] != "" && OptionHotkeyToggles[8].Length > 4 ? OptionHotkeyToggles[8] : "None");
					key_settings_lines[9] = "F1:" + (OptionHotkeyToggles[9] != "" && OptionHotkeyToggles[9].Length > 4 ? OptionHotkeyToggles[9] : "None");
					key_settings_lines[10] = "F2:" + (OptionHotkeyToggles[10] != "" && OptionHotkeyToggles[10].Length > 4 ? OptionHotkeyToggles[10] : "None");
					key_settings_lines[11] = "F3:" + (OptionHotkeyToggles[11] != "" && OptionHotkeyToggles[11].Length > 4 ? OptionHotkeyToggles[11] : "None");
					key_settings_lines[12] = "F4:" + (OptionHotkeyToggles[12] != "" && OptionHotkeyToggles[12].Length > 4 ? OptionHotkeyToggles[12] : "None");
					key_settings_lines[13] = "F5:" + (OptionHotkeyToggles[13] != "" && OptionHotkeyToggles[13].Length > 4 ? OptionHotkeyToggles[13] : "None");
					key_settings_lines[14] = "F6:" + (OptionHotkeyToggles[14] != "" && OptionHotkeyToggles[14].Length > 4 ? OptionHotkeyToggles[14] : "None");
					key_settings_lines[15] = "F7:" + (OptionHotkeyToggles[15] != "" && OptionHotkeyToggles[15].Length > 4 ? OptionHotkeyToggles[15] : "None");
					key_settings_lines[16] = "F8:" + (OptionHotkeyToggles[16] != "" && OptionHotkeyToggles[16].Length > 4 ? OptionHotkeyToggles[16] : "None");
					File.WriteAllLines(settings_file, key_settings_lines);

					//Deactivate Menu, even if nothing is selected
					DebugMenuHotkey = false;
					
				}


			}






		}

		//If Debug Menu isn't active (Start Routine)
		if(!DebugMenuActivated){


			if(skip_1_frame == 0){
				//Clear List
				if(OptionDetailsActivated){
					if(OptionDetailsData.Count > 0) OptionDetailsData.Clear();

					//Find Objects
					PlayerLogic player = GameObject.FindObjectOfType<PlayerLogic>();
					PlayerGravitySets player_gravity = player.GravitySets;

					FollowCamera camera = (FollowCamera)FindObjectOfType<CameraManager>().GetPlayerCamera(PlayerCameraTypes.Follow);


					if(OptionDetailsToggles[0]){
						//Player Coordinates
						try{OptionDetailsData.Add("-Player Position-");}catch{}
						try{OptionDetailsData.Add("Position: " + player.transform.position.ToString());}catch{}
						try{OptionDetailsData.Add("Rotation: " + player.transform.eulerAngles.ToString());}catch{}
						try{OptionDetailsData.Add("Forward: " + player.transform.forward.ToString());}catch{}
					}

					if(OptionDetailsToggles[1]){
						//Player Physics
						try{OptionDetailsData.Add("-Player Gravity-");}catch{}
						try{OptionDetailsData.Add("GravityDefault Factor: " + player_gravity.GravityDefault.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityDefault DragYUp: " + player_gravity.GravityDefault.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityDefault DragYDown:" + player_gravity.GravityDefault.DragYDownwards.ToString());}catch{}
					
						try{OptionDetailsData.Add("GravityDynamic Factor: " + player_gravity.GravityDynamic.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityDynamic DragYUp: " + player_gravity.GravityDynamic.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityDynamic DragYDown:" + player_gravity.GravityDynamic.DragYDownwards.ToString());}catch{}
					
						try{OptionDetailsData.Add("GravityFartBubble Factor: " + player_gravity.GravityFartBubble.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFartBubble DragYUp: " + player_gravity.GravityFartBubble.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFartBubble DragYDown:" + player_gravity.GravityFartBubble.DragYDownwards.ToString());}catch{}
					
						try{OptionDetailsData.Add("GravityFluidQuicks Factor: " + player_gravity.GravityFluidQuicksandSurface.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFluidQuicks DragYUp: " + player_gravity.GravityFluidQuicksandSurface.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFluidQuicks DragYDown:" + player_gravity.GravityFluidQuicksandSurface.DragYDownwards.ToString());}catch{}
					
						try{OptionDetailsData.Add("GravityFly Factor: " + player_gravity.GravityFly.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFly DragYUp: " + player_gravity.GravityFly.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityFly DragYDown:" + player_gravity.GravityFly.DragYDownwards.ToString());}catch{}

						try{OptionDetailsData.Add("GravityWaterSurface Factor: " + player_gravity.GravityWaterSurface.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityWaterSurface DragYUp: " + player_gravity.GravityWaterSurface.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityWaterSurface DragYDown:" + player_gravity.GravityWaterSurface.DragYDownwards.ToString());}catch{}

						try{OptionDetailsData.Add("GravityUnderwater Factor: " + player_gravity.GravityWaterSwimUnderwater.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityUnderwater DragYUp: " + player_gravity.GravityWaterSwimUnderwater.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityUnderwater DragYDown:" + player_gravity.GravityWaterSwimUnderwater.DragYDownwards.ToString());}catch{}

						try{OptionDetailsData.Add("GravityUnderwaterV2 Factor: " + player_gravity.GravityWaterSwimUnderwaterV2.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityUnderwaterV2 DragYUp: " + player_gravity.GravityWaterSwimUnderwaterV2.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityUnderwaterV2 DragYDown:" + player_gravity.GravityWaterSwimUnderwaterV2.DragYDownwards.ToString());}catch{}
					
						try{OptionDetailsData.Add("GravityXFCasino Factor: " + player_gravity.GravityXFCasino.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityXFCasino DragYUp: " + player_gravity.GravityXFCasino.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityXFCasino DragYDown:" + player_gravity.GravityXFCasino.DragYDownwards.ToString());}catch{}

						try{OptionDetailsData.Add("GravityXFShoal Factor: " + player_gravity.GravityXFShoal.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GravityXFShoal DragYUp: " + player_gravity.GravityXFShoal.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GravityXFShoal DragYDown:" + player_gravity.GravityXFShoal.DragYDownwards.ToString());}catch{}

						try{OptionDetailsData.Add("GroundPound Factor: " + player_gravity.GroundPound.Gravity.ToString());}catch{}
						try{OptionDetailsData.Add("GroundPound DragYUp: " + player_gravity.GroundPound.DragYUpwards.ToString());}catch{}
						try{OptionDetailsData.Add("GroundPound DragYDown:" + player_gravity.GroundPound.DragYDownwards.ToString());}catch{}
					}

					if(OptionDetailsToggles[2]){
						//Player Data
						try{OptionDetailsData.Add("-Player Data-");}catch{}
						try{OptionDetailsData.Add("HP: " + player.MyHealth.CurrentHealth.ToString() + "/" + player.MyHealth.MaxHealth.ToString());}catch{}
						try{OptionDetailsData.Add("Invulnerable: " + player.MyHealth.Invulnerable.ToString());}catch{}
						try{OptionDetailsData.Add("Num. Colliding Objects: " + player.GetNumCollidingObjects().ToString());}catch{}
						try{OptionDetailsData.Add("Combat Timeout Duration: " + player.CombatTimeOutDuration.ToString());}catch{}
					}

					if(OptionDetailsToggles[3]){
						//Player Movement
						try{OptionDetailsData.Add("-Player Movement-");}catch{}
						try{OptionDetailsData.Add("Velocity Cache: " + player.VelocityCache.GetVelocity().ToString());}catch{}
						try{OptionDetailsData.Add("Height to Fall: " + player.Jump.HeightAboveGroundBeforeFallConsidered.ToString());}catch{}
						try{OptionDetailsData.Add("Is Fall Triggered: " + player.Jump.isFallTriggered().ToString());}catch{}
						try{OptionDetailsData.Add("Is Jump Triggered: " + player.Jump.isJumpTriggered().ToString());}catch{}
						try{OptionDetailsData.Add("Rampo Jump Mod: " + player.Jump.RampoExtraJumpMod.ToString());}catch{}
						try{OptionDetailsData.Add("Ground Check Time: " + player.Ground.BlockGroundCheckTimeWhenJumped.ToString());}catch{}
						try{OptionDetailsData.Add("Platform Position Delta: " + player.Ground.GetPositionDeltaFromPlatform().ToString());}catch{}
						try{OptionDetailsData.Add("Is Above Platform: " + player.Ground.IsAbovePlatform.ToString());}catch{}
						try{OptionDetailsData.Add("Ground Position: " + player.Ground.GroundPosition.ToString());}catch{}
						try{OptionDetailsData.Add("Ground Surface Angle: " + player.Ground.GroundSurfaceAngleRad.ToString());}catch{}
						try{OptionDetailsData.Add("Height Above Ground: " + player.Ground.HeightAboveGround.ToString());}catch{}
						try{OptionDetailsData.Add("Is Grounded: " + player.Ground.IsGrounded.ToString());}catch{}
						try{OptionDetailsData.Add("Raycast Distance: " + player.Ground.RaycastDistance.ToString());}catch{}
						try{OptionDetailsData.Add("Current XZ Speed %: " + player.Motion.CurrentXZSpeedPercent.ToString());}catch{}
						try{OptionDetailsData.Add("External XZ Force: " + player.Motion.ExternalForceXZ.ToString());}catch{}
						try{OptionDetailsData.Add("Current Max Speed: " + player.Motion.GetCurrentMaxSpeed().ToString());}catch{}
						try{OptionDetailsData.Add("Is Motion Disabled: " + player.Motion.IsMotionDisabled.ToString());}catch{}
						try{OptionDetailsData.Add("Is XZ Input Disabled: " + player.Motion.IsXZInputDisabled.ToString());}catch{}
					}

					if(OptionDetailsToggles[4]){
						//Camera Position
						try{OptionDetailsData.Add("-Camera Position-");}catch{}
						try{OptionDetailsData.Add("Position: " + camera.transform.position.ToString());}catch{}
						try{OptionDetailsData.Add("Position True: " + camera.CameraPosition.ToString());}catch{}
						try{OptionDetailsData.Add("Angle: " + camera.transform.eulerAngles.ToString());}catch{}
						try{OptionDetailsData.Add("Angle True: " + camera.CameraRotation.ToString());}catch{}
						try{OptionDetailsData.Add("Forward: " + camera.transform.forward.ToString());}catch{}
						try{OptionDetailsData.Add("Max Speed Bad Position: " + camera.MaxPlayerSpeedAllowCameraMoveToBadPosition.ToString());}catch{}
						try{OptionDetailsData.Add("Flying Height Speed Curve: " + camera.FlyingSpeedToHeightCurve.ToString());}catch{}
						try{OptionDetailsData.Add("Base Sideway Resolve Speed: " + camera.BaseSidewaysResolveSpeed.ToString());}catch{}
					}

					if(OptionDetailsToggles[5]){
						//Camera Details
						try{OptionDetailsData.Add("-Camera Details-");}catch{}
						try{OptionDetailsData.Add("FOV: " + camera.CameraFOV.ToString());}catch{}
						try{OptionDetailsData.Add("Interpolate: " + camera.InterpolateCameraTransform.ToString());}catch{}
						try{OptionDetailsData.Add("Flying Height Speed Curve: " + camera.BaseFallingHeightOffset.ToString());}catch{}
						try{OptionDetailsData.Add("Flying Height Speed Curve: " + camera.BaseRestingHeightOffset.ToString());}catch{}
						try{OptionDetailsData.Add("Stick Min/Max Rotate: " + camera.CameraStickMinRotateScale.ToString() + "/" + camera.CameraStickMaxRotateScale.ToString());}catch{}
					}

					if(OptionDetailsToggles[6]){
						//World Data

					}

					if(OptionDetailsToggles[7]){
						//World Booleans

					}

				}
			}


















			//Keybindings Call Functions
			if(OptionHotkeysActivated){
				if(Input.GetKey(KeyCode.Keypad1) && OptionHotkeyCanPress[0]){
					OptionHotkeyCanPress[0] = HotkeyToFunc(OptionHotkeyToggles[0]);
				}else if(Input.GetKey(KeyCode.Keypad2) && OptionHotkeyCanPress[1]){
					OptionHotkeyCanPress[1] = HotkeyToFunc(OptionHotkeyToggles[1]);
				}else if(Input.GetKey(KeyCode.Keypad3) && OptionHotkeyCanPress[2]){
					OptionHotkeyCanPress[2] = HotkeyToFunc(OptionHotkeyToggles[2]);
				}else if(Input.GetKey(KeyCode.Keypad4) && OptionHotkeyCanPress[3]){
					OptionHotkeyCanPress[3] = HotkeyToFunc(OptionHotkeyToggles[3]);
				}else if(Input.GetKey(KeyCode.Keypad5) && OptionHotkeyCanPress[4]){
					OptionHotkeyCanPress[4] = HotkeyToFunc(OptionHotkeyToggles[4]);
				}else if(Input.GetKey(KeyCode.Keypad6) && OptionHotkeyCanPress[5]){
					OptionHotkeyCanPress[5] = HotkeyToFunc(OptionHotkeyToggles[5]);
				}else if(Input.GetKey(KeyCode.Keypad7) && OptionHotkeyCanPress[6]){
					OptionHotkeyCanPress[6] = HotkeyToFunc(OptionHotkeyToggles[6]);
				}else if(Input.GetKey(KeyCode.Keypad8) && OptionHotkeyCanPress[7]){
					OptionHotkeyCanPress[7] = HotkeyToFunc(OptionHotkeyToggles[7]);
				}else if(Input.GetKey(KeyCode.Keypad9) && OptionHotkeyCanPress[8]){
					OptionHotkeyCanPress[8] = HotkeyToFunc(OptionHotkeyToggles[8]);
				}else if(Input.GetKey(KeyCode.F1) && OptionHotkeyCanPress[9]){
					OptionHotkeyCanPress[9] = HotkeyToFunc(OptionHotkeyToggles[9]);
				}else if(Input.GetKey(KeyCode.F2) && OptionHotkeyCanPress[10]){
					OptionHotkeyCanPress[10] = HotkeyToFunc(OptionHotkeyToggles[10]);
				}else if(Input.GetKey(KeyCode.F3) && OptionHotkeyCanPress[11]){
					OptionHotkeyCanPress[11] = HotkeyToFunc(OptionHotkeyToggles[11]);
				}else if(Input.GetKey(KeyCode.F4) && OptionHotkeyCanPress[12]){
					OptionHotkeyCanPress[12] = HotkeyToFunc(OptionHotkeyToggles[12]);
				}else if(Input.GetKey(KeyCode.F5) && OptionHotkeyCanPress[13]){
					OptionHotkeyCanPress[13] = HotkeyToFunc(OptionHotkeyToggles[13]);
				}else if(Input.GetKey(KeyCode.F6) && OptionHotkeyCanPress[14]){
					OptionHotkeyCanPress[14] = HotkeyToFunc(OptionHotkeyToggles[14]);
				}else if(Input.GetKey(KeyCode.F7) && OptionHotkeyCanPress[15]){
					OptionHotkeyCanPress[15] = HotkeyToFunc(OptionHotkeyToggles[15]);
				}else if(Input.GetKey(KeyCode.F8) && OptionHotkeyCanPress[16]){
					OptionHotkeyCanPress[16] = HotkeyToFunc(OptionHotkeyToggles[16]);
				}

				//Keybindings Release Detection
				if(Input.GetKeyUp(KeyCode.Keypad1)){
					OptionHotkeyCanPress[0] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad2)){
					OptionHotkeyCanPress[1] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad3)){
					OptionHotkeyCanPress[2] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad4)){
					OptionHotkeyCanPress[3] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad5)){
					OptionHotkeyCanPress[4] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad6)){
					OptionHotkeyCanPress[5] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad7)){
					OptionHotkeyCanPress[6] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad8)){
					OptionHotkeyCanPress[7] = true;
				}
				if(Input.GetKeyUp(KeyCode.Keypad9)){
					OptionHotkeyCanPress[8] = true;
				}
				if(Input.GetKeyUp(KeyCode.F1)){
					OptionHotkeyCanPress[9] = true;
				}
				if(Input.GetKeyUp(KeyCode.F2)){
					OptionHotkeyCanPress[10] = true;
				}
				if(Input.GetKeyUp(KeyCode.F3)){
					OptionHotkeyCanPress[11] = true;
				}
				if(Input.GetKeyUp(KeyCode.F4)){
					OptionHotkeyCanPress[12] = true;
				}
				if(Input.GetKeyUp(KeyCode.F5)){
					OptionHotkeyCanPress[13] = true;
				}
				if(Input.GetKeyUp(KeyCode.F6)){
					OptionHotkeyCanPress[14] = true;
				}
				if(Input.GetKeyUp(KeyCode.F7)){
					OptionHotkeyCanPress[15] = true;
				}
				if(Input.GetKeyUp(KeyCode.F8)){
					OptionHotkeyCanPress[16] = true;
				}
			}












		}

		//Custom Scripts
		if(skip_2_frame == 0){
			if(ScriptGodMode){
					FindObjectOfType<PlayerLogic>().MyHealth.SetHealth(FindObjectOfType<PlayerLogic>().MyHealth.MaxHealth, false);
			}
		}








		//Font Settings
		if(skip_1_frame == 0){
			int f_size_title = (int)(110f * scr_scale_w);
			int f_size_about = (int)(50f * scr_scale_w);
			int f_size_normal = (int)(40f * scr_scale_w);
			int f_size_small = (int)(30f * scr_scale_w);

			StyleTitle.fontSize = f_size_title;
			StyleAbout.fontSize = f_size_about;

			StyleNormalWhite.fontSize = f_size_normal;
			StyleNormalGray.fontSize = f_size_normal;
			StyleNormalRed.fontSize = f_size_normal;
			StyleNormalGreen.fontSize = f_size_normal;
			StyleNormalPurple.fontSize = f_size_normal;
			StyleNormalCyan.fontSize = f_size_normal;

			StyleSmallWhite.fontSize = f_size_small;
			StyleSmallGray.fontSize = f_size_small;
			StyleSmallRed.fontSize = f_size_small;
			StyleSmallGreen.fontSize = f_size_small;
			StyleSmallPurple.fontSize = f_size_small;
			StyleSmallCyan.fontSize = f_size_small;
			StyleSmallYellow.fontSize = f_size_small;
		}

		

		//Increase Performance Frameskip
		skip_1_frame++;
		skip_2_frame++;
		if(skip_1_frame >= 2) skip_1_frame = 0;
		if(skip_2_frame >= 3) skip_2_frame = 0;

	}



    public static void OpenConsole()
    {
		AllocConsole();
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
		Application.logMessageReceivedThreaded += (condition, stackTrace, type) => Console.WriteLine(condition + " " + stackTrace);
    }

    public static void ClearConsole()
    {
		system("CLS");
	}



	public bool HotkeyToFunc(string command){
		//Set lowe case
		command = command.ToLower();

		//Allow Rapid Press
		bool allowPressMore = false;

		//Find Arguments
		List<string> hotkey_arguments = new List<string>();
		if(command[command.IndexOf("(")+1] != ')'){
			string sub_arguments = command.Replace(command.Substring(0, command.IndexOf("(") + 1), "");
			sub_arguments = sub_arguments.Replace(")", "");
			string[] h_args = sub_arguments.Split(',');

			for(int i = 0; i < h_args.Length; i++){
				hotkey_arguments.Add(h_args[i].Replace(" ", ""));
			}
		}

		//Find Objects
		PlayerLogic player = FindObjectOfType<PlayerLogic>();
		FollowCamera camera = (FollowCamera)FindObjectOfType<CameraManager>().GetPlayerCamera(PlayerCameraTypes.Follow);

		if(command.Contains("player.changepositionx")){
			Vector3 position = new Vector3(player.transform.position.x + float.Parse(hotkey_arguments[0]), player.transform.position.y, player.transform.position.z);
			player.MyPlayerAsk.WarpToPosition(position, 0f, true);
			allowPressMore = true;
		}else if(command.Contains("player.changepositiony")){
			Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y + float.Parse(hotkey_arguments[0]), player.transform.position.z);
			player.MyPlayerAsk.WarpToPosition(position, 0f, true);
			allowPressMore = true;
		}else if(command.Contains("player.changepositionz")){
			Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + float.Parse(hotkey_arguments[0]));
			player.MyPlayerAsk.WarpToPosition(position, 0f, true);
			allowPressMore = true;
		}else if(command.Contains("player.warp")){
			Vector3 position = new Vector3(float.Parse(hotkey_arguments[0]), float.Parse(hotkey_arguments[1]), float.Parse(hotkey_arguments[2]));
			player.MyPlayerAsk.WarpToPosition(position, 0f, true);
			allowPressMore = false;
		}else if(command.Contains("player.setcurrenthealth")){
			player.MyHealth.SetHealth(int.Parse(hotkey_arguments[0]), bool.Parse(hotkey_arguments[1]));
			allowPressMore = false;
		}else if(command.Contains("player.addcurrenthealth")){
			player.MyHealth.AddHealth(int.Parse(hotkey_arguments[0]), bool.Parse(hotkey_arguments[1]));
			allowPressMore = false;
		}else if(command.Contains("player.subcurrenthealth")){
			player.MyHealth.SubtractHealth(int.Parse(hotkey_arguments[0]), bool.Parse(hotkey_arguments[1]));
			allowPressMore = false;
		}else if(command.Contains("camera.changepositionx")){
			camera.transform.position.Set(camera.transform.position.x + float.Parse(hotkey_arguments[0]), camera.transform.position.y, camera.transform.position.z);
			allowPressMore = true;
		}else if(command.Contains("camera.changepositiony")){
			camera.transform.position += new Vector3(camera.transform.position.x, camera.transform.position.y + float.Parse(hotkey_arguments[0]), camera.transform.position.z);
			allowPressMore = true;
		}else if(command.Contains("camera.changepositionz")){
			camera.transform.position += new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + float.Parse(hotkey_arguments[0]));
			allowPressMore = true;
		}else if(command.Contains("camera.setposition")){
			camera.transform.position += new Vector3(float.Parse(hotkey_arguments[0]), float.Parse(hotkey_arguments[1]), float.Parse(hotkey_arguments[2]));
			allowPressMore = false;
		}else if(command.Contains("camera.unlock")){
			CameraUnlock = !CameraUnlock;
			if(hotkey_arguments.Count > 0) CameraUnlock = bool.Parse(hotkey_arguments[0]);
			allowPressMore = false;
		}else if(command.Contains("camera.setextralookdisabled")){
			ExtraLook = !ExtraLook;
			if(hotkey_arguments.Count > 0) ExtraLook = bool.Parse(hotkey_arguments[0]);
			camera.SetExtraLookDisabled(ExtraLook);
			allowPressMore = false;
		}else if(command.Contains("camera.setrestingposition")){
			camera.SetRestingPosition();
			allowPressMore = false;
		}else if(command.Contains("camera.fov")){
			camera.CameraFOV = float.Parse(hotkey_arguments[0]);
			allowPressMore = true;
		}else if(command.Contains("camera.playerhaswarped")){
			CameraManager.Instance.PlayerHasWarped();
			allowPressMore = true;
		}else if(command.Contains("tas.execute")){
			var TASObject = new GameObject();
			ScriptPath = tas_dir + "\\" + hotkey_arguments[0];
	        TASObject.AddComponent<xNyuTAS>();
        	GameObject.DontDestroyOnLoad(TASObject);
			allowPressMore = false;
		}else if(command.Contains("tas.openconsole")){
			OpenConsole();
			allowPressMore = false;
		}else if(command.Contains("tas.closeconsole")){
			FreeConsole();
			allowPressMore = false;
		}else if(command.Contains("tas.clearconsole")){
			ClearConsole();
			allowPressMore = false;
		}else if(command.Contains("tas.stopscript")){
			xNyuTAS gameObject = FindObjectOfType<xNyuTAS>();
			if(gameObject != null) Destroy(gameObject);
			PlayerInputStore  InputHandler = FindObjectOfType<PlayerLogic>().InputStore;
			//Enable Input
			InputHandler.MoveStick.SetFakedInput(false, Vector3.zero);
			InputHandler.Jump.SetFakedInput(false, false);
			InputHandler.Fly.SetFakedInput(false, false);
			InputHandler.Context.SetFakedInput(false, false);
			InputHandler.Crouch.SetFakedInput(false, false);
			InputHandler.Wheel.SetFakedInput(false, false);
			InputHandler.BasicAttack.SetFakedInput(false, false);
			InputHandler.SwimUnderwater.SetFakedInput(false, false);
			InputHandler.SwimUnderwaterAlt.SetFakedInput(false, false);
			InputHandler.SonarBlastAttack.SetFakedInput(false, false);
			InputHandler.SonarBoomAttack.SetFakedInput(false, false);
			InputHandler.SonarShieldAttack.SetFakedInput(false, false);
			InputHandler.Aiming.SetFakedInput(false, false);
			InputHandler.ShootEatenItem.SetFakedInput(false, false);
			InputHandler.TongueEdibleItem.SetFakedInput(false, false);
			InputHandler.Invisibility.SetFakedInput(false, false);
			InputHandler.WheelSpin.SetFakedInput(false, false);
			InputHandler.GroundPound.SetFakedInput(false, false);
			InputHandler.FartBubble.SetFakedInput(false, false);
			InputHandler.EmoteHappy.SetFakedInput(false, false);
			InputHandler.EmoteTaunt.SetFakedInput(false, false);
			InputHandler.EmoteDisappointed.SetFakedInput(false, false);
			InputHandler.EmoteAngry.SetFakedInput(false, false);
			allowPressMore = false;
		}else if(command.Contains("special.pausegame")){
			SuspendGame();
			allowPressMore = false;
		}else if(command.Contains("special.unpausegame")){
			ResumeGame();
			allowPressMore = false;
		}else if(command.Contains("custom.godmode")){
			bool to_set = !ScriptGodMode;
			if(hotkey_arguments.Count > 0) to_set = bool.Parse(hotkey_arguments[0]);
			ScriptGodMode = to_set;
			allowPressMore = false;
		}else if(command.Contains("custom.maxspeed")){
			bool to_set = !ScriptMaxSpeed;
			if(hotkey_arguments.Count > 0) to_set = bool.Parse(hotkey_arguments[0]);
			ScriptMaxSpeed = to_set;
			allowPressMore = false;
		}else if(command.Contains("custom.maxjump")){
			bool to_set = !ScriptMaxJump;
			if(hotkey_arguments.Count > 0) to_set = bool.Parse(hotkey_arguments[0]);
			ScriptMaxJump = to_set;
			allowPressMore = false;
		}

		

		return allowPressMore;

	}

	public void WriteConsole(object text){
		try{Console.WriteLine(text);}catch{}
	}


	public void Start(){
		//First Init for Aspect Ratio
		width = (float)Screen.width;
		height = (float)Screen.height;
		scr_scale_w = width / fixed_size_width;
		scr_scale_h = height / fixed_size_height;

		//Set Font Styles
		StyleTitle = new GUIStyle();
		StyleTitle.normal.textColor = Color.red;
		StyleAbout = new GUIStyle();
		StyleAbout.normal.textColor = Color.yellow;
		StyleNormalWhite = new GUIStyle();
		StyleNormalWhite.normal.textColor = Color.white;
		StyleNormalGray = new GUIStyle();
		StyleNormalGray.normal.textColor = Color.gray;
		StyleNormalRed = new GUIStyle();
		StyleNormalRed.normal.textColor = Color.red;
		StyleNormalGreen = new GUIStyle();
		StyleNormalGreen.normal.textColor = Color.green;
		StyleNormalPurple = new GUIStyle();
		StyleNormalPurple.normal.textColor = Color.magenta;
		StyleNormalCyan = new GUIStyle();
		StyleNormalCyan.normal.textColor = Color.cyan;
		StyleSmallWhite = new GUIStyle();
		StyleSmallWhite.normal.textColor = Color.white;
		StyleSmallGray = new GUIStyle();
		StyleSmallGray.normal.textColor = Color.gray;
		StyleSmallRed = new GUIStyle();
		StyleSmallRed.normal.textColor = Color.red;
		StyleSmallGreen = new GUIStyle();
		StyleSmallGreen.normal.textColor = Color.green;
		StyleSmallPurple = new GUIStyle();
		StyleSmallPurple.normal.textColor = Color.magenta;
		StyleSmallCyan = new GUIStyle();
		StyleSmallCyan.normal.textColor = Color.cyan;
		StyleSmallYellow = new GUIStyle();
		StyleSmallYellow.normal.textColor = Color.yellow;

		//Initialize Toggles
		for(int i = 0; i < OptionDetailsToggles.Length; i++) OptionDetailsToggles[i] = false;
		for(int i = 0; i < OptionHotkeyToggles.Length; i++) OptionHotkeyToggles[i] = "";
		for(int i = 0; i < OptionHotkeyCanPress.Length; i++) OptionHotkeyCanPress[i] = true;

		//Set Box Styles
		StyleBoxBlack = new GUIStyle();







		//Directory and Settings create
		if(!Directory.Exists(tas_dir)){
			Directory.CreateDirectory(tas_dir);
			File.WriteAllLines(tas_dir + "\\TestScript.ylt", TestScriptSource.Split('#'));
		}
		if(!Directory.Exists(settings_dir)) Directory.CreateDirectory(settings_dir);
		if(!File.Exists(settings_file)){
			string[] lines = new string[17];

			lines[0] = "NUM_1:";
			lines[1] = "NUM_2:Camera.ChangePositionY(-25)";
			lines[2] = "NUM_3:";
			lines[3] = "NUM_4:Camera.ChangePositionX(-25)";
			lines[4] = "NUM_5:";
			lines[5] = "NUM_6:Camera.ChangePositionX(25)";
			lines[6] = "NUM_7:Camera.ChangePositionZ(-100)";
			lines[7] = "NUM_8:Camera.ChangePositionY(-25)";
			lines[8] = "NUM_9:Camera.ChangePositionZ(100)";
			lines[9] = "F1:Camera.PlayerHasWarped()";
			lines[10] = "F2:None";
			lines[11] = "F3:None";
			lines[12] = "F4:None";
			lines[13] = "F5:None";
			lines[14] = "F6:None";
			lines[15] = "F7:None";
			lines[16] = "F8:None";

			File.WriteAllLines(settings_file, lines);
		}

		string[] key_settings_lines = File.ReadAllLines(settings_file);

		//Read Key Settings
		for(int i = 0; i < key_settings_lines.Length; i++){
			OptionHotkeyToggles[i] = key_settings_lines[i].Split(':')[1];
		}



		//Hotkey Functions List
		Funcs_Player.Add("-Player Functions-");
		Funcs_Player.Add("Player.ChangePositionX(25)");
		Funcs_Player.Add("Player.ChangePositionY(-25)");
		Funcs_Player.Add("Player.ChangePositionZ(100)");
		Funcs_Player.Add("Player.Warp(-23,10,42)");
		Funcs_Player.Add("Player.SetCurrentHealth(99,true)");
		Funcs_Player.Add("Player.AddCurrentHealth(1,true)");
		Funcs_Player.Add("Player.SubCurrentHealth(1,true)");
		
		Funcs_Camera.Add("-Camera Functions-");
		Funcs_Camera.Add("Camera.Unlock()");
		Funcs_Camera.Add("Camera.ChangePositionX(25)");
		Funcs_Camera.Add("Camera.ChangePositionY(-25)");
		Funcs_Camera.Add("Camera.ChangePositionZ(100)");
		Funcs_Camera.Add("Camera.SetPosition(100,200,300)");
		Funcs_Camera.Add("Camera.setExtraLookDisabled()");
		Funcs_Camera.Add("Camera.setRestingPosition()");
		Funcs_Camera.Add("Camera.FOV(55)");
		
		Funcs_Blank3.Add("-Blank 3-");
		Funcs_Blank3.Add("None");

		Funcs_Blank5.Add("-Blank 5-");
		Funcs_Blank3.Add("None");

		Funcs_Blank6.Add("-Blank 6-");
		Funcs_Blank3.Add("None");

		Funcs_Special.Add("-Special Functions-");
		Funcs_Special.Add("Special.PauseGame()");
		Funcs_Special.Add("Special.UnPauseGame()");

		Funcs_Scripts.Add("-Custom Scripts-");
		Funcs_Scripts.Add("Custom.Godmode()");
		Funcs_Scripts.Add("Custom.MaxSpeed()");
		Funcs_Scripts.Add("Custom.MaxJump()");

		//Add the Lists
		Key_functions_list.Add(Funcs_Player);
		Key_functions_list.Add(Funcs_Camera);
		Key_functions_list.Add(Funcs_Blank3);
		Key_functions_list.Add(Funcs_Blank5);
		Key_functions_list.Add(Funcs_Blank6);
		Key_functions_list.Add(Funcs_Special);
		Key_functions_list.Add(Funcs_Scripts);

	}



	public void SuspendGame(){
		if (HudController.instance != null)
		{
			HudController.instance.Pause();
		}
		Singleton<PauseController>.instance.Pause(true);
		AudioEngine.PostEvent("Pause_All", null);
		AudioEngine.PostEvent("Play_Pause_Menu", base.gameObject);
		AudioEngine.PostEvent("Play_Pause_Music", base.gameObject);
	}
	
	public void ResumeGame(){
		Singleton<PauseController>.instance.Pause(false);
		AudioEngine.PostEvent("Resume_All", null);
		AudioEngine.PostEvent("Play_Pause_Menu", base.gameObject);
		AudioEngine.PostEvent("Stop_Pause_Music", base.gameObject);
		if (HudController.instance != null)
		{
			HudController.instance.Unpause();
		}
	}
	
	public void LoadTASScripts(){
		if(Funcs_TAS.Count > 0) Funcs_TAS.Clear();

		Funcs_TAS.Add("-TAS Scripts-");
		Funcs_TAS.Add("TAS.OpenConsole()");
		Funcs_TAS.Add("TAS.CloseConsole()");
		Funcs_TAS.Add("TAS.ClearConsole()");
		Funcs_TAS.Add("TAS.StopScript()");

		string[] load_scripts = Directory.GetFiles(tas_dir);

		for(int i = 0; i < load_scripts.Length; i++){
			string[] file_name = load_scripts[i].Split('\\');
			if(load_scripts[i].Contains(".ylt")) Funcs_TAS.Add("TAS.Execute(" + file_name[file_name.Length-1] + ")");
		}
	}


	//Directory Settings
	public string settings_dir = Directory.GetCurrentDirectory() + @"\xNyuDebug";
	public string settings_file = Directory.GetCurrentDirectory() + @"\xNyuDebug\key_settings.txt";
	public string tas_dir = Directory.GetCurrentDirectory() + @"\xNyuDebug\TAS";

	//Menus
	public bool DebugMenuActivated = false;
	public bool DebugMenuHotkey = false;

	//Option Details
	public bool OptionDetailsActivated = false;
	public bool[] OptionDetailsToggles = new bool[10];
	public List<string> OptionDetailsData = new List<string>();

	//Option Hotkeys
	public bool OptionHotkeysActivated = false;
	public string[] OptionHotkeyToggles = new string[17];
	public bool[] OptionHotkeyCanPress = new bool[17];
	public int OptionHotkeySlot = 0;
	public bool OptionHotkeyMenuActive = false;
	public List<List<string>> Key_functions_list = new List<List<string>>();
	public bool ExtraLook = false;
	public bool CameraUnlock = false;

	//Hotkey Lists
	List<string> Funcs_Player = new List<string>();
	List<string> Funcs_Camera = new List<string>();
	List<string> Funcs_Blank3 = new List<string>();
	List<string> Funcs_TAS = new List<string>();
	List<string> Funcs_Blank5 = new List<string>();
	List<string> Funcs_Blank6 = new List<string>();
	List<string> Funcs_Special = new List<string>();
	List<string> Funcs_Scripts = new List<string>();

	//Custom Scripts
	public bool ScriptGodMode = false;
	public bool ScriptGodModeInit = false;
	public bool ScriptMaxSpeed = false;
	public bool ScriptMaxJump = false;

	//Styles
	public GUIStyle StyleBoxBlack;
	public GUIStyle StyleTitle;
	public GUIStyle StyleAbout;
	public GUIStyle StyleNormalWhite;
	public GUIStyle StyleNormalGray;
	public GUIStyle StyleNormalRed;
	public GUIStyle StyleNormalGreen;
	public GUIStyle StyleNormalPurple;
	public GUIStyle StyleNormalCyan;
	public GUIStyle StyleSmallWhite;
	public GUIStyle StyleSmallGray;
	public GUIStyle StyleSmallRed;
	public GUIStyle StyleSmallGreen;
	public GUIStyle StyleSmallPurple;
	public GUIStyle StyleSmallCyan;
	public GUIStyle StyleSmallYellow;

	//Display Settings
	public float fixed_size_width = 3840;
	public float fixed_size_height = 2160;

	public float width = 0;
	public float height = 0;
	public float scr_scale_w = 0;
	public float scr_scale_h = 0;

	//Performance
	public int skip_1_frame = 0;
	public int skip_2_frame = 0;

	public string debugstring = "";
	public bool debugbool = false;

	//TAS
	public string ScriptPath = "";
	public bool ConsoleActivate = false;

	public string TestScriptSource = "frame{jump();Camera(0,0,0)}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump()}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();basicattack()}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{jump();walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{crouch()}#frame{walk(10,10,10)}#frame{walk(10,10,10)}#frame{jump()}#frame{camera(90,158,0)}#frame{jump()}#frame{camera(36,0,0)}";


}


