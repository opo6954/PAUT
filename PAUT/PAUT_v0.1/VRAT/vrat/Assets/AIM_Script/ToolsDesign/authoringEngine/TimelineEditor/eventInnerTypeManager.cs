using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    //event template에서 내부 inner type에서의 연결을 담당함

    public static class PRIMColor
    {
        public static Color noneAttach = new Color(127,127,127);

        public static Color triggerColor = new Color(0,149,255);

        public static Color actionColor = new Color(214,91,0);

        public static Color instColor = new Color(165,167,0);

    }
    
    //일단 action에 붙이자
    public class eventInnerTypeManager : MonoBehaviour
    {

        /*
        public delegate void OnClickAssetImg(int orderIdx, PRIMDETECTDROP _type);
        public OnClickAssetImg callback;
        */

        [SerializeField]
        UnityEngine.UI.Button button;
        
        public bool isDetectDrop(Vector2 position)
        {
            

            var rt = button.GetComponent<RectTransform>();
            Vector2 localPos = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, position, Camera.main, out localPos);

            //if (RectTransformUtility.RectangleContainsScreenPoint(rt, localPos, Camera.main) == true)
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, position, Camera.main) == true)
            {
                return true;
            }
            return false;
        }
        
      
        

    }
}