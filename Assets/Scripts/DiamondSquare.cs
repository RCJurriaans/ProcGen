using UnityEngine;
using System.Collections;

public static class DiamondSquare {
	static float[,] noiseMap;
	static int max;
	static float maxNoiseHeight = float.MinValue;
	static float minNoiseHeight = float.MaxValue;
	public static float[,] GenerateNoiseMap(int mapWidth, int seed, float noiseScale, Vector2 offset){


		noiseMap = new float[mapWidth, mapWidth];

		max = mapWidth-1;
		// Initialize corners
		noiseMap [0, 0] = 0.5f;
		noiseMap [mapWidth-1, 0] = 0.5f;
		noiseMap [0, mapWidth-1] = 0.5f;
		noiseMap [mapWidth-1, mapWidth-1] = 0.5f;

		divide ();
		for(int y = 0; y < mapWidth; y++){
			for(int x = 0; x < mapWidth; x++){
				noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
			}
		}
		return noiseMap;
	}

	public static void divide(){
		float h = 2f;
		for (int sideLength = max; sideLength>=2; sideLength/=2) {
			int halfSide = sideLength/2;
			h = h*0.4f;
			for(int x=0;x<max;x+=sideLength){
				for(int y=0;y<max;y+=sideLength){
					//x, y is upper left corner of square
					//calculate average of existing corners
					float avg = noiseMap[x,y] + noiseMap[x+sideLength,y] + noiseMap[x,y+sideLength] + noiseMap[x+sideLength,y+sideLength];
					avg /= 4.0f;
					avg += (Random.Range (0f,1f)*2f*h) - h;
					//center is average plus random offset
					if(avg > maxNoiseHeight){
						maxNoiseHeight= avg;
					}
					if(avg < minNoiseHeight){
						minNoiseHeight= avg;
					}
					noiseMap[x+halfSide,y+halfSide] = avg;

				}
			}

			for(int x=0;x<max;x+=halfSide){
				for(int y=(x+halfSide)%sideLength;y<max;y+=sideLength){
					float avg = noiseMap[(x-halfSide+max+1)%(max+1),y] + noiseMap[(x+halfSide)%(max+1),y] + noiseMap[x,(y+halfSide)%(max+1)] + noiseMap[x,(y-halfSide+(max+1))%(max+1)];
					avg /= 4.0f;

					avg += (Random.Range (0f,1f)*2f*h) - h;
					if(avg > maxNoiseHeight){
						maxNoiseHeight= avg;
					}
					if(avg < minNoiseHeight){
						minNoiseHeight= avg;
					}
					noiseMap[x,y] = avg;

					if(x == 0)  noiseMap[max,y] = avg;
					if(y == 0)  noiseMap[x,max] = avg;
				}
			}

		}
	}
	public static void square(int x, int y, int half, float random){
			float tl = noiseMap [x-half, y-half];
			float bl = noiseMap [x-half, y + half];
			float tr = noiseMap [x+half, y];
			float br = noiseMap [x+half, y + half];
			noiseMap [x, y] = average (tl, bl, tr, br);

	}

	public static void diamond(int x, int y, int half, float random){
		int wlX = (x-half);
		if (wlX < 0) {
			wlX *= -1;
		}
		int wrX = (x+half)%(max);
		int wtY = (y-half);
		if (wtY < 0) {
			wtY *= -1;
		}
		int wbY = (y+half)%(max);
		float l = noiseMap [wlX, y];
		float r = noiseMap [wrX, y];
		float t = noiseMap [x, wtY];
		float b = noiseMap [x, wbY];

		noiseMap [x, y] = average (l, r, t, b);
		}

	public static float average(float a, float b, float c, float d){
		float ave = 0.25f*(a + b + c + d);
		return ave;
	}
}
