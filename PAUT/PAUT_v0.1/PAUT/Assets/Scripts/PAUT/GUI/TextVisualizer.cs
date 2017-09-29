using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PAUT;

public class TextVisualizer : MonoBehaviour {

    public Timeline timeline;
    Text text2D;
    TextMesh text3D;

	// Use this for initialization
	void Start () {
        text2D = GetComponent<Text>();
        text3D = GetComponent<TextMesh>();
        timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timeline.isFinished) {
            if(text2D!=null)
				text2D.text = "훈련이 모두 종료되었습니다.\n수고하셨습니다.";
            if(text3D!=null)
				text3D.text = "훈련이 모두 종료되었습니다.\n수고하셨습니다.";
        }
        else
        {
            if (text2D != null)
                text2D.text = "Stage " + timeline.curEventIdx + ": " + timeline.player.curEvent.instruction.toDo;
            if (text3D != null)
                text3D.text = "Stage " + timeline.curEventIdx + ": " + timeline.player.curEvent.instruction.toDo;
        }
	}
}
