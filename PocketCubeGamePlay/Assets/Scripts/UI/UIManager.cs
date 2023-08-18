using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class UIManager
    {
        private static UIManager instance;

        private Transform uiRoot;

        private Dictionary<string, string> pathDict;

        private Dictionary<string, GameObject> prefabDict;

        public Dictionary<string, BasePanel> panelDict;


        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        public Transform UIRoot
        {
            get
            {
                if (uiRoot == null)
                {
                    if (GameObject.Find("Canvas"))
                    {
                        uiRoot = GameObject.Find("Canvas").transform;
                    }
                    else
                    {
                        uiRoot = new GameObject("Canvas").transform;
                    }   

                };

                return uiRoot;
            }
        }

        private UIManager()
        {
            InitDicts();
        }

        private void InitDicts()
        {
            prefabDict = new Dictionary<string, GameObject>();

            panelDict = new Dictionary<string, BasePanel>();

            pathDict = new Dictionary<string, string>()
            {
                {UIConst.MainMenuPanel, "Panel/MainMenuPanel"},
                {UIConst.LevelMapPanel, "Panel/LevelMapPanel"},
                {UIConst.SettingPanel, "Panel/SettingPanel"},
                {UIConst.CollectionPanel, "Panel/CollectionPanel"},
                {UIConst.CreditPanel,"Panel/CreditPanel" },
                {UIConst.ExitPanel, "Panel/ExitPanel" }


            };
        }

        public BasePanel OpenPanel(string name)
        {
            BasePanel panel = null;

            // Check if open
            if (panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError(name + "Already Exit");
                return null;
            }

            // Check if the path was written
            string path = "";
            if (!pathDict.TryGetValue(name, out path))
            {
                Debug.LogError(name + "name error or the path is null");
                return null;
            }

            //Use pre-load prefab
            GameObject panelPrefab = null;
            if (!prefabDict.TryGetValue(name, out panelPrefab))
            {
                string realPath = "Prefab/" + path;
                panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
                prefabDict.Add(name, panelPrefab);
            }

            //Open
            GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
            panel = panelObject.GetComponent<BasePanel>();
            panelDict.Add(name, panel);
            panel.OpenPanel(name);
            return panel;

        }

        public bool ClosePanel(string name)
        {
            BasePanel panel = null;
            if (!panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError(name + "is not yet open"); 
                return false;
            }

            panel.ClosePanel();
            return true;
        }

        

        
    }
    public class UIConst
    {
        public const string MainMenuPanel = "MainMenuPanel";
        public const string LevelMapPanel = "LevelMapPanel";
        public const string SettingPanel = "SettingPanel";
        public const string CollectionPanel = "CollectionPanel";
        public const string CreditPanel = "CreditPanel";
        public const string ExitPanel = "ExitPanel";
}


