using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Pathfinder : MonoBehaviour
{

    [Serializable]
    public class PathfindingKVP
    {
        public string tag;
        public float minDistance;
    }

    public bool debug;

    float time;

    [Header("Movement")]
    [SerializeField] float speed;
    Vector3 currentDirection;
    [SerializeField] string targetName;
    [SerializeField] GameObject targetRef;

    [Header("Calculations")]
    [SerializeField] float updateFrequency;
    [SerializeField] int circlePointsResolution;
    [SerializeField] int localisationFactor;
    GameObject target;

    
    [SerializeField] List<PathfindingKVP> tagsToAvoid = new List<PathfindingKVP>();
    Dictionary<string, float> objectsToAvoid = new Dictionary<string, float>();


    // Start is called before the first frame update
    void Start()
    {
        foreach (PathfindingKVP kvp in tagsToAvoid)
        {
            objectsToAvoid.Add(kvp.tag, kvp.minDistance);
        }

        target = (targetRef != null)? targetRef: GameObject.Find(targetName);

        if (localisationFactor < 1) localisationFactor = 1;

        time = updateFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            time = updateFrequency;
            FindDirection();
        }
        else
        {
            time -= updateFrequency;
        }
        MoveTo(currentDirection);
    }

    void FindDirection()
    {
        GameObject[] objectsDetected = FindObjectsToAvoid(transform.position);
        GameObject closestObject = GetClosestObject(objectsDetected, transform.position);
        bool isFree = IsFree(transform.position, closestObject);
        if (isFree)
        {
            currentDirection = target.transform.position;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, closestObject.transform.position);
            Vector3[] points = GeneratePointsBasedOn(transform.position, distance, circlePointsResolution);
            Vector3[] validsPoints = GetValidPoints(points, distance);
            if (validsPoints.Length > 0)
            {
                Vector3 bestPoint = GetBestPoint(validsPoints);
                currentDirection = (bestPoint - transform.position).normalized * speed;
            }
        }
    }

    GameObject[] FindObjectsToAvoid(Vector3 position)
    {
        List<GameObject> objectsDetected = new List<GameObject>();
        foreach (KeyValuePair<string,float> kvp in objectsToAvoid)
        {
            List<GameObject> objs = new List<GameObject>(GameObject.FindGameObjectsWithTag(kvp.Key));
            objs.Remove(gameObject);
            objectsDetected.AddRange(objs);
        }

        return objectsDetected.ToArray();
        
    }

    GameObject GetClosestObject(GameObject[] objects, Vector3 position)
    {
        GameObject closestObject = null;
        float minDistance = float.MaxValue;
        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = obj;
            }
        }

        if (debug)
        {
            if (closestObject != null)
            {
                Debug.DrawLine(position, closestObject.transform.position, Color.red);
            }

        }
        return closestObject;
    }

    bool IsFree(Vector3 position, GameObject obstacle)
    {
        if (obstacle == null) return true;
        float distance = Vector3.Distance(position, obstacle.transform.position);
        float distanceToRespect;
        objectsToAvoid.TryGetValue(obstacle.tag, out distanceToRespect);

        if (debug)
        {
            Debug.DrawCircle(position, Mathf.Min(distance, distanceToRespect), 30, Color.red);
        }

        return (distanceToRespect <= distance);
    }

    Vector3[] GeneratePointsBasedOn(Vector3 position, float distance, int resolution)
    {
        Vector3[] points = new Vector3[resolution];
        
        float step = 2* Mathf.PI / resolution;
        float angle = 0f;
        for (int i = 0; i < resolution; i++) 
        {
            points[i] = position + distance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            if (debug)
            {
                Debug.DrawCircle(points[i], 0.1f, 10, Color.blue);
            }
            angle += step;
        }
        return points;
    }

    Vector3[] GetValidPoints(Vector3[] points, float minDistance)
    {
        List<Vector3> validPoints = new List<Vector3>();

        foreach(Vector3 point in points)
        {
            Vector3 movement = point - transform.position;
            movement = transform.position + (movement / localisationFactor);
            GameObject[] objectsDetected = FindObjectsToAvoid(movement);
            GameObject closestObject = GetClosestObject(objectsDetected, movement);
            float distance = Vector3.Distance(movement, closestObject.transform.position);

            if (distance >= minDistance)
            {
                validPoints.Add(movement);
                if (debug) Debug.DrawCircle(movement, distance, 20, Color.yellow);
            }
            
        }

        return validPoints.ToArray();
    }

    Vector3 GetBestPoint(Vector3[] validPoints)
    {
        Vector3 bestPoint = Vector3.zero;
        float bestDistance = float.MaxValue;

        foreach (Vector3 point in validPoints)
        {
            float distance = Vector3.Distance(target.transform.position, point);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPoint = point;
            }
        }

        if (debug) Debug.DrawCircle(bestPoint, bestDistance, 30, Color.green);
        return bestPoint;
    }

    void MoveTo(Vector3 position)
    {
        

        if (debug)
        {
            Debug.DrawLine(transform.position, position, Color.yellow, Time.deltaTime);
        }

        transform.position += currentDirection.normalized * Time.deltaTime * speed;
    }


}
