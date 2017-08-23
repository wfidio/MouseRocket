using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour {

    [SerializeField]
    private GameObject[] aviableRooms;

    [SerializeField]
    private List<GameObject> currentRooms;

    private float screenWidthInPoints;

    [SerializeField]
    private GameObject[] aviableObjects;

    [SerializeField]
    private List<GameObject> objects;

    public float objectMinDistance = 5.0f;
    public float objectMaxDistance = 10.0f;

    public float objectMinY = -1.4f;
    public float objectMaxY = 1.4f;

    public float objectMinRotation = -45.0f;
    public float objectMaxRotation = 45.0f;



	// Use this for initialization
	void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {

	}


    void FixedUpdate()
    {
        GenerateRoomIfRequired();
        GenerateObjectIfRequired();
    }


    void AddRoom(float farthestRoomEndX)
    {
        int randomRoomIndex = Random.Range(0, aviableRooms.Length);

        GameObject room = (GameObject)Instantiate(aviableRooms[randomRoomIndex]);

        float roomWidth = room.transform.Find("floor").localScale.x;

        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        room.transform.position = new Vector3(roomCenter, 0, 0);

        currentRooms.Add(room);
    }

    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, aviableObjects.Length);

        GameObject obj = (GameObject)Instantiate(aviableObjects[randomIndex]);


        float objectPositionX = lastObjectX + Random.Range(objectMinDistance, objectMaxDistance);
        float randomY = Random.Range(objectMinY, objectMaxY);

        obj.transform.position = new Vector3(objectPositionX, randomY, 0.0f);


        float rotation = Random.Range(objectMinRotation, objectMaxRotation);

        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        objects.Add(obj);
    }

    void GenerateObjectIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objToRemove = new List<GameObject>();

        foreach(var obj in objects)
        {
            float objX = obj.transform.position.x;

            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectX)
            {
                objToRemove.Add(obj);
            }
            
        }

        foreach(var obj in objToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }


    void GenerateRoomIfRequired()
    {
        List<GameObject> roomToRemove = new List<GameObject>();

        bool addRooms = true;

        float playerX = transform.position.x;

        float removeRoomX = playerX - screenWidthInPoints;

        float addRoomX = playerX + screenWidthInPoints;

        float farthestRoomEndX = 0;

        foreach(var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - roomWidth * 0.5f;
            float roomEndX = roomStartX + roomWidth;

            if(roomStartX > addRoomX)
            {
                addRooms = false;
            }

            if(roomEndX < removeRoomX)
            {
                roomToRemove.Add(room);
            }

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach(var room in roomToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }
}
