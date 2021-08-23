using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Rewired;

public class xNyuTAS : MonoBehaviour
{
	public void Update(){
		if(ScriptIteration >= ScriptLines.Length){
			ScriptEnd = true;
		}

		if(!ScriptEnd){
			//TAS Routine

			//Clean Key Checkers
			for(int i = 0; i < AllInputsCheck.Length; i++) AllInputsCheck[i] = false;
			StickL = Vector3.zero;
			StickR = Vector3.zero;
			if(ScriptLines[ScriptIteration].Contains("repeat(")){
				RepeatFrames = int.Parse(ScriptLines[ScriptIteration].Replace("repeat(", "").Split(')')[0]);
			}
			if(WaitFrames <= 0){
				if(ScriptLines[ScriptIteration].Contains("frame{")){
					string[] frame = ScriptLines[ScriptIteration].Replace("frame{", "").Replace("}", "").Split(';');

					if(frame.Length > 0){
						foreach(string action in frame){
							//Detect what should be pessed

							if(action == "jump()"){
								AllInputsCheck[1] = true;
							}else if(action == "fly()"){
								AllInputsCheck[2] = true;
							}else if(action == "context()"){
								AllInputsCheck[3] = true;
							}else if(action == "crouch()"){
								AllInputsCheck[4] = true;
							}else if(action == "wheel()"){
								AllInputsCheck[5] = true;
							}else if(action == "basicattack()"){
								AllInputsCheck[6] = true;
							}else if(action == "swimunderwater()"){
								AllInputsCheck[7] = true;
							}else if(action == "swimunderwateralt()"){
								AllInputsCheck[8] = true;
							}else if(action == "sonarblastattack()"){
								AllInputsCheck[9] = true;
							}else if(action == "sonarboomattack()"){
								AllInputsCheck[10] = true;
							}else if(action == "sonarshieldattack()"){
								AllInputsCheck[11] = true;
							}else if(action == "aiming()"){
								AllInputsCheck[12] = true;
							}else if(action == "shooteatenitem()"){
								AllInputsCheck[13] = true;
							}else if(action == "tongueedibleitem()"){
								AllInputsCheck[14] = true;
							}else if(action == "invisibility()"){
								AllInputsCheck[15] = true;
							}else if(action == "wheelspin()"){
								AllInputsCheck[16] = true;
							}else if(action == "groundpound()"){
								AllInputsCheck[17] = true;
							}else if(action == "fartbubble()"){
								AllInputsCheck[18] = true;
							}else if(action == "emotehappy()"){
								AllInputsCheck[19] = true;
							}else if(action == "emotetaunt()"){
								AllInputsCheck[20] = true;
							}else if(action == "emotedisappointed()"){
								AllInputsCheck[21] = true;
							}else if(action == "emoteangry()"){
								AllInputsCheck[22] = true;
							}else if(action.Contains("walk")){
								string[] vector_split = action.Replace(")", "").Split('(')[1].Split(',');
								StickL = new Vector3(float.Parse(vector_split[0]), float.Parse(vector_split[1]), float.Parse(vector_split[2]));
							}else if(action.Contains("camera")){
								string[] camera_split = action.Replace(")", "").Split('(')[1].Split(',');
								//GetComponent Camera
								FollowCamera FCamera = (FollowCamera)FindObjectOfType<CameraManager>().GetPlayerCamera(PlayerCameraTypes.Follow);
								FCamera.AskSetCameraPosition(new Vector3(float.Parse(camera_split[0]), float.Parse(camera_split[1]), float.Parse(camera_split[2])));
							}
						}
					}
				}else{
					//Wait
					WaitFramesToSet = int.Parse(ScriptLines[ScriptIteration].Replace("wait{", "").Replace("}", "")) - 1;
					WaitExtraFramesResult += WaitFramesToSet;
				}
			}

			//Execute Presses/Non-Presses
			InputHandler.MoveStick.SetFakedInput(true, StickL);
			InputHandler.Jump.SetFakedInput(true, AllInputsCheck[1]);
			InputHandler.Fly.SetFakedInput(true, AllInputsCheck[2]);
			InputHandler.Context.SetFakedInput(true, AllInputsCheck[3]);
			InputHandler.Crouch.SetFakedInput(true, AllInputsCheck[4]);
			InputHandler.Wheel.SetFakedInput(true, AllInputsCheck[5]);
			InputHandler.BasicAttack.SetFakedInput(true, AllInputsCheck[6]);
			InputHandler.SwimUnderwater.SetFakedInput(true, AllInputsCheck[7]);
			InputHandler.SwimUnderwaterAlt.SetFakedInput(true, AllInputsCheck[8]);
			InputHandler.SonarBlastAttack.SetFakedInput(true, AllInputsCheck[9]);
			InputHandler.SonarBoomAttack.SetFakedInput(true, AllInputsCheck[10]);
			InputHandler.SonarShieldAttack.SetFakedInput(true, AllInputsCheck[11]);
			InputHandler.Aiming.SetFakedInput(true, AllInputsCheck[12]);
			InputHandler.ShootEatenItem.SetFakedInput(true, AllInputsCheck[13]);
			InputHandler.TongueEdibleItem.SetFakedInput(true, AllInputsCheck[14]);
			InputHandler.Invisibility.SetFakedInput(true, AllInputsCheck[15]);
			InputHandler.WheelSpin.SetFakedInput(true, AllInputsCheck[16]);
			InputHandler.GroundPound.SetFakedInput(true, AllInputsCheck[17]);
			InputHandler.FartBubble.SetFakedInput(true, AllInputsCheck[18]);
			InputHandler.EmoteHappy.SetFakedInput(true, AllInputsCheck[19]);
			InputHandler.EmoteTaunt.SetFakedInput(true, AllInputsCheck[20]);
			InputHandler.EmoteDisappointed.SetFakedInput(true, AllInputsCheck[21]);
			InputHandler.EmoteAngry.SetFakedInput(true, AllInputsCheck[22]);

			//Debug Message
			if(ScriptLines.Length > 10){
				if(ScriptIteration > 0 && ScriptIteration % ((int)Math.Round((float)ScriptLines.Length / 10f)) == 0){
					ScriptProgress++;
					WriteConsole("TAS Progress: " + (ScriptProgress * 10).ToString() + "%");
				}
			}
			
			//Increase Iterator
			if(WaitFrames > 0){
				WaitFrames--;
			}else{
				ScriptIteration++;
				WaitFrames = WaitFramesToSet;
				WaitFramesToSet = 0;
			}

		}else{
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

			//Script End
			WriteConsole("\n--- TAS finished ---\n");
			//WriteConsole("Duration: " + (DateTime.Now - ScriptStartTime).ToString("hh:mm:ss.FFF"));
			WriteConsole("Frames: " + (ScriptIteration + 1 + WaitExtraFramesResult).ToString() + "\n");
			Destroy(this.gameObject);
		}


	}

