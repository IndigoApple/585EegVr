using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLevel2 : Collision1
{

    //public int[,] objects = new int[21, 21];
    [SerializeField] private GameObject[] obj;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 21; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                if (i == 0 || j == 0 || i == 20 || j == 20)
                    objects[i, j] = -1;
                else
                    objects[i, j] = 0;
            }
        }
        objects[11, 13] = 1;
        objects[13, 1] = 2;
        objects[5, 17] = 3;
        objects[17, 5] = 4;
    }

    public override void callEvent(int x, int y)
    {
        Event[] eventscripts = obj[objects[x, y] - 1].GetComponents<Event>();
        foreach (Event script in eventscripts)
        {
            script.OnTriggerEnter(null);
        }
    }

    public override void endEvent(int x, int y)
    {
        Event[] eventscripts = obj[objects[x, y] - 1].GetComponents<Event>();
        foreach (Event script in eventscripts)
        {
            script.OnTriggerExit(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
