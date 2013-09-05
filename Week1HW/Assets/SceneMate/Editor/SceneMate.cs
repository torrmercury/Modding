/*-----------------------------------------------------------
 Unity Editor Extension
 
 SceneMate
 
 Created By: Tim Wiese
 
 Version 1.1.2
 
 Last Edit: 1/1/2013
-----------------------------------------------------------*/

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SceneMate : EditorWindow
{
	// Hot Keys
	private bool assignHotKeysDropDown;
	private bool assignHKwindowResize;
	private bool assignNextKey;
	private bool[] toolAssignKey = new bool[32];
	private KeyCode[] toolKeyCodes = 
	{KeyCode.F1,KeyCode.F2,KeyCode.G,KeyCode.None,KeyCode.C,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.Alpha1,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None};
	private string[] toolKeyStrings = new string[32];
	private string[] toolPrefStrings = {"QtBtn1","QtBtn2","QtBtn3","QtBtn4","QtBtn5",
		"QtBtn6","QtBtn7","QtBtn8","QtBtn9","QtBtn10","QtBtn11","QtBtn12","QtBtn13","QtBtn14","QtBtn15","QtBtn16","QtBtn17","QtBtn18","QtBtn19","QtBtn20","QtBtn21","QtBtn22","QtBtn23"
		,"QtBtn24","QtBtn25","QtBtn26","QtBtn27","QtBtn28","QtBtn29","QtBtn30","QtBtn31","QtBtn31"};
	private string[] hkBtnNames = {"Rotate Clockwise","Rotate Counter Clockwise","Snap to Ground","Snap to Wall",
		"Snap to Center","Reset Objects","Match Position Target ","Match Rotation Target",
		"Match Scale Target","Offset Selected","Selected Objects Amount","Snap to Surface","Flip X","Flip Y","Flip Z","Object Painter","Fit Collider",
		"Cycle Rotation Axis","Replace Selection","Color Swatches","Snap Drag", "Align to Normal","Snap Rotation 90", "Randomize Rotation", "Randomize Scale", "Randomize Position",
		"Distribute","Align High","Align Low","Set Spacing","Bounds Spacing","Bounds Snapping"};
	private bool[] modControl = new bool[32];
	private bool[] modShift = new bool[32];
	private bool[] modAlt = new bool[32];
	private int assignHKsel;
	
	//Popup Menus
	private string[] rotDir =
	{"(X)","(Y)","(Z)"};
	private string[] snapDir =
	{"-X","+X","-Y","+Y","-Z","+Z"};
	private Vector3[] snapDirVectors = 
	{Vector3.left,Vector3.right,Vector3.down,Vector3.up,Vector3.back,Vector3.forward};
	private int rotAxis = 1;
	private int stgDir = 2;
	private int stwDir = 4;
	private int stcDir = 2;
	
	//Offset Selected
	private float offsetXAmount;
	private float offsetYAmount;
	private float offsetZAmount;
	private string[] offsetStyle =
	{"Add","Multiply","Divide"};
	private int curOffsetStyle;
	
	//Rotation Stuff
	private bool snapRotation90;
	private float rotationAmount = 10f;
	private int rotateCurAxis;
	
	//Option Menu
	public static bool materialImport = true;
	public static bool animationImport = true;
	private bool importOptionsDropDown;
	private bool optionsWindowResize;
	
	//Color Swatches
	private bool colorSwatches;
	private bool CSwindowResize;
	private Color color1 = new Color(1,1,1,1);
	private Color curColor;
	private int removeColor;
	private List<Color> colorList = new List<Color>();
	
	//Fit Collider
	private GameObject colSelectedObj = null;
	private Object[] colSelectedObjArr = new Object[1];
	
	//Object Painter Variables
	private bool objPainter;
	private bool wasMouseDown;
	private bool selectLast;
	private bool painterKeyDown;
	private bool assignParent;
	private bool randRot;
	private bool randScale;
	private bool uniformScale = true;
	private float randScaleMin = .5f;
	private float randScaleMax = 2;
	private bool randRotX;
	private bool randRotY = true;
	private bool randRotZ;
	private float randomRotSnap = 0;
	private bool OPwindowResize;
	private bool eraserMode;
	private int removeObj;
	private int rowWidth = 3;
	private int curObject;
	private string[] objNames;
	private GameObject newObject;
	private GameObject parentObj = null;
	private Object[] parentObjs = new Object[1];
	private List<Object> objs = new List<Object>();
	
	//Match Variables
	private bool waitForSelMatch = false;
	private bool matchPos;
	private bool matchRot;
	private bool matchScale;
	private bool grabSource;
	private GameObject[] sourceObjs;
	
	private bool posMatchX = true;
	private bool posMatchY = true;
	private bool posMatchZ = true;
	private bool rotMatchX = true;
	private bool rotMatchY = true;
	private bool rotMatchZ = true;
	private bool scaleMatchX = true;
	private bool scaleMatchY = true;
	private bool scaleMatchZ = true;
	
	//Snap Stuff
	private Vector3 handlePos = new Vector3(0,0,0);
	private Vector3 handleNormal = new Vector3(0,0,0);
	private bool snapSurface;
	private bool boundsSnapping;
	private bool alignToNormal = true;
	private bool snapDrag;
	private bool dragDirNegX = true;
	private bool dragDirPosX = true;
	private bool dragDirNegY = true;
	private bool dragDirPosY = true;
	private bool dragDirNegZ = true;
	private bool dragDirPosZ = true;
	private Vector3 wallSnapRayStart;
	private float snapDragThreshold = 1;
	private Vector3 boundsObjPos = new Vector3(0,0,0);
	private Transform lclBoundsObj;
	private float snapOffset;
	
	//Distribute and Align
	private bool disX = true;
	private bool disY;
	private bool disZ;
	private bool disBySize;
	private bool setSpacing;
	private float disSpacing;
	private bool reverseDis;
	private bool alignX = true;
	private bool alignY;
	private bool alignZ;
	
	//Button Icons and Tooltips
	private string[] toolTips =
	{"Rotate Clockwise","Rotate Counter Clockwise","Snap to Ground: \nGlobal Axis","Snap to Wall: \nLocal Axis",
		"Snap to Center: \nGlobal Axis ","Reset Objects: \nReset Position, Rotation, Scale","Replace Selection","Match Position Target","Match Rotation Target",
		"Match Scale Target","Offset Selected","Debug Log Selected Objects","Snap to Surface","Flip X","Flip Y",
		"Flip Z","Object Painter","Fit Collider to Mesh","Cycle Rotation Axis","Align To Normal","Snap Rotation 90",
		"Snap Drag","Color Swatches","Assign Hot Keys","Import Options","Randomize Rotation Selected","Randomize Scale Selected","Randomize Position Selected",
		"Distribute","Align High","Align Low","Set Spacing","Space Object By Size, Using Object Bounds","Snap Using Object Bounds"};
	private static string[] buttonFileNames =
	{"Rotate Left_00","Rotate Right_01","Snap to Ground_02","Snap to Wall_03",
		"Snap to Center_04","Reset Objects_05","Replace Selection_06","Match Pos Tgt_07","Match Rot Tgt_08",
		"Match Scale Tgt_09","Offset Selected_10","Selected Objs Amt_11","Snap to Surface_12","Flip X_13",
		"Flip Y_14","Flip Z_15","Object Painter_16","Fit Collider_17","Cycle Rotation Axis_18","Align To Normal_19",
		"Snap Rotation 90_20","Snap Drag_21","Color Swatches_22","Assign Hot Keys_23","Import Options_24",
		"Randomize Rotation_25","Randomize Scale_26","Randomize Position_27","Distribute_28","Align High_29","Align Low_30",
		"Set Spacing_31","By Size_32","Bounds Snap_33"};
	private GUIContent[] guiContent = new GUIContent[34];
	private GUISkin customSkin;
	private GUISkin customSkin2;
	private GUISkin customSkin3;
	
	//GUI Groups
	private int optPixelHeight;
	private int pixelHeight;
	
	//Scroll Bars
	private float vScrollPos;
	
	//Replace Selection
	private bool inheritPos = true;
	private bool inheritRot = true;
	private bool inheritScale = true;
	private bool keepOrig;
	
	//Layer Masking
	public static List<string> layers;
	public static List<int> layerNumbers;
	public static string[] layerNames;
	public static long lastUpdateTick;
	private LayerMask snapsMask = -1;
	private LayerMask painterMask = -1;
	
	//Randomize Variables
	private string[] axis =
	{"Global","Local"};
	private int randRotCurAxis;
	private int randPosCurAxis;
	private bool scaleX = true;
	private bool scaleY = true;
	private bool scaleZ = true;
	private bool rotX = true;
	private bool rotY = true;
	private bool rotZ = true;
	private bool positionX = true;
	private bool positionY = true;
	private bool positionZ = true;
	private bool randScaleUniform = true;
	private float randomizeScaleMin = .5f;
	private float randomizeScaleMax = 2;
	private float randomizePosMin = .5f;
	private float randomizePosMax = 2;
	private float randomizeRotSnap = 0;
	
	//Replace Selection
	private int offsetSelCurAxis;
	
	//Selected Amount Debug
	private List<string> componentNames = new List<string>();
	private List<int> componentNumber = new List<int>();
	private List<string> tagNames = new List<string>();
	private List<int> tagNumber = new List<int>();
	private List<string> layerNamesList = new List<string>();
	private List<int> layerNumberList = new List<int>();
	private int vertCount;
	
	//Method Names
	private string[] methodNames = {
		"RotateLeft","RotateRight","SnapToGround","SnapToWall","SnapToCenter","ResetObjs","MatchPosToggle","MatchRotToggle","MatchScaleToggle","OffsetSel","SelAmt",
		"","FlipX","FlipY","FlipZ","ObjPainterToggle","FitCollider","CycleRotationAxis","ReplaceSelection","ColorSwatchesToggle","SnapDragToggle","AlignToNormalToggle",
		"SnapRotation90Toggle","RandomizeRotation","RandomizeScale","RandomizePosition","Distribute","AlignHigh","AlignLow","SetSpacingToggle","BySizeToggle","BoundsSnapToggle"};
	
	public void OnEnable()
	{	
		customSkin = ScriptableObject.CreateInstance<GUISkin>();
		customSkin2 = ScriptableObject.CreateInstance<GUISkin>();
		customSkin3 = ScriptableObject.CreateInstance<GUISkin>();
		waitForSelMatch = false;
		grabSource = false;
		//Check for and get saved values
		if (EditorPrefs.HasKey("materialImport"))
			materialImport = EditorPrefs.GetBool("materialImport");	
		if (EditorPrefs.HasKey("animationImport"))	
			animationImport = EditorPrefs.GetBool("animationImport");
		if (EditorPrefs.HasKey("objPainter"))
			objPainter = EditorPrefs.GetBool("objPainter");
		if (EditorPrefs.HasKey("assignHotKeysDropDown"))
			assignHotKeysDropDown = EditorPrefs.GetBool("assignHotKeysDropDown");
		if (EditorPrefs.HasKey("importOptionsDropDown"))
			importOptionsDropDown = EditorPrefs.GetBool("importOptionsDropDown");
		if (EditorPrefs.HasKey("rotAxis"))	
			rotAxis = EditorPrefs.GetInt("rotAxis");;
		if (EditorPrefs.HasKey("stgDir"))	
			stgDir = EditorPrefs.GetInt("stgDir");
		if (EditorPrefs.HasKey("stwDir"))	
			stwDir = EditorPrefs.GetInt("stwDir");
		if (EditorPrefs.HasKey("stcDir"))	
			stcDir = EditorPrefs.GetInt("stcDir");
		if (EditorPrefs.HasKey("rowWidth"))	
			rowWidth = EditorPrefs.GetInt("rowWidth");
		if (EditorPrefs.HasKey("snapsMask"))	
			snapsMask = EditorPrefs.GetInt("snapsMask");
		if (EditorPrefs.HasKey("painterMask"))	
			painterMask = EditorPrefs.GetInt("painterMask");
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			toolKeyStrings[i] = toolKeyCodes[i].ToString();	
		}
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			if (EditorPrefs.HasKey(toolPrefStrings[i]))
			{
				toolKeyStrings[i] = EditorPrefs.GetString(toolPrefStrings[i]);
				toolKeyCodes[i] = (KeyCode)System.Enum.Parse( typeof(KeyCode), toolKeyStrings[i]);
			}
		}
		
		//Create the editor prefs for modfiers if they don't already exist, "SMmodControl0" is just one of the many that should already exist
		if(!EditorPrefs.HasKey("SMmodControl0"))
		{
			for(int i = 0;i < hkBtnNames.Length;i++)
			{
				//Set Control Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodControl" + i.ToString(),modControl[i]);
				//Set Shift Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodShift" + i.ToString(),modShift[i]);
				//Set Alt Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodAlt" + i.ToString(),modAlt[i]);
			}
		}
		//Get all the editor prefs for the modifiers
		for(int i = 0;i < hkBtnNames.Length;i++)
		{
			modControl[i] = EditorPrefs.GetBool("SMmodControl" + i.ToString());
			modShift[i] = EditorPrefs.GetBool("SMmodShift" + i.ToString());
			modAlt[i] = EditorPrefs.GetBool("SMmodAlt" + i.ToString());
		}
		
		
		//Load Icon Images
		for(int i = 0; i < buttonFileNames.Length; i++)
		{
			//load all the icons in the GUI content
			guiContent[i] = new GUIContent("",Resources.LoadAssetAtPath("Assets/SceneMate/Icons/" + buttonFileNames[i].Substring(0,buttonFileNames[i].Length - 3) + ".psd",typeof(Texture)) as Texture,toolTips[i]);			
		}
	}
	
	[MenuItem("Window/SceneMate")]
	static void Init()
	{
		//Create the window
		SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
		window.minSize = new Vector2(842, 98);
		window.maxSize = new Vector2(843, 98);
		window.title = ("SceneMate");
	}
	
	public void OnGUI()
	{
		//Custom Styles
		customSkin.button =  new GUIStyle(GUI.skin.button);
		customSkin.button.padding = new RectOffset(0,0,0,0);
		
		customSkin2.button =  new GUIStyle(GUI.skin.button);
		customSkin2.button.padding = new RectOffset(3,3,1,1);
		customSkin2.button.fontSize = 8;
		customSkin2.button.fontStyle = FontStyle.Bold;
		
		customSkin2.label = new GUIStyle(GUI.skin.label);
		customSkin2.label.fontSize = 12;
		customSkin2.label.fontStyle = FontStyle.Bold;
		
		customSkin2.toggle = new GUIStyle(GUI.skin.toggle);
		customSkin2.toggle.fontSize = 9;
		
		customSkin.toggle = new GUIStyle(GUI.skin.button);
		customSkin.toggle.padding = new RectOffset(0,0,0,0);
		
		customSkin.textArea = new GUIStyle(GUI.skin.textArea);
		
		customSkin.label = new GUIStyle(GUI.skin.label);
		customSkin.label.fontSize = 9;
		
		customSkin3.label = new GUIStyle(GUI.skin.label);
		customSkin3.label.fontSize = 9;
		customSkin3.label.fontStyle = FontStyle.Bold;
		
		customSkin3.toggle = new GUIStyle(GUI.skin.button);
		customSkin3.toggle.padding = new RectOffset(0,0,0,0);
		customSkin3.toggle.fontSize = 9;
		customSkin3.toggle.fontStyle = FontStyle.Bold;
		
		//onSceneGUIDelegate allows use of controls while focused in the viewport and not focused on the GUI.
		if(SceneView.onSceneGUIDelegate != this.OnSceneGUI)
	    {
	       SceneView.onSceneGUIDelegate = this.OnSceneGUI;
	    }
		
		//First Group of Buttons goes to the color swatches and object painter from the top.
		GUI.BeginGroup(new Rect(1,0,1000,530));

		if (GUI.Button(new Rect(2,60,34,34),guiContent[13],customSkin.button))
			FlipX();
		
		if (GUI.Button(new Rect(2,2,34,34),guiContent[7],customSkin.button))
		{
			if(!waitForSelMatch && ! matchPos)
			{
				if(Selection.transforms.Length > 0)
				{
					matchPos = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchPos();
			}
		}
		posMatchX = GUI.Toggle(new Rect(3,36,12,14),posMatchX,"x",customSkin3.toggle);
		posMatchY = GUI.Toggle(new Rect(13,36,12,14),posMatchY,"y",customSkin3.toggle);
		posMatchZ = GUI.Toggle(new Rect(23,36,12,14),posMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(36,60,34,34),guiContent[14],customSkin.button))
			FlipY();
	
		if (GUI.Button(new Rect(36,2,34,34),guiContent[8],customSkin.button))
		{
			if(!waitForSelMatch && !matchRot)
			{
				if(Selection.transforms.Length > 0)
				{
					matchRot = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchRot();
			}
		}
		rotMatchX = GUI.Toggle(new Rect(37,36,12,14),rotMatchX,"x",customSkin3.toggle);
		rotMatchY = GUI.Toggle(new Rect(47,36,12,14),rotMatchY,"y",customSkin3.toggle);
		rotMatchZ = GUI.Toggle(new Rect(57,36,12,14),rotMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(70,60,34,34),guiContent[15],customSkin.button))
			FlipZ();
		
		if (GUI.Button(new Rect(70,2,34,34),guiContent[9],customSkin.button))
		{
			if(!waitForSelMatch && !matchScale)
			{
				if(Selection.transforms.Length > 0)
				{
					matchScale = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchScale();
			}
		}
		scaleMatchX = GUI.Toggle(new Rect(71,36,12,14),scaleMatchX,"x",customSkin3.toggle);
		scaleMatchY = GUI.Toggle(new Rect(81,36,12,14),scaleMatchY,"y",customSkin3.toggle);
		scaleMatchZ = GUI.Toggle(new Rect(91,36,12,14),scaleMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(106,2,34,30),guiContent[5],customSkin.button))
			ResetObjs();
		if (GUI.Button(new Rect(104,32,14,20),"P",customSkin.button))
			ResetObjsPos();	
		if (GUI.Button(new Rect(116,32,14,20),"R",customSkin.button))
			ResetObjsRot();
		if (GUI.Button(new Rect(128,32,14,20),"S",customSkin.button))
			ResetObjsScale();
		
		
		if (GUI.Button(new Rect(106,60,34,34),guiContent[11],customSkin.button))
			SelAmt();
		
		//Divder made out of empty group
		GUI.BeginGroup(new Rect(141,0,106,96),customSkin.textArea);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(141,58,209,38),customSkin.textArea);
		GUI.EndGroup();
		
		//rand rotation buttons
		if (GUI.Button(new Rect(144,60,34,34),guiContent[25],customSkin.button))
			RandomizeRotation();
		
		rotX = GUI.Toggle(new Rect(178,58,14,12),rotX,"x",customSkin3.toggle);
		rotY = GUI.Toggle(new Rect(178,70,14,12),rotY,"y",customSkin3.toggle);
		rotZ = GUI.Toggle(new Rect(178,82,14,12),rotZ,"z",customSkin3.toggle);
		
		GUI.Label(new Rect(219,64,84,80),"snap",customSkin.label);
		randomizeRotSnap = EditorGUI.FloatField(new Rect(192,62,30,16),"", randomizeRotSnap);
		randRotCurAxis = EditorGUI.Popup(new Rect(196,80,50,16),"",randRotCurAxis,axis);
		
		//Divder made out of empty group
		GUI.BeginGroup(new Rect(0,54,141,2),customSkin.textArea);
		GUI.EndGroup();
		
		//Rand Scale Button
		if (GUI.Button(new Rect(250,60,34,34),guiContent[26],customSkin.button))
			RandomizeScale();
		
		scaleX = GUI.Toggle(new Rect(284,58,14,12),scaleX,"x",customSkin3.toggle);
		scaleY = GUI.Toggle(new Rect(284,70,14,12),scaleY,"y",customSkin3.toggle);
		scaleZ = GUI.Toggle(new Rect(284,82,14,12),scaleZ,"z",customSkin3.toggle);
		
		randomizeScaleMin = EditorGUI.FloatField(new Rect(298,60,30,16),"", randomizeScaleMin);
		GUI.Label(new Rect(325,57,84,80),"min",customSkin.label);
		GUI.Label(new Rect(325,80,84,80),"max",customSkin.label);
		randomizeScaleMax = EditorGUI.FloatField(new Rect(298,76,30,16),"", randomizeScaleMax);
		
		randScaleUniform = GUI.Toggle(new Rect(328,67,16,16),randScaleUniform, "");
		
		//Rand Position Button
		if (GUI.Button(new Rect(144,2,34,34),guiContent[27],customSkin.button))
			RandomizePosition();
		
		positionX = GUI.Toggle(new Rect(178,0,14,12),positionX,"x",customSkin3.toggle);
		positionY = GUI.Toggle(new Rect(178,12,14,12),positionY,"y",customSkin3.toggle);
		positionZ = GUI.Toggle(new Rect(178,24,14,12),positionZ,"z",customSkin3.toggle);
		
		randomizePosMin = EditorGUI.FloatField(new Rect(192,2,30,16),"", randomizePosMin);
		GUI.Label(new Rect(220,4,84,80),"min",customSkin.label);
		GUI.Label(new Rect(220,20,84,80),"max",customSkin.label);
		randomizePosMax = EditorGUI.FloatField(new Rect(192,18,30,16),"", randomizePosMax);
		
		randPosCurAxis = EditorGUI.Popup(new Rect(196,38,50,16),"",randPosCurAxis,axis);
		
		//Divder
		GUI.BeginGroup(new Rect(250,0,100,55),customSkin.textArea);
		
		if (GUI.Button(new Rect(6,2,34,34),guiContent[17],customSkin.button))
			FitCollider();
		
		keepOrig = GUI.Toggle(new Rect(76,25,12,12),keepOrig,"O",customSkin3.toggle);
		inheritPos = GUI.Toggle(new Rect(46,25,12,12),inheritPos,"P",customSkin3.toggle);
		inheritRot = GUI.Toggle(new Rect(56,25,12,12),inheritRot,"R",customSkin3.toggle);
		inheritScale = GUI.Toggle(new Rect(66,25,12,12),inheritScale,"S",customSkin3.toggle);
		if (GUI.Button(new Rect(50,2,34,24),guiContent[6],customSkin.button))
			ReplaceSelection();
		
		colSelectedObjArr[0] = EditorGUI.ObjectField(new Rect(3,39,95,15),"", colSelectedObjArr[0], typeof(GameObject), true) as GameObject;
		colSelectedObj = colSelectedObjArr[0] as GameObject;
		
		GUI.EndGroup();
		
		//Divder
		GUI.BeginGroup(new Rect(353,0,83,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(4,2,34,34),guiContent[10],customSkin.button))
			OffsetSel();
		
		offsetSelCurAxis = EditorGUI.Popup(new Rect(16,56,64,16),"",offsetSelCurAxis,axis);
		
		curOffsetStyle = EditorGUI.Popup(new Rect(16,72,64,16),"",curOffsetStyle,offsetStyle);
		
		GUI.Label(new Rect(38,2,30,16),"X");
		offsetXAmount = EditorGUI.FloatField(new Rect(49,2,30,16),"", offsetXAmount);
		GUI.Label(new Rect(38,18,30,16),"Y");
		offsetYAmount = EditorGUI.FloatField(new Rect(49,18,30,16),"", offsetYAmount);
		GUI.Label(new Rect(38,34,30,16),"Z");
		offsetZAmount = EditorGUI.FloatField(new Rect(49,34,30,16),"", offsetZAmount);
		
		GUI.EndGroup();
		
		//Divder
		GUI.BeginGroup(new Rect(439,0,162,96),customSkin.textArea);
		
		snapDrag = GUI.Toggle(new Rect(16,2,34,34),snapDrag,guiContent[21],customSkin.toggle);
		
		GUI.Label(new Rect(4,35,30,16),"-",customSkin2.label);
		GUI.Label(new Rect(3,48,30,16),"+",customSkin2.label);
		dragDirNegX = GUI.Toggle(new Rect(14,36,14,14),dragDirNegX,"x",customSkin3.toggle);
		dragDirPosX = GUI.Toggle(new Rect(14,48,14,14),dragDirPosX,"x",customSkin3.toggle);
		dragDirNegY = GUI.Toggle(new Rect(26,36,14,14),dragDirNegY,"y",customSkin3.toggle);
		dragDirPosY = GUI.Toggle(new Rect(26,48,14,14),dragDirPosY,"y",customSkin3.toggle);
		dragDirNegZ = GUI.Toggle(new Rect(38,36,14,14),dragDirNegZ,"z",customSkin3.toggle);
		dragDirPosZ = GUI.Toggle(new Rect(38,48,14,14),dragDirPosZ,"z",customSkin3.toggle);
		
		GUI.Label(new Rect(19,78,60,16),"Thld",customSkin.label);
		snapDragThreshold = EditorGUI.FloatField(new Rect(17,64,33,15),"", snapDragThreshold);
		if(snapDragThreshold < .1f)
			snapDragThreshold = .1f;
		if(snapDragThreshold > 9999)
			snapDragThreshold = 9999;
		
		if (GUI.Button(new Rect(54,2,34,34),guiContent[3],customSkin.button))
			SnapToWall();
		stwDir = EditorGUI.Popup(new Rect(54,36,34,16),"",stwDir,snapDir);
		
		GUI.Label(new Rect(54,78,60,16),"Offst",customSkin.label);
		snapOffset = EditorGUI.FloatField(new Rect(54,64,33,15),"", snapOffset);
		if(snapOffset < -9999)
			snapOffset = -9999;
		if(snapOffset > 9999)
			snapOffset = 9999;
		
		if (GUI.Button(new Rect(88,2,34,34),guiContent[4],customSkin.button))
			SnapToCenter();
		stcDir = EditorGUI.Popup(new Rect(88,36,34,16),"",stcDir,snapDir);
		
		if (GUI.Button(new Rect(122,2,34,34),guiContent[2],customSkin.button))
			SnapToGround();
		stgDir = EditorGUI.Popup(new Rect(122,36,34,16),"",stgDir,snapDir);
		
		alignToNormal = GUI.Toggle(new Rect(88,54,34,17),alignToNormal,guiContent[19],customSkin.toggle);
		boundsSnapping = GUI.Toggle(new Rect(88,71,34,17),boundsSnapping,guiContent[33],customSkin.toggle);
		
		snapSurface = GUI.Toggle(new Rect(122,54,34,34),snapSurface,guiContent[12],customSkin.toggle);
		
		GUI.EndGroup();
		
		if (snapRotation90)
			rotationAmount = 90f;
		
		if (snapRotation90 == false)
			rotationAmount = 10f;
		
		//Divder
		GUI.BeginGroup(new Rect(604,0,74,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(3,2,34,34),guiContent[0],customSkin.button))
			RotateLeft();

		if (GUI.Button(new Rect(3,42,34,34),guiContent[1],customSkin.button))
			RotateRight();
		
		rotateCurAxis = EditorGUI.Popup(new Rect(3,77,50,16),"",rotateCurAxis,axis);
		
		rotAxis = EditorGUI.Popup(new Rect(37,2,34,16),"",rotAxis,rotDir);
		snapRotation90 = GUI.Toggle(new Rect(37,28,24,24),snapRotation90, guiContent[20],customSkin.toggle);
		
		GUI.EndGroup();
		
		//Divder
		GUI.BeginGroup(new Rect(681,0,84,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(7,2,34,34),guiContent[28],customSkin.button))
			Distribute();
		disX = GUI.Toggle(new Rect(8,36,12,14),disX,"x",customSkin3.toggle);
		if(disX)
		{
			disY = false;
			disZ = false;
		}
		if(!disX && !disY && !disZ)
			disX = true;
		disY = GUI.Toggle(new Rect(18,36,12,14),disY,"y",customSkin3.toggle);
		if(disY)
		{
			disX = false;
			disZ = false;
		}
		if(!disX && !disY && !disZ)
			disY = true;
		disZ = GUI.Toggle(new Rect(28,36,12,14),disZ,"z",customSkin3.toggle);
		if(disZ)
		{
			disX = false;
			disY = false;
		}
		if(!disX && !disY && !disZ)
			disZ = true;
		
		disBySize = GUI.Toggle(new Rect(6,50,17,17),disBySize,guiContent[32],customSkin.toggle);
		setSpacing = GUI.Toggle(new Rect(23,50,17,17),setSpacing,guiContent[31],customSkin.toggle);
		if(setSpacing)
		{
			disSpacing = EditorGUI.FloatField(new Rect(6,68,34,14),"", disSpacing);
			reverseDis = GUI.Toggle(new Rect(6,80,12,12),reverseDis, "");
			GUI.Label(new Rect(17,82,84,80),"Last",customSkin.label);
			
			if(disSpacing < 0)
				disSpacing = 0;
		}
		
		//Align Buttons
		if (GUI.Button(new Rect(44,2,34,34),guiContent[29],customSkin.button))
			AlignHigh();
		if (GUI.Button(new Rect(44,36,34,34),guiContent[30],customSkin.button))
			AlignLow();
		
		alignX = GUI.Toggle(new Rect(45,70,12,14),alignX,"x",customSkin3.toggle);
		if(alignX)
		{
			alignY = false;
			alignZ = false;
		}
		if(!alignX && !alignX && !alignZ)
			alignX = true;
		alignY = GUI.Toggle(new Rect(55,70,12,14),alignY,"y",customSkin3.toggle);
		if(alignY)
		{
			alignX = false;
			alignZ = false;
		}
		if(!alignX && !alignY && !alignZ)
			alignY = true;
		alignZ = GUI.Toggle(new Rect(65,70,12,14),alignZ,"z",customSkin3.toggle);
		if(alignZ)
		{
			alignX = false;
			alignY = false;
		}
		if(!alignX && !alignY && !alignZ)
			alignZ = true;
		
		GUI.EndGroup();
		
		objPainter = GUI.Toggle(new Rect(767,2,32,47),objPainter,guiContent[16],customSkin.toggle);
		if(!objPainter)
		{
			if(OPwindowResize)
			{
				if(!importOptionsDropDown && !assignHotKeysDropDown)
				{
					SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
					window.minSize = new Vector2(842, 98);
					window.maxSize = new Vector2(843, 98);
					if(CSwindowResize)
					{
						window.minSize = new Vector2(928, 98);
						window.maxSize = new Vector2(929, 98);
					}
				}
				OPwindowResize = false;
			}
		}
		
		GUI.EndGroup();
			
		//Object Painter=======================================================================================
		if(objPainter)
		{
			if(!OPwindowResize)
			{
				SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
				window.minSize = new Vector2(1094, 98);
				window.maxSize = new Vector2(1095, 98);
				OPwindowResize = true;
				if(CSwindowResize)
				{
					window.minSize = new Vector2(1178, 98);
					window.maxSize = new Vector2(1179, 98);
				}
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			
			//Second Group where all the other buttons reside. This allows the next group to start at 0
			GUI.BeginGroup(new Rect(802,2,250,94));
			//When mouse is over the window update the menus, 
			//this updates the names
			if(EditorWindow.mouseOverWindow)
			{
				for(int i = 0; i < objs.Count; i++)
				{
					if(objs[i] == null)
						objNames[i] = "None";
					else
					{
						objNames[i] = objs[i].name;
					}
				}
			}
			//Object painter group
			GUI.BeginGroup(new Rect(0,0,250,94),customSkin.textArea);
			if(objs.Count > 0)
			{
				curObject = EditorGUI.Popup(new Rect(10,2,136,16),"",curObject,objNames);

				if(GUI.Button(new Rect(0,2,10,16),"X",customSkin.button))
				{
					objs.RemoveAt(curObject);
					RebuildNames();
				}
				if(objNames.Length >= curObject + 1)
				{					
					randRot = GUI.Toggle(new Rect(0,29,16,16),randRot, "");
					GUI.Label(new Rect(10,31,84,80),"Rand Rotation",customSkin.label);
					if(randRot)
					{
						randRotX = GUI.Toggle(new Rect(10,41,24,16),randRotX,"X");
						randRotY = GUI.Toggle(new Rect(34,41,24,16),randRotY,"Y");
						randRotZ = GUI.Toggle(new Rect(58,41,24,16),randRotZ,"Z");
						
						GUI.Label(new Rect(28,58,84,80),"snap",customSkin.label);
						randomRotSnap = EditorGUI.FloatField(new Rect(28,72,30,16),"", randomRotSnap);
					}
					
					randScale = GUI.Toggle(new Rect(90,29,16,16),randScale, "");
					GUI.Label(new Rect(100,31,84,80),"Rand Scale",customSkin.label);
					if(randScale)
					{
						randScaleMin = EditorGUI.FloatField(new Rect(102,74,30,16),"", randScaleMin);
						GUI.Label(new Rect(80,69,84,80),"min",customSkin.label);
						GUI.Label(new Rect(80,77,84,80),"max",customSkin.label);
						randScaleMax = EditorGUI.FloatField(new Rect(134,74,30,16),"", randScaleMax);
						uniformScale = GUI.Toggle(new Rect(116,42,16,16),uniformScale, "");
						GUI.Label(new Rect(84,56,84,16),"Uniform Scale",customSkin.label);
						
						if(randScaleMax < randScaleMin)
							randScaleMax = randScaleMin;
						
						if(randScaleMin > randScaleMax)
							randScaleMin = randScaleMax;
					}					
				}
			}
			else
				GUI.Label(new Rect(0,1,116,80),"Select one or multiple \nGameObjects in \nthe Project Tab, \nthen click \n\"Add Objects\"",customSkin.label);
			
			GUI.EndGroup();
			//}
			
			GUI.BeginGroup(new Rect(160,2,84,78));
			assignParent = GUI.Toggle(new Rect(0,16,16,16),assignParent, "");
			if(assignParent)
			{
				parentObjs[0] = EditorGUI.ObjectField(new Rect(0,32,84,16),"", parentObjs[0], typeof(GameObject), true) as GameObject;
				parentObj = parentObjs[0] as GameObject;
			}
			GUI.Label(new Rect(8,18,84,80),"Assign Parent",customSkin.label);
			
			//Area for drag and dropping of objects
			if(GUI.Button(new Rect(0,0,86,16),"Add Objects"))
			{
				if(Selection.objects.Length > 0)
				{
					for(int i = 0; i < Selection.objects.Length; i++)
					{
						if(PrefabUtility.GetPrefabType(Selection.objects[i]) == PrefabType.Prefab)
						{
							bool isInList = false;
							foreach(GameObject obj in objs)
							{
								if(obj.name == Selection.objects[i].name)
									isInList = true;
							}
							if(!isInList)
							{
								objs.Add(Selection.objects[i]);
							}
							else
								Debug.Log ("Object with same name is already in the Object Painter List");
						}
					}
					RebuildNames();
				}
			}
			eraserMode = false;
			eraserMode = GUI.Toggle(new Rect(0,100,16,16),eraserMode, "");
			GUI.Label(new Rect(10,102,84,16),"Eraser Mode",customSkin.label);
			
			GUI.EndGroup();
			GUI.EndGroup();
		}
		
		//Object Painter End================================================================================
		if(!objPainter)
			optPixelHeight = pixelHeight = 802;
		if(objPainter)
			optPixelHeight = pixelHeight = 1052;
		if(colorSwatches)
			optPixelHeight = 886;
		if(objPainter && colorSwatches)
			optPixelHeight = 1136;
		
		colorSwatches = GUI.Toggle(new Rect(768,54,34,34),colorSwatches,guiContent[22],customSkin.toggle);
		if(!colorSwatches)
		{
			if(CSwindowResize)
			{
				if(!importOptionsDropDown && !assignHotKeysDropDown)
				{
					SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
					window.minSize = new Vector2(842, 98);
					window.maxSize = new Vector2(843, 98);
					if(OPwindowResize)
					{
						window.minSize = new Vector2(1094, 98);
						window.maxSize = new Vector2(1095, 98);
					}
				}
				CSwindowResize = false;
			}
		}
		
		if(colorSwatches)
		{
			if(!CSwindowResize)
			{
				SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
				window.minSize = new Vector2(928, 98);
				window.maxSize = new Vector2(929, 98);
				CSwindowResize = true;
				if(OPwindowResize)
				{
					window.minSize = new Vector2(1178, 98);
					window.maxSize = new Vector2(1179, 98);
				}
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			
			GUI.BeginGroup(new Rect(pixelHeight,2,84,94),customSkin.textArea);
			if(GUI.Button(new Rect(0,0,17,16),"+",customSkin.button))
			{
				colorList.Add(color1);
			}
			if(colorList.Count == 0)
			{
				colorList.Add(color1);
			}
			
			vScrollPos = GUI.VerticalScrollbar(new Rect(68,0,16,93),vScrollPos,1,0,colorList.Count*16 - 93);
			
			for(int i = 0; i < colorList.Count; i++)
			{
				colorList[i] = EditorGUI.ColorField(new Rect(27,i*16-vScrollPos,42,16),"",colorList[i]);
				if(GUI.Button(new Rect(17,i*16-vScrollPos,10,16),"X",customSkin.button))
				{
					colorList.RemoveAt(i);
				}
			}
			GUI.EndGroup();
		}
			
		GUI.BeginGroup(new Rect(optPixelHeight,2,400,94));
		//Drop down for assigning hot keys. contains a button for each hot key that is assignable
		assignHotKeysDropDown = GUI.Toggle(new Rect(2,0,34,26),assignHotKeysDropDown,guiContent[23],customSkin.toggle);		
		if(!assignHotKeysDropDown)
		{
			if(assignHKwindowResize)
			{
				if(!objPainter && !importOptionsDropDown && !colorSwatches)
				{
					SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
					window.minSize = new Vector2(842, 98);
					window.maxSize = new Vector2(843, 98);
				}
				assignHKwindowResize = false;
			}
		}
		if(assignHotKeysDropDown)
		{
			if(!assignHKwindowResize)
			{
				SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
				window.minSize = new Vector2(1050, 98);
				window.maxSize = new Vector2(1051, 98);
				assignHKwindowResize = true;
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(colorSwatches)
				colorSwatches = false;
			if(objPainter)
				objPainter = false;
			
			//Check if any modifiers have changed and save them
			for(int i = 0;i < hkBtnNames.Length;i++)
			{
				if(modControl[i] != EditorPrefs.GetBool("SMmodControl" + i.ToString()))
					EditorPrefs.SetBool("SMmodControl" + i.ToString(),modControl[i]);
				
				if(modShift[i] != EditorPrefs.GetBool("SMmodShift" + i.ToString()))
					EditorPrefs.SetBool("SMmodShift" + i.ToString(),modShift[i]);
				
				if(modAlt[i] != EditorPrefs.GetBool("SMmodAlt" + i.ToString()))
					EditorPrefs.SetBool("SMmodAlt" + i.ToString(),modAlt[i]);
			}
			
			GUI.BeginGroup(new Rect(48,2,250,94));
			assignHKsel = EditorGUI.Popup(new Rect(0,2,150,16),assignHKsel,hkBtnNames);
			
			GUI.Label(new Rect(6,20,84,80),toolKeyStrings[assignHKsel],customSkin3.label);
			
			GUI.Label(new Rect(100,20,84,80),"Modifiers:",customSkin.label);
			
			modControl[assignHKsel] = GUI.Toggle(new Rect(100,30,84,16),modControl[assignHKsel], "Control");
			modShift[assignHKsel] = GUI.Toggle(new Rect(100,44,84,16),modShift[assignHKsel], "Shift");
			modAlt[assignHKsel] = GUI.Toggle(new Rect(100,58,84,16),modAlt[assignHKsel], "Alt");
			
			if(GUI.Button(new Rect(2,42,78,16),"Assign Key",customSkin.button))
			{
				toolAssignKey[assignHKsel] = true;
				assignNextKey = true;
				Debug.Log("Please press the key you would like to assign to " + hkBtnNames[assignHKsel] + " ||  If you would like to remove the hot key assignment Press Escape");
				Repaint();
			}
			if(GUI.Button(new Rect(2,74,78,16),"Print All",customSkin.button))
			{
				string allFormatted = "Highlight to see all Key Assignments...";
				for(int i = 0;i < toolKeyCodes.Length; i++)
				{
					string tempControl;
					string tempShift;
					string tempAlt;
					if(modControl[i])
						tempControl = "Control";
					else
						tempControl = "";
					if(modShift[i])
						tempShift = "Shift";
					else
						tempShift = "";
					if(modAlt[i])
						tempAlt = "Alt";
					else
						tempAlt = "";
					
					allFormatted += "\n" + hkBtnNames[i] + " =  " + "((" + toolKeyStrings[i] + "))" + "  Modifiers(" + tempControl + ", " + tempShift + ", " + tempAlt + ")";
				}
				Debug.Log(allFormatted);
			}
			GUI.EndGroup();
		}
		//Dropdown for extra options
		importOptionsDropDown = GUI.Toggle(new Rect(2,31,34,26),importOptionsDropDown,guiContent[24],customSkin.toggle);
		if(!importOptionsDropDown)
		{
			if(optionsWindowResize)
			{
				if(!objPainter && !assignHotKeysDropDown && !colorSwatches)
				{
					SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
					window.minSize = new Vector2(842, 98);
					window.maxSize = new Vector2(843, 98);
				}
				optionsWindowResize = false;
			}
		}
		
		if(importOptionsDropDown)
		{			
			if(!optionsWindowResize)
			{
				SceneMate window = (SceneMate)EditorWindow.GetWindow(typeof(SceneMate));
				window.minSize = new Vector2(1050, 98);
				window.maxSize = new Vector2(1051, 98);
				optionsWindowResize = true;
			}
			//close other menus to make room for this one
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			if(colorSwatches)
				colorSwatches = false;
			if(objPainter)
				objPainter = false;
			
			GUI.BeginGroup(new Rect(48,2,400,90));
			
			GUI.Label(new Rect(0,0,84,16),"Import Settings",customSkin3.label);
			materialImport = GUI.Toggle(new Rect(0,20,84,16),materialImport, "Materials",customSkin2.toggle);
			animationImport = GUI.Toggle(new Rect(0,36,84,16),animationImport, "Animations",customSkin2.toggle);
			
			GUI.Label(new Rect(100,0,84,16),"Layer Masks",customSkin3.label);
			
			//find all layers and use them for the layer mask popup
			if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) 
			{
	        	lastUpdateTick = System.DateTime.Now.Ticks;
	       		if (layers == null) 
				{
		            layers = new List<string>();
		            layerNumbers = new List<int>();
		            layerNames = new string[4];
	        	}
				else 
				{
		            layers.Clear ();
		            layerNumbers.Clear ();
	        	}
	
		        int emptyLayers = 0;
		        for (int i=0;i<32;i++) 
				{
	           		string layerName = LayerMask.LayerToName (i);
	
		            if (layerName != "") 
					{
		                for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
		                layerNumbers.Add (i);
		                layers.Add (layerName);
		            } 
					else 
					{
	                	emptyLayers++;
	            	}
	        	}
	
	       		if (layerNames.Length != layers.Count) 
				{
	            	layerNames = new string[layers.Count];
	       		}
	        for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
    		}
			int tempSnapsMask = snapsMask.value;
			//Snaps Layer Masking
			GUI.Label(new Rect(100,14,84,16),"Snaps",customSkin.label);
    		snapsMask.value = EditorGUI.MaskField(new Rect(100,26,84,16),"",snapsMask.value,layerNames);
			
			//Object Painter Layer Masking
			GUI.Label(new Rect(100,44,84,16),"Painter",customSkin.label);
			painterMask.value = EditorGUI.MaskField(new Rect(100,56,84,16),"",painterMask.value,layerNames);
			
			//Don't allow the user to check Ignore Raycast, since that is used for snaps
			if(snapsMask.value == tempSnapsMask + 4)
				snapsMask.value = snapsMask.value - 4;
			if(snapsMask.value == tempSnapsMask -4)
				snapsMask.value = snapsMask.value + 4;
			if(snapsMask.value == -1)
				snapsMask.value = snapsMask.value - 4;
			
			GUI.EndGroup();
		}
		
		if(assignNextKey)
		{
			GUI.Label(new Rect(50,60,78,16),"Press Any Key",customSkin.label);
		}
				
		//Assign Hot Keys while GUI is active
		Event current = Event.current;
		
		if (Event.current.type == EventType.KeyDown)
		{
			if(assignNextKey)
			{
				for(int i = 0;i < toolKeyCodes.Length; i++)
				{
					if(toolAssignKey[i] == true)
					{
						if(current.keyCode == KeyCode.Escape || current.keyCode == KeyCode.LeftControl 
							|| current.keyCode == KeyCode.RightControl || current.keyCode == KeyCode.LeftShift 
							|| current.keyCode == KeyCode.RightShift || current.keyCode == KeyCode.LeftAlt 
							|| current.keyCode == KeyCode.RightAlt)
						{
							toolKeyCodes[i] = KeyCode.None;
							toolKeyStrings[i] = KeyCode.None.ToString();
							toolAssignKey[i] = false;
							Debug.Log("Hot key assignment removed.");
						}
						else
						{
							toolKeyCodes[i] = current.keyCode;
							toolKeyStrings[i] = toolKeyCodes[i].ToString();
							toolAssignKey[i] = false;
						}
						assignNextKey = false;
						for(int o = 0; o < toolKeyCodes.Length;o++)
						{
							EditorPrefs.SetString(toolPrefStrings[o], toolKeyStrings[o]);
						}
						Repaint();
					}
				}
			}
		}
		GUI.EndGroup();
	}
	//Rebuilds the names of the buttons in the selection grid for the object painter
	public void RebuildNames()
	{
		objNames = new string[objs.Count];
		for(int i = 0; i < objs.Count; i++)
		{
			if(objs[i] == null)
				objNames[i] = "None";
			else
				objNames[i] = objs[i].name;
		}
	}
	
	public void OnSceneGUI (SceneView scnView)
	{		
		//Assign and use Hot Keys
		Event current = Event.current;
		
		if (Event.current.type == EventType.KeyDown)
		{
			//Assign Hot Keys While viewport is active
			if(assignNextKey)
			{
				for(int i = 0;i < toolKeyCodes.Length; i++)
				{
					if(toolAssignKey[i] == true)
					{
						if(current.keyCode == KeyCode.Escape || current.keyCode == KeyCode.LeftControl 
							|| current.keyCode == KeyCode.RightControl || current.keyCode == KeyCode.LeftShift 
							|| current.keyCode == KeyCode.RightShift || current.keyCode == KeyCode.LeftAlt 
							|| current.keyCode == KeyCode.RightAlt)
						{
							toolKeyCodes[i] = KeyCode.None;
							toolKeyStrings[i] = KeyCode.None.ToString();
							toolAssignKey[i] = false;
							Debug.Log("Hot key assignment removed.");
						}
						else
						{
							toolKeyCodes[i] = current.keyCode;
							toolKeyStrings[i] = toolKeyCodes[i].ToString();
							toolAssignKey[i] = false;
						}
					}
					assignNextKey = false;
					for(int o = 0; o < toolKeyCodes.Length;o++)
					{
						EditorPrefs.SetString(toolPrefStrings[o], toolKeyStrings[o]);
					}
				}
			}
			//Use Hot Keys-----------------------------

			for(int i = 0; i < toolKeyCodes.Length;i++)
			{
				if (current.keyCode == toolKeyCodes[i] && current.control == modControl[i] 
					&& current.shift == modShift[i] &&  current.alt == modAlt[i] 
					&& current.keyCode != KeyCode.None)
				{
					if(methodNames[i] != "")
					{
						System.Type myMate = this.GetType();
						MethodInfo theMethod = myMate.GetMethod(methodNames[i]);
						theMethod.Invoke(this,null);
					}
				}
			}
		}
		
		//Drag Follows Collision
		if (snapDrag)
		{
			if (Selection.transforms.Length > 0)
			{
				RaycastHit hitInfo = new RaycastHit();
				int currentLayer = Selection.transforms[0].gameObject.layer;
				Selection.transforms[0].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[0].position;
				
				if(dragDirNegX)
				{
					if (Physics.Raycast(snapRayStart,Vector3.left, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negX = hitInfo.point.x + Selection.transforms[0].collider.bounds.extents.x;
								Selection.transforms[0].position = new Vector3(negX,hitInfo.point.y,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x + snapOffset, Selection.transforms[0].position.y, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosX)
				{
					if (Physics.Raycast(snapRayStart,Vector3.right, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posX = hitInfo.point.x - Selection.transforms[0].collider.bounds.extents.x;
								Selection.transforms[0].position = new Vector3(posX,hitInfo.point.y,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x + snapOffset, Selection.transforms[0].position.y, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirNegY)
				{
					if (Physics.Raycast(snapRayStart,Vector3.down, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negY = hitInfo.point.y + Selection.transforms[0].collider.bounds.extents.y;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,negY,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y + snapOffset, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosY)
				{
					if (Physics.Raycast(snapRayStart,Vector3.up, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posY = hitInfo.point.y - Selection.transforms[0].collider.bounds.extents.y;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,posY,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y + snapOffset, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirNegZ)
				{
					if (Physics.Raycast(snapRayStart,Vector3.back, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negZ = hitInfo.point.z + Selection.transforms[0].collider.bounds.extents.z;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,hitInfo.point.y,negZ);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y, Selection.transforms[0].position.z + snapOffset);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosZ)
				{
					if (Physics.Raycast(snapRayStart,Vector3.forward, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posZ = hitInfo.point.z + Selection.transforms[0].collider.bounds.extents.z;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,hitInfo.point.y,posZ);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y, Selection.transforms[0].position.z + snapOffset);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				
				Handles.color = Color.blue;
				if(!alignToNormal)
				{
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,new Vector3(0,1,0),.4f);
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,new Vector3(0,1,0),.46f);
					HandleUtility.Repaint();
				}
				if(alignToNormal)
				{
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,Selection.transforms[0].up,.4f);
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,Selection.transforms[0].up,.46f);
					HandleUtility.Repaint();
				}
			}
		}
		
		if(snapSurface)
		{
			Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
			RaycastHit hitInfo;
			RaycastHit hitInfoMesh;
			
			Handles.color = Color.blue;
			if(!alignToNormal)
			{
				Handles.DrawWireDisc(handlePos,new Vector3(0,1,0),.4f);
				Handles.DrawWireDisc(handlePos,new Vector3(0,1,0),.46f);
				HandleUtility.Repaint();
			}
			if(alignToNormal)
			{
				Handles.DrawWireDisc(handlePos,handleNormal,.4f);
				Handles.DrawWireDisc(handlePos,handleNormal,.46f);
				HandleUtility.Repaint();
			}
			
			if (Selection.transforms.Length > 0)
			{
				bool hasMeshCollider;
				bool meshColEnabled = true;
				if (current.keyCode == toolKeyCodes[11] && current.control == modControl[11] 
				&& current.shift == modShift[11] &&  current.alt == modAlt[11] 
				&& current.keyCode != KeyCode.None)
				{	
					hasMeshCollider = false;
					Undo.RegisterUndo(Selection.transforms[0], "Snap To Surface");
					int currentLayer = Selection.transforms[0].gameObject.layer;
					Selection.transforms[0].gameObject.layer = 2;
						
					if (Physics.Raycast(worldRay, out hitInfo,Mathf.Infinity,snapsMask))
					{
						MeshCollider meshCollider = hitInfo.transform.gameObject.GetComponent<MeshCollider>();
						if(meshCollider != null)
						{
							hasMeshCollider = true;
							if (meshCollider.enabled == false)
								meshColEnabled = false;
						}
						else
						{
							meshCollider = hitInfo.transform.gameObject.AddComponent<MeshCollider>();
						}
						hitInfo.collider.enabled = false;
						meshCollider.enabled = true;
						
						if (Physics.Raycast(worldRay, out hitInfoMesh))
						{
							Selection.transforms[0].position = hitInfoMesh.point;
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfoMesh.normal);
							}
							handlePos = hitInfoMesh.point;
							handleNormal = hitInfoMesh.normal;
						}
						if(!hasMeshCollider)
						{
							DestroyImmediate(meshCollider);
						}
						hitInfo.collider.enabled = true;
						
						if(!meshColEnabled)
							meshCollider.enabled = false;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;
				}
			}
		}
		
		//Object Painter===============================================================================
		if(objPainter)
		{
			//Draw the painter brush outline aka disc
			Ray worldRayDisc = HandleUtility.GUIPointToWorldRay(current.mousePosition);
			RaycastHit hitInfoDisc;
			if (Physics.Raycast(worldRayDisc, out hitInfoDisc))
			{
				if(eraserMode)
				{
					Handles.color = Color.red;
				}
				else
					Handles.color = Color.green;
				if(alignToNormal)
				{
					if(hitInfoDisc.distance > 1)
					{
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/25);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/50);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/200);
					}
					else
					{
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.04f);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.02f);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.005f);
					}
				}
				else
				{
					if(hitInfoDisc.distance > 1)
					{
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/25);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/50);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/200);
					}
					else
					{
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.04f);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.02f);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.005f);
					}
				}
				
				HandleUtility.Repaint();
			}
			//Make the last painted object the current selection.
			if(newObject != null)
			{
				if(Selection.activeTransform != newObject.transform)
				{
					if(selectLast)
					{
						Selection.activeTransform = newObject.transform;
						selectLast = false;
					}
				}
			}
			if(current.modifiers != EventModifiers.Alt)
			{
				if (current.type == EventType.MouseDown && current.button == 0)
				{
					if(current.type == EventType.MouseDown && current.button == 0)
						wasMouseDown = true;
					
					if(!painterKeyDown)
					{
						//HandleUtility.AddDefaultControl(controlID);
						painterKeyDown = true;
						Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
						RaycastHit hitInfo;
								
						if (Physics.Raycast(worldRay, out hitInfo,Mathf.Infinity,painterMask))
						{
							if(eraserMode)
							{
								//Undo.RegisterUndo(hitInfo.transform.gameObject, "Eraser");
								DestroyImmediate(hitInfo.transform.gameObject);
							}
							else
							{
								if(objs.Count > 0)
								{
								if (objs[curObject] != null)
								{
									newObject = (GameObject)PrefabUtility.InstantiatePrefab (objs[curObject]);
									newObject.transform.position = hitInfo.point;
									Selection.activeTransform = newObject.transform;
									if(assignParent && parentObj != null)
										newObject.transform.parent = parentObj.transform;
									if(alignToNormal)
									{
										newObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
									}
									if(randRot)
									{
										if(randRotX)
										{
											float tempRot = Random.Range(0,360);
											if(randomRotSnap != 0)
												tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
											newObject.transform.Rotate(tempRot,0,0,Space.Self);
										}
										
										if(randRotY)
										{
											float tempRot = Random.Range(0,360);
											if(randomRotSnap != 0)
												tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
											newObject.transform.Rotate(0,tempRot,0,Space.Self);
										}
										
										if(randRotZ)
										{
											float tempRot = Random.Range(0,360);
											if(randomRotSnap != 0)
												tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
											newObject.transform.Rotate(0,0,tempRot,Space.Self);
										}
									}
									
									if(randScale)
									{
										if(uniformScale)
										{
											float scale = Random.Range(randScaleMin,randScaleMax);
											newObject.transform.localScale = new Vector3(scale,scale,scale);
										}
										else
										{
											newObject.transform.localScale = new Vector3(Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax));
										}
									}
								}
								}
							}
						}
						current.Use();
					}
				}
				if(current.type == EventType.KeyUp)
				{
					painterKeyDown = false;
				}
				
				else if(wasMouseDown)
				{
					painterKeyDown = false;
					wasMouseDown = false;
					selectLast = true;
				}
			}
		}
		//Object Painter End===============================================================================
		
		//Match Tools
		if(waitForSelMatch)
		{
			Undo.RegisterUndo(Selection.transforms, "Match");
			if(!grabSource)
			{
				grabSource = true;
				sourceObjs = new GameObject[Selection.gameObjects.Length];
				for(int i = 0; i < sourceObjs.Length;i++)
				{
					sourceObjs[i] = Selection.gameObjects[i];
				}
			}
			else
			{
				Ray worldRayDisc = HandleUtility.GUIPointToWorldRay(current.mousePosition);
				RaycastHit hitInfoDisc;
				if (Physics.Raycast(worldRayDisc, out hitInfoDisc))
				{
					Handles.color = Color.yellow;
					if(matchPos)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawSolidDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/130);
						}
						else
						{
							Handles.DrawSolidDisc(hitInfoDisc.point,hitInfoDisc.normal,.0077f);
						}
					}
					if(matchRot)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/60);
						}
						else
						{
							
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.0167f);
						}
					}
					if(matchScale)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/30);
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/26);
						}
						else
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.033f);
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.038f);
						}
					}
					HandleUtility.Repaint();
				}
				
				if(Selection.activeGameObject != null)
				{
					if(Selection.gameObjects[0] != sourceObjs[0])
					{
						for(int i = 0; i < sourceObjs.Length;i++)
						{
							if(matchPos)
							{
								Vector3 tempPos = sourceObjs[i].transform.position;
								if(posMatchX)
									tempPos.x = Selection.gameObjects[0].transform.position.x;
								if(posMatchY)
									tempPos.y = Selection.gameObjects[0].transform.position.y;
								if(posMatchZ)
									tempPos.z = Selection.gameObjects[0].transform.position.z;
								sourceObjs[i].transform.position = tempPos;
							}
							if(matchRot)
							{
								Vector3 tempRot = sourceObjs[i].transform.localEulerAngles;
								if(rotMatchX)
									tempRot.x = Selection.gameObjects[0].transform.localEulerAngles.x;
								if(rotMatchY)
									tempRot.y = Selection.gameObjects[0].transform.localEulerAngles.y;
								if(rotMatchZ)
									tempRot.z = Selection.gameObjects[0].transform.localEulerAngles.z;
								sourceObjs[i].transform.localEulerAngles = tempRot;
							}
							if(matchScale)
							{
								Vector3 tempScale = sourceObjs[i].transform.localScale;
								if(scaleMatchX)
									tempScale.x = Selection.gameObjects[0].transform.localScale.x;
								if(scaleMatchY)
									tempScale.y = Selection.gameObjects[0].transform.localScale.y;
								if(scaleMatchZ)
									tempScale.z = Selection.gameObjects[0].transform.localScale.z;
								sourceObjs[i].transform.localScale = tempScale;
							}
						}
						matchPos = false;
						matchRot = false;
						matchScale = false;
						grabSource = false;
						waitForSelMatch = false;
					}
				}
				else
				{
					if(matchPos)
						matchPos = false;
					if(matchRot)
						matchRot = false;
					if(matchScale)
						matchScale = false;
				}
				Selection.objects = sourceObjs;
			}	
		}
	}
	
	public void MatchPosToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchPos = true;
			waitForSelMatch = true;
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void MatchRotToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchRot = true;
			waitForSelMatch = true;	
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void MatchScaleToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchScale = true;
			waitForSelMatch = true;	
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void ObjPainterToggle()
	{
		if(!objPainter)
			objPainter = true;
		else
			objPainter = false;
		Repaint ();
	}
	
	public void ColorSwatchesToggle()
	{
		if(!colorSwatches)
			colorSwatches = true;
		else
			colorSwatches = false;
		Repaint ();
	}
	
	public void SnapDragToggle()
	{
		if(!snapDrag)
			snapDrag = true;
		else
			snapDrag = false;
		Repaint ();
	}
	
	public void AlignToNormalToggle()
	{
		if(!alignToNormal)
			alignToNormal = true;
		else
			alignToNormal = false;
		Repaint ();
	}
	
	public void SnapRotation90Toggle()
	{
		if(!snapRotation90)
			snapRotation90 = true;
		else
			snapRotation90 = false;
		Repaint ();
	}
	public void SetSpacingToggle()
	{
		if(!setSpacing)
			setSpacing = true;
		else
			setSpacing = false;
		Repaint ();
	}
	public void BySizeToggle()
	{
		if(!disBySize)
			disBySize = true;
		else
			disBySize = false;
		Repaint ();
	}
	public void BoundsSnapToggle()
	{
		if(!boundsSnapping)
			boundsSnapping = true;
		else
			boundsSnapping = false;
		Repaint ();
	}
	
	public void RotateLeft()
	{
		if (Selection.transforms.Length > 0)
		{
			if(rotateCurAxis == 1)
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (rotationAmount,0,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (0,rotationAmount,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (0,0,rotationAmount);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
			else
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(1,0,0),rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(0,1,0),rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(0,0,1),rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}			
		}
	}
	
	public void RotateRight()
	{
		if (Selection.transforms.Length > 0)
		{
			if(rotateCurAxis == 1)
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (-rotationAmount,0,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (0,-rotationAmount,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].Rotate (0,0,-rotationAmount);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
			else
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(1,0,0),-rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(0,1,0),-rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
						Selection.transforms[r].RotateAround(new Vector3(0,0,1),-rotationAmount/57.296f);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
		}
	}
	
	public void BoundsGblObjSnap(Vector3 snapDir, Vector3 hitPoint, Vector3 boundsExtents)
	{
		if (snapDir == snapDirVectors[0])
		{
			float negX = hitPoint.x + boundsExtents.x;
			boundsObjPos = new Vector3(negX,hitPoint.y,hitPoint.z);
		}
		if (snapDir == snapDirVectors[1])
		{
			float posX = hitPoint.x - boundsExtents.x;
			boundsObjPos = new Vector3(posX,hitPoint.y,hitPoint.z);
		}
		if (snapDir == snapDirVectors[2])
		{
			float negY = hitPoint.y + boundsExtents.y;
			boundsObjPos = new Vector3(hitPoint.x,negY,hitPoint.z);
		}
		if (snapDir == snapDirVectors[3])
		{
			float posY = hitPoint.y - boundsExtents.y;
			boundsObjPos = new Vector3(hitPoint.x,posY,hitPoint.z);
		}
		if (snapDir == snapDirVectors[4])
		{
			float negZ = hitPoint.z + boundsExtents.z;
			boundsObjPos = new Vector3(hitPoint.x,hitPoint.y,negZ);
		}
		if (snapDir == snapDirVectors[5])
		{
			float posZ = hitPoint.z - boundsExtents.z;
			boundsObjPos = new Vector3(hitPoint.x,hitPoint.y,posZ);
		}
	}
	
	public void SnapToGround()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Ground");
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;
				//snapRayStart = snapRayStart + new Vector3(0,1,0);
	
				if (Physics.Raycast(snapRayStart,snapDirVectors[stgDir], out hitInfo, Mathf.Infinity,snapsMask))
				{
					if (hitInfo.point != snapRayStart)
					{
						if(alignToNormal)
						{
							Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
						}
						if(boundsSnapping)
						{
							BoundsGblObjSnap(snapDirVectors[stgDir],hitInfo.point,Selection.transforms[i].collider.bounds.extents);
							Selection.transforms[i].position = boundsObjPos;
						}
						else
						{
							Selection.transforms[i].position = hitInfo.point;
						}
						//snapOffset
						if(stgDir == 0 || stgDir == 1)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
						if(stgDir == 2 || stgDir == 3)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
						if(stgDir == 4 || stgDir == 5)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;				
			}
		}
	}
	
	public void SnapToWall()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Wall");
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;// + new Vector3(0,.05f,0);
				
				{
					if(stwDir == 0)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].right, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(tempMesh.bounds.extents.x,0,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 1)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].right, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(-tempMesh.bounds.extents.x,0,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 2)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].up, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,tempMesh.bounds.extents.y,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 3)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].up, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,-tempMesh.bounds.extents.y,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 4)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].forward, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,0,tempMesh.bounds.extents.z);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
							}
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
						}
					}
					if(stwDir == 5)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].forward, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,0,-tempMesh.bounds.extents.z);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
							}
						}
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;
			}
		}
	}
	
	public void SnapToCenter()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Center");
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;
				//snapRayStart = snapRayStart + new Vector3(0,1,0);
	
				if (Physics.Raycast(snapRayStart,snapDirVectors[stcDir], out hitInfo, Mathf.Infinity,snapsMask))
				{
					if (hitInfo.point != snapRayStart)
					{
						if(alignToNormal)
						{
							Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
						}
						if(boundsSnapping)
						{
							BoundsGblObjSnap(snapDirVectors[stcDir],hitInfo.collider.bounds.center,Selection.transforms[i].collider.bounds.extents);
							Selection.transforms[i].position = boundsObjPos;
						}
						else
						{
							Selection.transforms[i].position = hitInfo.collider.bounds.center;
						}
						//snapOffset
						if(stcDir == 0 || stcDir == 1)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
						if(stcDir == 2 || stcDir == 3)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
						if(stcDir == 4 || stcDir == 5)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;		
			}
		}
	}
	
	public void ResetObjs()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
				Selection.transforms[i].localScale = new Vector3(1,1,1);
				Selection.transforms[i].position = new Vector3(0,0,0);
				Selection.transforms[i].localEulerAngles = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsPos()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
				Selection.transforms[i].position = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsRot()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
				Selection.transforms[i].localEulerAngles = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsScale()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
				Selection.transforms[i].localScale = new Vector3(1,1,1);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void OffsetSel()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Offset Selected");
				Vector3 tempPos = Selection.transforms[i].position;
				if(offsetSelCurAxis == 0)
				{
					if(curOffsetStyle == 0)
					{
						tempPos.x += offsetXAmount;
						tempPos.y += offsetYAmount;
						tempPos.z += offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
					if(curOffsetStyle == 1)
					{
						tempPos.x = tempPos.x * offsetXAmount;
						tempPos.y = tempPos.y * offsetYAmount;
						tempPos.z = tempPos.z * offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
					if(curOffsetStyle == 2)
					{
						if(tempPos.x != 0 && offsetXAmount != 0)
							tempPos.x = tempPos.x / offsetXAmount;
						if(tempPos.y != 0 && offsetYAmount != 0)
							tempPos.y = tempPos.y / offsetYAmount;
						if(tempPos.z != 0 && offsetZAmount != 0)
							tempPos.z = tempPos.z / offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
				}
				else
				{
					if(curOffsetStyle == 0)
					{
						Selection.transforms[i].Translate(offsetXAmount,offsetYAmount,offsetZAmount);
					}
					if(curOffsetStyle == 1)
					{
						Selection.transforms[i].Translate(tempPos.x * offsetXAmount,tempPos.y * offsetYAmount,tempPos.z * offsetZAmount);
					}
					if(curOffsetStyle == 2)
					{
						if(tempPos.x != 0 && offsetXAmount != 0)
							Selection.transforms[i].Translate(tempPos.x / offsetXAmount,0,0);
						if(tempPos.y != 0 && offsetYAmount != 0)
							Selection.transforms[i].Translate(0,tempPos.y / offsetYAmount,0);
						if(tempPos.z != 0 && offsetZAmount != 0)
							Selection.transforms[i].Translate(0,0,tempPos.z / offsetZAmount);
					}
				}
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void SelAmt()
	{
		if (Selection.transforms.Length > 0)
		{
			componentNames.Clear();
			componentNumber.Clear();
			tagNames.Clear();
			tagNumber.Clear();
			layerNamesList.Clear();
			layerNumberList.Clear();
			vertCount = 0;
			//Prints out a "Debug" to the console of all the currently selected objects
			Debug.Log ("Total Selected: " + Selection.objects.Length);
			Component[] monoArr = new Component[50];
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				monoArr = new Component[50];
				monoArr = Selection.transforms[i].gameObject.GetComponents(typeof(Component));
				
				for(int o = 0; o < monoArr.Length;o++)
				{
					string tempName = monoArr[o].ToString();
					tempName = tempName.Substring(tempName.LastIndexOf(".") + 1,tempName.LastIndexOf(")") - tempName.LastIndexOf(".") - 1);
					if(componentNames.Contains(tempName) == false)
					{
						componentNames.Add(tempName);
						componentNumber.Add(0);
						componentNumber[componentNames.IndexOf(tempName)]++;
					}
					else
					{
						componentNumber[componentNames.IndexOf(tempName)]++;
					}
				}
			}
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				string tempName = Selection.transforms[i].tag.ToString();
				if(tagNames.Contains(tempName) == false)
				{
					tagNames.Add(tempName);
					tagNumber.Add(0);
					tagNumber[tagNames.IndexOf(tempName)]++;
				}
				else
				{
					tagNumber[tagNames.IndexOf(tempName)]++;
				}
				string tempLayerName = LayerMask.LayerToName(Selection.transforms[i].gameObject.layer);
				if(layerNamesList.Contains(tempLayerName) == false)
				{
					layerNamesList.Add(tempLayerName);
					layerNumberList.Add(0);
					layerNumberList[layerNamesList.IndexOf(tempLayerName)]++;
				}
				else
				{
					layerNumberList[layerNamesList.IndexOf(tempLayerName)]++;
				}
			}
			
			//Vert Count
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				MeshFilter tempMesh = Selection.transforms[i].GetComponent(typeof(MeshFilter)) as MeshFilter;
				if(tempMesh != null)
				{
					vertCount += tempMesh.sharedMesh.vertexCount;
				}
			}
			
			for(int i = 0; i < componentNames.Count;i++)
			{
				Debug.Log(componentNames[i] + ": " + componentNumber[i]); 
			}
			for(int i = 0; i < tagNames.Count;i++)
			{
				Debug.Log("Tag (" + tagNames[i] + ": " + tagNumber[i] + ")"); 
			}
			for(int i = 0; i < layerNamesList.Count;i++)
			{
				Debug.Log("Layer (" + layerNamesList[i] + ": " + layerNumberList[i] + ")"); 
			}
			Debug.Log("Vertex Count: " + vertCount);
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipX()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Flip X");
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x * -1,Selection.transforms[i].localScale.y, Selection.transforms[i].localScale.z);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipY()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Flip Y");
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x,Selection.transforms[i].localScale.y * -1, Selection.transforms[i].localScale.z);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipZ()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Flip Z");
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x,Selection.transforms[i].localScale.y, Selection.transforms[i].localScale.z * -1);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FitCollider()
	{
		if(colSelectedObj != null)
		{
			if(colSelectedObj.GetComponent(typeof(MeshFilter)) as MeshFilter == null && colSelectedObj.GetComponent(typeof(MeshRenderer)) as MeshRenderer == null)
			{
				colSelectedObjArr[0] = null;
				colSelectedObj = null;
				Debug.Log ("Object needs to have a Mesh Filter or Mesh Renderer Component");
			}
		}
		Undo.RegisterUndo(Selection.activeTransform, "Fit Collider");
		if(Selection.activeTransform == null)
		{
			Debug.Log ("Please Select an object with a collider component!");
		}
		if(colSelectedObj == null)
		{
			Debug.Log ("Please put a Game Object in the Fit To Mesh object field!");
		}
		else if(colSelectedObj != null && Selection.activeTransform != null)
		{
			if(Selection.activeTransform.GetComponent(typeof(BoxCollider)) as BoxCollider != null)
			{
				BoxCollider newBoxCol = colSelectedObj.AddComponent<BoxCollider>();
				BoxCollider curBoxCol = Selection.activeTransform.GetComponent(typeof(BoxCollider)) as BoxCollider;
				
				curBoxCol.size = newBoxCol.size;
				curBoxCol.center = newBoxCol.center;
				DestroyImmediate(newBoxCol);
			}
			if(Selection.activeTransform.GetComponent(typeof(SphereCollider)) as SphereCollider != null)
			{
				SphereCollider newSphereCol = colSelectedObj.AddComponent<SphereCollider>();
				SphereCollider curSphereCol = Selection.activeTransform.GetComponent(typeof(SphereCollider)) as SphereCollider;
				
				curSphereCol.radius = newSphereCol.radius;
				curSphereCol.center = newSphereCol.center;
				DestroyImmediate(newSphereCol);
			}
			if(Selection.activeTransform.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider != null)
			{
				CapsuleCollider newCapCol = colSelectedObj.AddComponent<CapsuleCollider>();
				CapsuleCollider curCapCol = Selection.activeTransform.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
				
				curCapCol.radius = newCapCol.radius;
				curCapCol.center = newCapCol.center;
				curCapCol.height = newCapCol.height;
				curCapCol.direction = newCapCol.direction;
				DestroyImmediate(newCapCol);
			}
			else
			{
				Debug.Log("Selected Object does not contain a Box, Sphere or Capsule Collider!");
			}
		}
	}
	
	public void CycleRotationAxis()
	{
		rotAxis++;
		if(rotAxis == 3)
			rotAxis = 0;
		Repaint();
	}
	
	public void ReplaceSelection()
	{
		Undo.RegisterUndo(Selection.activeTransform, "Replace Selection");
		if(colSelectedObj != null)
		{
			GameObject[] selectedObjs = new GameObject[Selection.transforms.Length];
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				Transform tempParent = Selection.transforms[i].parent;
				GameObject newGO = PrefabUtility.InstantiatePrefab(colSelectedObj) as GameObject;
				Selection.transforms[i].parent = null;
				if(inheritPos)
					newGO.transform.position = Selection.transforms[i].position;
				if(inheritRot)
					newGO.transform.localRotation = Selection.transforms[i].localRotation;
				if(inheritScale)
					newGO.transform.localScale = Selection.transforms[i].localScale;
				newGO.transform.parent = tempParent;
				selectedObjs[i] = Selection.transforms[i].gameObject;
			}
			if(!keepOrig)
			{
				for(int i = 0; i < selectedObjs.Length;i++)
				{
					DestroyImmediate(selectedObjs[i]);
				}
			}
		}
		else
			Debug.Log ("Please put a Game Object in the object field");
	}
	
	public void RandomizeRotation()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempRot;
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Rotation");

				tempRot = Selection.transforms[i].eulerAngles;
			
				if(rotX)
				{
					float randRotX = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotX = (Mathf.Round(randRotX / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.x = randRotX;
					else
						Selection.transforms[i].Rotate(randRotX,0,0);
				}
				if(rotY)
				{
					float randRotY = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotY = (Mathf.Round(randRotY / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.y = randRotY;
					else
						Selection.transforms[i].Rotate(0,randRotY,0);
				}
				if(rotZ)
				{
					float randRotZ = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotZ = (Mathf.Round(randRotZ / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.z = randRotZ;
					else
						Selection.transforms[i].Rotate(0,0,randRotZ);
				}
				if(randRotCurAxis == 0)
					Selection.transforms[i].localEulerAngles = tempRot;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void RandomizeScale()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempScale;
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Scale");
				
				tempScale = Selection.transforms[i].localScale;
				
				if(randScaleUniform)
				{
					float randScale = Random.Range(randomizeScaleMin,randomizeScaleMax);
					if(scaleX)
						tempScale.x = randScale;
					if(scaleY)
						tempScale.y = randScale;
					if(scaleZ)
						tempScale.z = randScale;
				}
				else
				{
					if(scaleX)
					{
						float randScaleX = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.x = randScaleX;
					}
					if(scaleY)
					{
						float randScaleY = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.y = randScaleY;
					}
					if(scaleZ)
					{
						float randScaleZ = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.z = randScaleZ;
					}
				}
				Selection.transforms[i].localScale = tempScale;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void RandomizePosition()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos;
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Position");

				tempPos = Selection.transforms[i].position;
			
				if(positionX)
				{
					float randPosX = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.x = randPosX;
					else
						Selection.transforms[i].Translate(randPosX,0,0);
				}
				if(positionY)
				{
					float randPosY = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.y = randPosY;
					else
						Selection.transforms[i].Translate(0,randPosY,0);
				}
				if(positionZ)
				{
					float randPosZ = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.z = randPosZ;
					else
						Selection.transforms[i].Translate(0,0,randPosZ);
				}
				if(randPosCurAxis == 0)
					Selection.transforms[i].position = tempPos;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void Distribute()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> disObjList = new List<GameObject>();
			List<float> disPosList = new List<float>();
			GameObject[] disObjListArr = null;
			float[] disPosListArr;
			float range = 0;
			float fixedSpacing = 0;
			float totalSize = 0;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Distribute");
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(disX)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.x);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(tempPos.x + (i * disSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(tempPos.x - (((disObjListArr.Length - 1) - i) * disSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(tempPos.x + disSpacing + disObjListArr[i].collider.bounds.extents.x + disObjListArr[i - 1].collider.bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(tempPos.x - disSpacing - disObjListArr[i].collider.bounds.extents.x - disObjListArr[i + 1].collider.bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.x - disObjListArr[0].transform.position.x;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(tempPos.x + (i * fixedSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].collider.bounds.extents.x * 2;
							}
							else
								totalSize += disObjListArr[i].collider.bounds.extents.x;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.x - disObjListArr[0].transform.position.x) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(tempPos.x + fixedSpacing + disObjListArr[i].collider.bounds.extents.x + disObjListArr[i - 1].collider.bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			if(disY)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.y);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + (i * disSpacing), disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y -(((disObjListArr.Length - 1) - i) * disSpacing), disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + disSpacing + disObjListArr[i].collider.bounds.extents.y + disObjListArr[i - 1].collider.bounds.extents.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y - disSpacing - disObjListArr[i].collider.bounds.extents.y - disObjListArr[i+1].collider.bounds.extents.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.y - disObjListArr[0].transform.position.y;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + (i * fixedSpacing), disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].collider.bounds.extents.y * 2;
							}
							else
								totalSize += disObjListArr[i].collider.bounds.extents.y;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.y - disObjListArr[0].transform.position.y) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + fixedSpacing + disObjListArr[i].collider.bounds.extents.y + disObjListArr[i - 1].collider.bounds.extents.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			if(disZ)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.z);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + (i * disSpacing));
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z - (((disObjListArr.Length - 1) - i) * disSpacing));
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + disSpacing + disObjListArr[i].collider.bounds.extents.z + disObjListArr[i - 1].collider.bounds.extents.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z - disSpacing - disObjListArr[i].collider.bounds.extents.z - disObjListArr[i+1].collider.bounds.extents.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.z - disObjListArr[0].transform.position.z;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + (i * fixedSpacing));
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].collider.bounds.extents.z * 2;
							}
							else
								totalSize += disObjListArr[i].collider.bounds.extents.z;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.z - disObjListArr[0].transform.position.z) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + fixedSpacing + disObjListArr[i].collider.bounds.extents.z + disObjListArr[i - 1].collider.bounds.extents.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
	
	public void AvgMatchPos()
	{		
		matchPos = false;
		waitForSelMatch = false;
		
		if(posMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
				total += Selection.transforms[i].position.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.x = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
		if(posMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
				total += Selection.transforms[i].position.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.y = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
		if(posMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
				total += Selection.transforms[i].position.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.z = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
	}
	
	public void AvgMatchRot()
	{
		matchRot = false;
		waitForSelMatch = false;
		
		if(rotMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
				total += Selection.transforms[i].eulerAngles.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.x = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
		if(rotMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
				total += Selection.transforms[i].eulerAngles.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.y = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
		if(rotMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
				total += Selection.transforms[i].eulerAngles.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.z = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
	}
	
	public void AvgMatchScale()
	{
		matchScale = false;
		waitForSelMatch = false;
		
		if(scaleMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
				total += Selection.transforms[i].localScale.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.x = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
		if(scaleMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
				total += Selection.transforms[i].localScale.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.y = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
		if(scaleMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
				total += Selection.transforms[i].localScale.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.z = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
	}
	
	public void AlignHigh()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> alignObjList = new List<GameObject>();
			List<float> alignPosList = new List<float>();
			GameObject[] alignObjListArr = null;
			float[] alignPosListArr;
			float offsetAmount;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Align");
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(alignX)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.x + Selection.transforms[i].collider.bounds.extents.x);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].collider.bounds.extents.x - alignObjListArr[i].collider.bounds.extents.x;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[alignObjListArr.Length-1].transform.position.x + offsetAmount, alignObjListArr[i].transform.position.y, alignObjListArr[i].transform.position.z);
				}			
			}
			if(alignY)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.y + Selection.transforms[i].collider.bounds.extents.y);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].collider.bounds.extents.y - alignObjListArr[i].collider.bounds.extents.y;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x ,alignObjListArr[alignObjListArr.Length-1].transform.position.y + offsetAmount, alignObjListArr[i].transform.position.z);
				}	
			}
			if(alignZ)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.z + Selection.transforms[i].collider.bounds.extents.z);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].collider.bounds.extents.z - alignObjListArr[i].collider.bounds.extents.z;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x , alignObjListArr[i].transform.position.y, alignObjListArr[alignObjListArr.Length-1].transform.position.z + offsetAmount);
				}	
			}
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
	
	public void AlignLow()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> alignObjList = new List<GameObject>();
			List<float> alignPosList = new List<float>();
			GameObject[] alignObjListArr = null;
			float[] alignPosListArr;
			float offsetAmount;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Undo.RegisterUndo(Selection.transforms[i], "Align");
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(alignX)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.x - Selection.transforms[i].collider.bounds.extents.x);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].collider.bounds.extents.x - alignObjListArr[i].collider.bounds.extents.x;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[0].transform.position.x - offsetAmount, alignObjListArr[i].transform.position.y, alignObjListArr[i].transform.position.z);
				}			
			}
			if(alignY)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.y - Selection.transforms[i].collider.bounds.extents.y);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].collider.bounds.extents.y - alignObjListArr[i].collider.bounds.extents.y;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x ,alignObjListArr[0].transform.position.y - offsetAmount, alignObjListArr[i].transform.position.z);
				}	
			}
			if(alignZ)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.z - Selection.transforms[i].collider.bounds.extents.z);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].collider.bounds.extents.z - alignObjListArr[i].collider.bounds.extents.z;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x , alignObjListArr[i].transform.position.y, alignObjListArr[0].transform.position.z - offsetAmount);
				}	
			}
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
	
	public void OnDestroy()
	{
		EditorPrefs.SetBool("materialImport", materialImport);
		EditorPrefs.SetBool("animationImport", animationImport);
		EditorPrefs.SetBool("importOptionsDropDown",importOptionsDropDown);
		EditorPrefs.SetBool("objPainter",objPainter);
		EditorPrefs.SetBool("assignHotKeysDropDown",assignHotKeysDropDown);
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			EditorPrefs.SetString(toolPrefStrings[i], toolKeyStrings[i]);
		}
			
		snapDrag = false;
		snapRotation90 = false;
		
		EditorPrefs.SetInt("stgDir",stgDir);
		EditorPrefs.SetInt("stwDir",stwDir);
		EditorPrefs.SetInt("stcDir",stcDir);
		EditorPrefs.SetInt("rotAxis",rotAxis);
		EditorPrefs.SetInt("rowWidth",rowWidth);
		EditorPrefs.SetInt("snapsMask",snapsMask);
		EditorPrefs.SetInt("painterMask",painterMask);
	}
	public class SceneMateImport : AssetPostprocessor 
	{   
		public void OnPreprocessTexture()
		{
			for(int i = 0; i < buttonFileNames.Length; i++)
			{
				//load all the icons in the GUI content
				TextureImporter curIcon = AssetImporter.GetAtPath("Assets/SceneMate/Icons/" + buttonFileNames[i].Substring(0,buttonFileNames[i].Length - 3) + ".psd") as TextureImporter;
				curIcon.wrapMode = TextureWrapMode.Clamp;
				curIcon.filterMode = FilterMode.Point;
				curIcon.textureType = TextureImporterType.Advanced;
				curIcon.textureFormat = TextureImporterFormat.RGBA32;
				curIcon.npotScale = TextureImporterNPOTScale.None;
				curIcon.mipmapEnabled = false;
				curIcon.maxTextureSize = 64;
			}
		}
			
		public void OnPreprocessModel() 
		{			
			if (!materialImport)
			{
				ModelImporter modelImporter = (ModelImporter) assetImporter;
				modelImporter.importMaterials = false;
			}
			if (!animationImport)
			{
				ModelImporter modelImporter = (ModelImporter) assetImporter;
				modelImporter.generateAnimations = ModelImporterGenerateAnimations.None;
			}
	    }
	}
}