	public void Start(){
		//Get Script Path from DebugMenu
		xNyuDebug DebugMenu = FindObjectOfType<xNyuDebug>();
		ScriptPath = DebugMenu.ScriptPath;

		//Read Lines from Script
		if(!File.Exists(ScriptPath)){
			WriteConsole("--- ERROR ---");
			WriteConsole("File " + @ScriptPath + " was not found!\n");
			Destroy(this.gameObject);
		}else{
			ScriptLines = File.ReadAllLines(ScriptPath);

			List<string> ScriptLinesProcess = new List<string>(ScriptLines);
			for(int i = 0; i < ScriptLinesProcess.Count; i++){
				ScriptLinesProcess[i] = ScriptLinesProcess[i].ToLower();
				if(ScriptLinesProcess[i].Length < 5) ScriptLinesProcess.RemoveAt(i);
			}

			ScriptLines = ScriptLinesProcess.ToArray();

			//Start Message
			WriteConsole("\n--- TAS START: " + ScriptPath.Split('\\')[ScriptPath.Split('\\').Length - 1] + " ---\n");
			WriteConsole(ScriptLines[0].Contains("wait") ? "Wait " + ScriptLines[0].Replace("wait{", "").Replace("}", "") + " frames before start...": "");

			//Start Settings
			bool ScriptEnd = false;
			int ScriptIteration = 0;
			ScriptStartTime = DateTime.Now;
			ScriptProgress = 0;
			WaitFrames = 0;
			WaitFramesToSet = 0;
			WaitExtraFramesResult = 0;
			RepeatFrames = 0;

			//Get Current Input Object
			InputHandler = FindObjectOfType<PlayerLogic>().InputStore;

			AllInputsCheck = new bool[23];
		}
	}

	public void WriteConsole(object text){
		try{Console.WriteLine(text);}catch{}
	}

	//Setings File

	//Settings TAS
	public string ScriptPath = "";
	public string[] ScriptLines;
	public bool ScriptEnd = false;
	public bool[] AllInputsCheck;
	public int ScriptIteration = 0;
	public int ScriptProgress;
	public int RepeatFrames;
	public int WaitFrames;
	public int WaitFramesToSet;
	public int WaitExtraFramesResult;
	public DateTime ScriptStartTime;
	public Vector3 StickL;
	public Vector3 StickR;
	PlayerInputStore InputHandler;
}


