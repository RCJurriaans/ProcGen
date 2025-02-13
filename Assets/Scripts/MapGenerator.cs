﻿using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;


	const int mapChunkSize = 241;
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistence;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
	public bool autoUpdate;

	public TerrainType[] regions;
	
	// Use this for initialization
	void Start () {
		GenerateMap ();
	}

	public void GenerateMap(){
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
		//float[,] noiseMap = DiamondSquare.GenerateNoiseMap(mapChunkSize, seed, noiseScale, offset);

		Color[] colourMap = new Color[mapChunkSize*mapChunkSize];
		for (int y=0; y<mapChunkSize; y++) {
			for(int x=0; x<mapChunkSize; x++){
				float currentHeight = noiseMap[x,y];
				for(int i=0; i<regions.Length; i++){
					if(currentHeight <= regions[i].height){
						float randomColor = Random.Range(-.03f, .03f);
						Color disturbColor = new Color(randomColor, randomColor, randomColor);
						colourMap[y*mapChunkSize+x] = regions[i].colour + disturbColor;
						break;
					}
				}
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		}
	}

	void OnValidate(){



		if (lacunarity < 1) {
			lacunarity=1;
		}
		if (octaves < 0) {
			octaves=0;
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}
