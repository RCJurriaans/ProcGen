using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;
	
	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistence;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

	public TerrainType[] regions;
	
	// Use this for initialization
	void Start () {
		GenerateMap ();
	}

	public void GenerateMap(){
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);
		//float[,] noiseMap = DiamondSquare.GenerateNoiseMap(mapWidth, seed, noiseScale, offset);

		Color[] colourMap = new Color[mapWidth*mapHeight];
		for (int y=0; y<mapHeight; y++) {
			for(int x=0; x<mapWidth; x++){
				float currentHeight = noiseMap[x,y];
				for(int i=0; i<regions.Length; i++){
					if(currentHeight <= regions[i].height){
						//noiseMap[x,y] = ((int)(currentHeight*20))/20f;
						if(noiseMap[x,y]<0.3){
							noiseMap[x,y] = 0.1f;
						}
						float randomColor = Random.Range(-.03f, .03f);
						Color disturbColor = new Color(randomColor, randomColor, randomColor);
						noiseMap[x,y]= Mathf.Pow (noiseMap[x,y],6)*1.2f;
						colourMap[y*mapWidth+x] = regions[i].colour + disturbColor;
						break;
					}
				}
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
		}
	}

	void OnValidate(){
		if (mapWidth < 1) {
			mapWidth=1;
		}
		if (mapHeight < 1) {
			mapHeight=1;
		}
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
