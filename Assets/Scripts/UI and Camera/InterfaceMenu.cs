﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;

public class InterfaceMenu : MonoBehaviour {
    //public GameObject termite;

    GameObject selectedBtn;

    public GameObject menuSelection;
    public GameObject menuDisplay;
    public GameObject placeHolder;

    public bool fullScreen;

    // Start is called before the first frame update
    void Start() {

        //ShowDisplay();

        



        //Config MenuSelection Canvas
        menuSelection.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        menuSelection.GetComponent<Canvas>().worldCamera = Camera.main;
        menuSelection.GetComponent<Canvas>().planeDistance = 0.5f;


        //Show buttons
        ListSupervisors();

        //Create Start Button
        GameObject btn = CreateButton(new Vector3(0, 0, 0));
        SetButtonText(btn, "Start");
        btn.GetComponent<Button>().onClick.AddListener(StartClicked);
    }

    // Update is called once per frame
    void Update() {
        DisplayHandler();
    }

    public void ChangeResolution() {

        fullScreen = !fullScreen;

        Screen.fullScreen = fullScreen;
        
    }


    void ListSupervisors() {

        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\Supervisors";

        string[] files = Directory.GetFiles(path);

        List<string> supPaths = new List<string>();

        foreach (string file in files) {

            if (Path.GetExtension(file) == ".xml") {
                supPaths.Add(file);
            }

        }

        int btnCount = 1;
        foreach (string supPath in supPaths) {

            GameObject btn = CreateButton(new Vector3(0, -30 * btnCount, 0));
            SetButtonText(btn, supPath.Split('\\')[supPath.Split('\\').Length - 1]);
            btn.GetComponent<Button>().onClick.AddListener(() => SelectionClicked(btn));

            btnCount++;
        }

    }

    void SelectionClicked(GameObject btn) {
        selectedBtn = btn;
        DestroyDisplay();

        FSM selectedSupervisor = new FSM(selectedBtn.GetComponentInChildren<Text>().text);

        ShowDisplay(selectedSupervisor);
    }

    void StartClicked() {

        if (selectedBtn != null) {
            Initialize();
        }

    }

    void DisplayHandler() {
        if(selectedBtn != null) {
            placeHolder.GetComponent<Text>().text = selectedBtn.GetComponentInChildren<Text>().text;
        }

    }


    void Initialize() {

        GameObject sceneInfo = new GameObject("SceneInfo");
        sceneInfo.tag = "SceneInfo";
        sceneInfo.AddComponent<Text>();
        sceneInfo.GetComponent<Text>().text = selectedBtn.GetComponentInChildren<Text>().text;
        DontDestroyOnLoad(sceneInfo);

        SceneManager.LoadScene("TileSystem_Reform");
    }

    


    void ShowDisplay(FSM supervisorio) {

        Coord tempCoord = supervisorio.size;

        //Init TileSystem
        transform.GetComponent<TermiteTS>().Initialize(tempCoord.x, tempCoord.y, "TermiteTile");
        HeightMap final = new HeightMap(tempCoord.x, tempCoord.y);
        foreach (var state in supervisorio.statesConteiner.Values) {
            if(state.marked == true) {
                final = state.heightMap;
            }
            
        }
        transform.GetComponent<TermiteTS>().UpdateMap(final);
        //Handle Display Camera
        GameObject.Find("RightCamera").transform.position = transform.GetComponent<TermiteTS>().center + new Vector3(0, transform.GetComponent<TermiteTS>().center.magnitude, 0);
        GameObject.Find("RightCamera").GetComponent<MouseCamRotation>().centerObject = transform.GetComponent<TermiteTS>().floor;

        GameObject.Find("RightCamera").GetComponent<MouseCamRotation>().active = true;

    }

    void DestroyDisplay() {

        Destroy(GameObject.Find("Floor"));
        GameObject.Find("RightCamera").GetComponent<MouseCamRotation>().active = false;
        GameObject.Find("RightCamera").GetComponent<MouseCamRotation>().centerObject = GameObject.Find("RightCamera");
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            Destroy(tile);
        }

    }


    // Utilities Functions

    private GameObject CreateButton(Vector3 position) {
        GameObject btn = (GameObject)Instantiate(Resources.Load("Button"), position, Quaternion.identity);
        btn.transform.SetParent(menuSelection.transform, false);
        return btn;
    }

    protected void SetButtonText(GameObject btn, string text) {
        btn.GetComponentInChildren<Text>().text = text;
    }
}
