using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunchList : MonoBehaviour {

    [SerializeField]
    Bunch bunchPrefab;

	// Use this for initialization
	void Start () {
        StartCoroutine(GetBunchHandle());
    }
	    
    IEnumerator GetBunchHandle()
    {
        using (var request = API.GetBunchList())
        {
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                Debug.LogWarning($"{request.responseCode}:{request.downloadHandler.text}");
                yield break;
            }

            var jsonArray = JsonArry<BunchTemplate>.FromJson(request.downloadHandler.text);
            Transform content = GetComponentInChildren<UnityEngine.UI.ScrollRect>().content;

            foreach (var bunch in jsonArray.values)
            {
                Bunch instance = Instantiate(bunchPrefab,content);
                instance.SetupByJson(bunch);
            }
        }
    }

}

public struct JsonArry<T> {
    public T[] values;

    public static JsonArry<T> FromJson(string json)
    {
        json = "{\"values\":" + json + "}";
        return JsonUtility.FromJson<JsonArry<T>>(json);
    }
}