using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class PheromoneSteeringSystem : SystemBase
{
	static Texture2D texture;
    static int mapSize;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        AntDefaults defaults = Camera.main.GetComponent<AntDefaults>();
        texture = defaults.pheromoneMap;
        mapSize = defaults.mapSize;
    }

    static int PheromoneIndex(int x, int y, int mapSize)
    {
		return x + y * mapSize;
	}

	static float PheromoneSteering(float2 position, float facingAngle, float distance, int mapSize)
	{
		float output = 0;

		for (int i=-1;i<=1;i+=2) {
			float angle = facingAngle + i * Mathf.PI*.25f;
			float testX = position.x + Mathf.Cos(angle) * distance;
			float testY = position.y + Mathf.Sin(angle) * distance;

			if (testX <0 || testY<0 || testX>=mapSize || testY>=mapSize) {

			} 
			else {
				//int index = PheromoneIndex((int)testX,(int)testY,mapSize);
				float value = texture.GetPixel((int)testX,(int)testY).r;
				output += value*i;
			}
		}
		return Mathf.Sign(output);
	}

    protected override void OnUpdate()
    {
    	//var mapResolution = GetSingleton<MapResolution>();
    	//Debug.Log("test map =" + mapResolution.value);

        //AntDefaults defaults = GameObject.Find("MainCamera").GetComponent<AntDefaults>();
        //Debug.Log("test map 2 =" + defaults.mapSize);
  

		Entities.WithAll<Ant>().ForEach((in Position pos, in DirectionAngle directionAngle) => {

			float pheromoneSteering = PheromoneSteering(pos.value, directionAngle.value, 3f, mapSize);

		}).ScheduleParallel();
    }
}
