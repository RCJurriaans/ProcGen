using UnityEngine;
using System.Collections;

public class GenerateCubes : MonoBehaviour {

	public GameObject cube;
	private int CubeAmount;
	public int maxCubes;

	public float timeBetweenCubes;
	private float timeSinceCube;

	// Use this for initialization
	void Start () {
		CubeAmount = 0;
		timeSinceCube = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		timeSinceCube += Time.deltaTime;

		if(timeSinceCube>=timeBetweenCubes && CubeAmount<maxCubes){
			Instantiate(cube);
			timeSinceCube = 0;
			CubeAmount++;
        }
	}
}
