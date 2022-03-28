using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TerrainUI : MonoBehaviour{
    public TMP_InputField seedField;
    public TMP_InputField numObjectsField;

    public TMP_InputField sizeX;
    public TMP_InputField sizeY;
    public TMP_InputField sizeZ;
    public TMP_InputField noiseScale;
    public Button makeWorldButton;

    public TerrainBuilder terrain;

    void Start(){
        seedField.onValueChanged.AddListener(OnSeedChanged);
        numObjectsField.onValueChanged.AddListener(OnNumObjectsChanged);
        makeWorldButton.onClick.AddListener(OnMakeWorldPressed);
    }

    private void OnSeedChanged(string newValue){
    }

    private void OnNumObjectsChanged(string newValue){
    }

    private void OnMakeWorldPressed(){

        terrain.seed = Convert.ToInt32(seedField.text);
        terrain.size.x = Convert.ToInt32(sizeX.text);
        terrain.size.y = Convert.ToInt32(sizeY.text);
        terrain.size.z = Convert.ToInt32(sizeZ.text);
        terrain.terrainNoiseScale = Convert.ToSingle(noiseScale.text);
        terrain.numObjects = Convert.ToInt32(numObjectsField.text);
        terrain.MakeWorld();
    }

}
