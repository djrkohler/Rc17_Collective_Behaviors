using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class SaveCamera : MonoBehaviour
{
    private Dictionary<String, Vector3> SavedPositions;
    private Dictionary<String, Quaternion> SavedRotations;
    private Dictionary<String, Vector3> SavedThisPositions;
    private string defViewName = "New View ";
    private string inputViewName;

    private int viewsCount = 0;
    [SerializeField] private InputField input;
    [SerializeField] private Dropdown drop;

    public bool RestoreView { get; set; }



	// Use this for initialization
	void Start () {
        SavedPositions=new Dictionary<string, Vector3>();
        SavedRotations=new Dictionary<string, Quaternion>();
        SavedThisPositions= new Dictionary<string, Vector3>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
  

    public void SetView(int value)
    {
        RestoreView = true;
        var option = drop.options[value];
        var key = option.text;

        transform.parent.localPosition = SavedPositions[key];
        transform.parent.localRotation = SavedRotations[key];
        transform.localPosition = SavedThisPositions[key];
    }

    public void SaveView()
    {
        var p = transform.parent.localPosition;
        var q = transform.parent.localRotation;
        var tp = transform.localPosition;

        string key;
        if (input.text!="")
        {
            key = inputViewName = input.text;
            if (CheckExistedKeys(key))
            {
                if (_counter != 0)
                {
                    key = inputViewName + _counter;
                    _counter = 0;
                }
                else
                {
                    key = inputViewName;
                }
            }

        }
        else
        {
            key = defViewName + viewsCount;
        }

    
        print("key" + key);
        SavedPositions.Add(key, p);
        SavedRotations.Add(key, q);
        SavedThisPositions.Add(key,tp);
       

        List<string> k = new List<string>(1);
        k.Add(key);
        drop.AddOptions(k);
        

        viewsCount++;
    }


    private int _counter = 0;
    bool  CheckExistedKeys(string key)
    {
        //var savedKey = key;
        if (!SavedPositions.ContainsKey(key)&&!SavedRotations.ContainsKey(key))
        {
            return true;
        }
        else
        {
            _counter++;
            key = inputViewName + _counter;
           
            return CheckExistedKeys(key);
        }
    }
}
