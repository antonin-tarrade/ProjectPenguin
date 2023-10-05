using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;


// This script calculates local pathfinding based on 2 constraints :
// 1 - The object is trying to reach a certain point represetend by a target in game
// 2 - The object must not be too close to certain objects based on their tags
// Algorithm is a follows :
// 1.We find the closest object, if it is outside of the constraints, we move freely else we calculate an appropriate pathfinding
// 2.We calculate all directions possible on the circle determined by the constraint, and scale it to make behaviour more or less local
// 3.We iterate on each point and select all the satisfying points, which are the ones for which the constraint is less than the current one
// 4.We keep the best point, which is the one that minimizes the distance to the target : this is our local destination

//TO DO : deplacement sur X OU Y (ez), gerer les mediatrices(?), les evitements de la target elle meme(??), optimisation (bcp à faire), fix les tremblements statiques (enormement a faire)
public class Pathfinder : MonoBehaviour
{
    // Class for editor purpose only (dictionnary not serializable)
    [Serializable]
    public class PathfindingKVP
    {
        public string tag;
        public float minDistance;
    }

    // To visualize the algorithm layers
    public bool debug1;
    public bool debug2;
    public bool debug3;
    public bool debug4;

    // Variable for custom frequency update
    float time;

    [Header("Movement")]
    [SerializeField] float speed;
    Vector3 currentDirection;
    // targetName does not matter if targetRef is set
    [SerializeField] string targetName;
    [SerializeField] GameObject targetRef;
    GameObject target;

    [Header("Calculations")]
    [SerializeField] float updateFrequency;
    // The number of points to calculate for the circle, a high resolution is not needed for good results and brings heavy calculations
    [SerializeField] int circlePointsResolution;
    // How much we want the movement to be precise and local
    [SerializeField] float localisationFactor;
    

    // For editor only
    [SerializeField] List<PathfindingKVP> tagsToAvoid = new List<PathfindingKVP>();
    // References for all types of objects to avoid
    Dictionary<string, float> objectsToAvoid = new Dictionary<string, float>();


    // Start is called before the first frame update
    void Start()
    {
        // Copy editor values into dictionnary
        foreach (PathfindingKVP kvp in tagsToAvoid)
        {
            objectsToAvoid.Add(kvp.tag, kvp.minDistance);
        }

        target = (targetRef != null)? targetRef: GameObject.Find(targetName);

        if (localisationFactor <= 0) localisationFactor = 1;

        time = updateFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        // Custom frequency update loop
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

    // General method that computes all of the algorithm and stores the result in currentDirection
    void FindDirection()
    {
        GameObject[] objectsDetected = FindObjectsToAvoid();
        GameObject closestObject = GetClosestObject(objectsDetected, transform.position);
        bool isFree = IsFree(transform.position, closestObject);
        if (isFree)
        {
            currentDirection = target.transform.position - transform.position;
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

    // Find all the instances of objects that are tagged such as we need to avoid them
    GameObject[] FindObjectsToAvoid()
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

    // Find the closest object from a set of objects and a position
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

        if (debug1)
        {
            if (closestObject != null)
            {
                Debug.DrawLine(position, closestObject.transform.position, Color.red);
            }

        }
        return closestObject;
    }

    // Determine if we are free or if we must compute a path because of a constraint
    bool IsFree(Vector3 position, GameObject obstacle)
    {
        if (obstacle == null) return true;
        float distance = Vector3.Distance(position, obstacle.transform.position);
        float distanceToRespect;
        objectsToAvoid.TryGetValue(obstacle.tag, out distanceToRespect);

        if (debug1)
        {
            Debug.DrawCircle(position, Mathf.Min(distance, distanceToRespect), 30, Color.red);
        }

        return (distanceToRespect <= distance);
    }

    // Generate a number of resolution points on the circle of center position, radius distance
    Vector3[] GeneratePointsBasedOn(Vector3 position, float distance, int resolution)
    {
        Vector3[] points = new Vector3[resolution];
        
        float step = 2* Mathf.PI / resolution;
        float angle = 0f;
        for (int i = 0; i < resolution; i++) 
        {
            points[i] = position + distance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            if (debug2)
            {
                Debug.DrawCircle(points[i], 0.1f, 10, Color.blue);
            }
            angle += step;
        }
        return points;
    }

    // From a set of points, get all the points that have a better constraint than minDistance
    Vector3[] GetValidPoints(Vector3[] points, float minDistance)
    {
        List<Vector3> validPoints = new List<Vector3>();

        foreach(Vector3 point in points)
        {
            Vector3 movement = point - transform.position;
            movement = transform.position + (movement / localisationFactor);
            GameObject[] objectsDetected = FindObjectsToAvoid();
            GameObject closestObject = GetClosestObject(objectsDetected, movement);
            float distance = Vector3.Distance(movement, closestObject.transform.position);

            if (distance >= minDistance)
            {
                validPoints.Add(movement);
                if (debug3) Debug.DrawCircle(movement, distance, 20, Color.yellow);
            }
            
        }

        return validPoints.ToArray();
    }

    // Get the best point of a set of points
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

        if (debug4) Debug.DrawCircle(bestPoint, bestDistance, 30, Color.green);
        return bestPoint;
    }

    void MoveTo(Vector3 position)
    {
        transform.position += currentDirection.normalized * Time.deltaTime * speed;
    }


}
