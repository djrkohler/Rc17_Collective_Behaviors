using UnityEngine;
using UnityEditor;

[System.Serializable]
public class BoundRange 
{
    public float XMax;
    public float XMin;
    public float YMax;
    public float YMin;
    public float ZMax;
    public float ZMin;

    public Vector3 Mutation(string axis,float amout,Vector3 original)
    {
        var m = original;

        switch (axis)
        {
            case "X":
                XMax += amout;
                XMin -= amout;
                m.x = GetVector().x;
                break;
            case "Y":
                YMax += amout;
                YMin -= amout;
                m.y = GetVector().y;
                break;
            case "Z":
                ZMax += amout;
                ZMin -= amout;
                m.z = GetVector().z;
                break;
        }

        return m;
       
    }

    public BoundRange(BoundRange bound)
    {
        XMin = bound.XMin;
        XMax = bound.XMax;
        YMin = bound.YMin;
        YMax = bound.YMax;
        ZMin = bound.ZMin;
        ZMax = bound.ZMax;
    }

    public Vector3 GetVector()
    {
        var x = Random.Range(XMin, XMax);
        var y = Random.Range(YMin, YMax);
        var z = Random.Range(ZMin, ZMax);

        return new Vector3(x, y, z);
    }
    
}