using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class DecodePoints
{

    static float z;
    public static void DecodeAndCreate(string message)
    {

        //message = "[[(221,1438),(746,1500),(270,1900),(700,1982)]]";
        string[] separatingChars = { "[[", "],[", "]]", "(", "),(", ")", "," };
        string[] words = message.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

        var myList = new List<KeyValuePair<float, float>>();

        int j = 0;
        for (int i = 0; i < words.Length / 8; i++)
        {

            myList.Add(new KeyValuePair<float, float>(float.Parse(words[j + 0]), Screen.height - float.Parse(words[j + 1])));
            myList.Add(new KeyValuePair<float, float>(float.Parse(words[j + 2]), Screen.height - float.Parse(words[j + 3])));
            myList.Add(new KeyValuePair<float, float>(float.Parse(words[j + 4]), Screen.height - float.Parse(words[j + 5])));
            myList.Add(new KeyValuePair<float, float>(float.Parse(words[j + 6]), Screen.height - float.Parse(words[j + 7])));
            myList.Sort((x, y) => (y.Value.CompareTo(x.Value)));


            if (myList[0].Key > myList[1].Key)
            {
                KeyValuePair<float, float> temp = myList[0];
                myList[0] = myList[1];
                myList[1] = temp;
            }

            if (myList[2].Key > myList[3].Key)
            {
                KeyValuePair<float, float> temp = myList[2];
                myList[2] = myList[3];
                myList[3] = temp;
            }

            //GameObject.Find("destroyer").GetComponentInChildren<Text>().text = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", myList[0].Key, myList[0].Value, myList[1].Key, myList[1].Value, myList[2].Key, myList[2].Value, myList[3].Key, myList[3].Value);
            QuadCreate.CreateQuad(myList[0].Key, myList[0].Value, myList[1].Key, myList[1].Value, myList[2].Key, myList[2].Value, myList[3].Key, myList[3].Value, z);
            j += 8;
        }
    }

    public static void SetZ (float slider_z)
    {
        z = slider_z;

    }

   
}


