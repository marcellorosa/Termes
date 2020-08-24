﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SimManager : MonoBehaviour {

    //Gameobject References
    public CentralController centralController;
    public TermiteTS tileSystem;
    public InterfaceFSM interfaceFSM;

    //Constants
    const string defaultSupervisor = "S2_3X3.xml";

    //Simulation Parameters
    public bool isFastAnim;

    //Scenario Parameters
    public string supervisorName;
    public bool isMultibot;
    Coord size;

    void Start() {

        LoadScenario();

        //Initialize TermiteTS
        tileSystem.Initialize(size.x, size.y, "TermiteTile");

        //Reference CentralController
        centralController.Initialize();

        //Initialize InterfaceFSM
        interfaceFSM.Initialize();
    }

    //Load the Simulation for one supervisor
    void LoadScenario() {

        //Load Selected Scenario or the Default one 
        GameObject sceneInfo = GameObject.FindGameObjectWithTag("SceneInfo");

        if (sceneInfo != null) {
            supervisorName = sceneInfo.GetComponent<Text>().text;
            print("Loaded: " + supervisorName + "supervisor");
        } else {
            supervisorName = defaultSupervisor;
            print("Loaded: " + defaultSupervisor + "(Default) supervisor");
        }


        // Setup Scenario Parameters
        FSM fsm = new FSM(supervisorName);
        isMultibot = fsm.isMultiBot;
        size = fsm.size;


        Destroy(sceneInfo);

    }

    public void FastAnimToggleListener(bool isFastAnimState) {

        isFastAnim = isFastAnimState;

    }

}