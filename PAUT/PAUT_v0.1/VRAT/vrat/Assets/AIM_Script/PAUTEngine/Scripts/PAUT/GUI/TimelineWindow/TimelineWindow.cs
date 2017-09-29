using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using PAUT;

#if UNITY_EDITOR
public class TimelineWindow : EditorWindow
{

	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/My Window")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		TimelineWindow window = (TimelineWindow)EditorWindow.GetWindow(typeof(TimelineWindow));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle("Toggle", myBool);
		myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup();
	}
	/*

    Scenario myScenario; // 처음에 기본으로 하나만 생성하게 하기. 저장과 불러오기를 구현하려면 이것 말고 다른 방식을 사용해야 함.

    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public GameObject foundObjectsWtTag;
    // The position of the window
    public Rect windowRect = new Rect(10, 50, 100, 100);

    Vector2 timelineFrameStartPos = new Vector2(10,0);
    Vector2 timelineWindowStartPos = new Vector2(100, 100);

    [MenuItem("Window/Timeline Window")]
    void Init()
    {
        EditorWindow.GetWindow(typeof(TimelineWindow));
    }

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TimelineWindow));
    }

    void OnGUI()
    {
		/*
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 수정해야 할 부분
        myScenario = GameObject.Find("Scenario").GetComponent<Scenario>();
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Scenario Settings", EditorStyles.boldLabel);
        GUILayout.Button("Load");
        GUILayout.Button("Save");
        GUILayout.Button("Clear");
        EditorGUILayout.EndHorizontal();
        myScenario.scenarioName = EditorGUILayout.TextField("Scenario Name", myScenario.scenarioName);

        timelineFrameStartPos =
            EditorGUILayout.BeginScrollView(timelineFrameStartPos, GUILayout.Width(position.width- timelineFrameStartPos.x), GUILayout.Height(position.height-70));
        for (int i=0; i<myScenario.timelines.Count; i++)
        {
            GUILayout.Box("", GUILayout.Width(position.width - 30), GUILayout.Height(100));
            GUILayout.BeginArea(new Rect(10, (104)*i+5, position.width - 40, 100));
            //GUILayout.BeginArea(new Rect(10,10*i,position.width,100));
            EditorGUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(50), GUILayout.Height(90));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Player", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();            
            //GUILayout.Button("Player",GUILayout.Width(50), GUILayout.Height(90));
            myScenario.timelines[i].player = (Player)EditorGUILayout.ObjectField(myScenario.timelines[i].player, typeof(Player), true, GUILayout.Width(50), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60),GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            GUILayout.Button("Event", GUILayout.Width(60), GUILayout.Height(90));
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
            //Debug.Log(i);
        }

        if (GUILayout.Button("Add Player"))
        {
            myScenario.AddTimeline();
        }
        EditorGUILayout.EndScrollView();
               

        /*
        EditorGUI.DrawRect(new Rect(10, 50, position.width-10, 100), Color.white);

        

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        BeginWindows();

        // All GUI.Window or GUILayout.Window must come inside here
        windowRect = new Rect(10, 50, position.width - 10, 100);
        windowRect = GUILayout.Window(1, windowRect, DoWindow, "Player1");
        windowRect = GUILayout.Window(2, new Rect(10, 160, position.width - 10, 100), DoWindow, "Player2");
        windowRect = GUILayout.Window(3, new Rect(10, 280, position.width - 10, 300), DoWindow, "Player3");

        EndWindows();

    }


    void DoWindow(int unusedWindowID)
    {
        GUILayout.Button("Hi");
        //GUI.DragWindow();
    }
    */
}
#endif