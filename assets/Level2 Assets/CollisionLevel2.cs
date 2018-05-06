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
        objects[9, 7] = 3;
        objects[17, 5] = 4;
        objects[17, 5] = 5;
        objects[9, 7] = 6;
        objects[3, 12] = 7;
        objects[13, 1] = 8;
        objects[11, 13] = 9;
        objects[1, 13] = -1;
        objects[2, 13] = -1;
        objects[2, 14] = -1;
        objects[3, 14] = -1;
        objects[1, 15] = -1;
        objects[2, 15] = -1;
        objects[9, 16] = -1;
        objects[15, 19] = -1;
        objects[16, 19] = -1;
        objects[17, 19] = -1;
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
