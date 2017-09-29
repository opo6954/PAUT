﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
/*
 * Asset editor
 * 
 * assetList에서 더블클릭시 asset이 선택되면서 asset property 창에 .asset의 내용이 추가되고 수정 가능하게 됨
 * 
 * 그리고 in-situ placement 버튼도 넣자
 * 
 * */
    public class AssetEditor : WindowTemplate
    {
        //public delegate void OnDoubleClickForEditor(AuthorableAsset aa);

        //in-situ placement를 위해서 있음
        [SerializeField]
        ViewChanger fpsChar;
         
        [SerializeField]
        InsituEditor insituEditor;

        //asset list window handler임, update를 위해서...
        [SerializeField]
        AssetListWindowHandler assetListWindowHandler;

        //선택된 asset 이름 설정하는 곳
        [SerializeField]
        UnityEngine.UI.Text currAssetName;


        //선택된 asset 이미지 설정하는 곳
        [SerializeField]
        UnityEngine.UI.RawImage currAssetImg;

        //asset의 property가 진열되는 위치
        [SerializeField]
        GameObject propertyTemplatePosition;

        //추가적인 정보 전달을 위한 subwindow instance
        [SerializeField]
        SubwindowManager subWindow;

        //asset trigger를 관리하는 editor
        [SerializeField]
        AssetTriggerEditor assetTriggerEditor;

        //asset의 propertyUI prefab을 관장하는 녀석
        Object propertyUIPrefab;

        //가장 최근에 바뀐 asset 정보
        AuthorableAsset currAssetInfo;
        
        //최근 asset의 본래 정보
        AuthorableAsset prevAssetInfo;

        //asset의 preview 이미지 저장
        Texture2D currPreviewImg;

        //current asset의 이름 저장
        string currFileName = "";
        
        //property UI에 대한 prefab
        private readonly string propertyUITemplatePath = "AssetEditor/propertyUITemplate";



        //asset list window handler한테 callback instance를 이 함수를 통해 넘겨줌/ asset 정보가 AuthorableAsset을 통해서 넘겨진다
        //그러면 이제 시작됨    
        public void OnSelectAsset(string _fileName, AuthorableAsset _currAssetInfo, Texture2D _currPreviewImg)
        {
            currAssetInfo = _currAssetInfo;
            currPreviewImg = _currPreviewImg;
            currFileName = _fileName;
            

            currAssetName.text = currAssetInfo.ObjectName;
            currAssetImg.texture = currPreviewImg;

            //이 부분에서 assetTrigger list를 업데이트핮
            Debug.Log("On select asset...");
            Debug.Log("Name of asset: " + currAssetInfo.ObjectName);
            assetTriggerEditor.OnSelectAsset();
            assetTriggerEditor.OnSetAssetTriggerListFromAsset(currAssetInfo.assetTriggerXmlTemplateList);
            
            visualizeAssetProperty();

        }
        //기존의 property 없애기
        public void clearAssetProperty()
        {
            for (int i = 0; i < propertyTemplatePosition.transform.childCount; i++)
            {
                GameObject.Destroy(propertyTemplatePosition.transform.GetChild(i).gameObject);
            }
        }
        //asset의 property의 표시
        public void visualizeAssetProperty()
        {
            clearAssetProperty();

            for (int i = 0; i < currAssetInfo.variableContainer.getNumberOfParameters(); i++)
            {
                XmlTemplate xt = currAssetInfo.variableContainer.getParameters(i);

                

                //항목 소환
                GameObject go = (GameObject)GameObject.Instantiate(propertyUIPrefab, new Vector3(), new Quaternion(), propertyTemplatePosition.transform);

                go.name = xt.Name;

                go.GetComponent<PropertyVisualizeHandler>().visualizePropertyAll(xt, i,50, -100);
            }

        }

        //add asset trigger 버튼을 누를 때임
        //이제 asset trigger를 저장해야 할텐데...

            //Add assetTriggerButton을 누를 경우
        public void OnAddAssetTriggerButton()
        {
            //assetTriggerEditor.gameObject.SetActive(true);            
        }
        //public void OnGetAssetTriggerInfo(string triggerName, string[] attachedActionLst, string[] actionList, string[] triggerParamList, string[] actionParamList)
        public void OnAddAssetTrigger(string triggerName, string[] attachedActionLst, string[] actionList, string[] triggerParamList, string[] actionParamList)
        {
            assetTriggerEditor.gameObject.SetActive(false);

            AssetTriggerXmlTemplate att = new AssetTriggerXmlTemplate("", "");

            att.setValue(triggerName, attachedActionLst, actionList, triggerParamList, actionParamList);

            currAssetInfo.assetTriggerXmlTemplateList.Add(att);
        }

        
        //asset parameter 정보를 입력된 값을 바탕으로 저장하자
        public void OnSaveAsset()
        {
            currAssetInfo.assetTriggerXmlTemplateList = assetTriggerEditor.OnPassAssetTriggerListToAsset();

            





            currAssetInfo.testSerialize(currFileName);
            subWindow.displayStatus("Save done with " + "\n" + currAssetInfo.ObjectName + ".asset");

            assetListWindowHandler.UpdateFileLists();
        }
        
        
        //asset parameter 정보를 저장하기 전의 값으로 되돌리자
        public void OnInitAsset()
        {
            //추후 구현
            /*
            Debug.Log("init?");

            currAssetInfo = prevAssetInfo;
            
            visualizeAssetProperty();
            */
        }


        //On situ placement 버튼 누를 시 실행되는 callback
        public void OnEnterSituPlacement()
        {
            List<ObjectTemplate> _objList = new List<ObjectTemplate>();
            List<AuthorableAsset> assetList = assetListWindowHandler.getAssetList();

            for(int i=0; i<assetList.Count; i++)
            {
                _objList.Add(assetList[i] as ObjectTemplate);
            }

            
            //insidu editor한테 파라미터 넘겨주기
            insituEditor.setParameter("AssetFiles/prefab/","asset", _objList);
            insituEditor.initialize();
            insituEditor.OnClickInSituMode(currAssetInfo.ObjectName);
        }

        public void OnSaveFromInSituPlacement()
        {
            currAssetInfo.testSerialize(currFileName);
            //LHWLHW
            

            assetListWindowHandler.initialize();
            assetListWindowHandler.UpdateFileLists();
        }

        

        // Use this for initialization
        void Start()
        {
            propertyUIPrefab = Resources.Load(propertyUITemplatePath);
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}