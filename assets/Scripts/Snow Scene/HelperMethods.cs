using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelperMethods
{
    public static List<GameObject> GetChildren(this GameObject Accessories)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in Accessories.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }
}
