using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringParser : MonoBehaviour {
    // Start is called before the first frame update

    public string[] ConvertStringsToArray(string s) {
        string[] stringContents = s.Split(',');
        return stringContents;
    }

    public float ConvertStringToFloat(string s) {
        float convertToFloat = float.Parse(s);
        return convertToFloat;
    }

    public bool ConvertBoolToFloat(string s) {
        bool state = bool.Parse(s);
        return state;
    }
    public void Parse(string s) {

    }

    // Update is called once per frame
    void Update() {

    }
}
