using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;
using System.Xml.Linq;
using UnityEngine.UI;

public class PostList : MonoBehaviour {

    public int bunchId;
    [SerializeField]
    Post postPrefab;
        
	// Use this for initialization
	void Start () {
        StartCoroutine(GetPostList());
	}

    IEnumerator GetPostList()
    {
        using (var request = API.GetPostOfBunch(bunchId))
        {
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                switch (request.responseCode)
                {
                    case 404:
                        Debug.LogWarning("無此討論串凸");
                        break;
                    default:
                        Debug.LogWarning($"{request.responseCode}:{request.downloadHandler.text}");
                        break;
                }
                yield break;
            }

            var bunch = JsonUtility.FromJson<BunchTemplate>(request.downloadHandler.text);
            Transform content = GetComponentInChildren<ScrollRect>().content;

			foreach (var post in bunch.posts) {
				Post instance = Instantiate (postPrefab, content);
				instance.SetupByJson (post);
			}
        }
    }

    /* XML版本
    IEnumerator GetPostList()
    {
        
        string uri = "https://wixart.ddns.net/fourm/classical/api.php";
        WWWForm form = new WWWForm();
        form.AddField("method", "getPosts");

        using (var request = Post(uri, form))
        {
            yield return request.SendWebRequest();
            print("GG");
            if (request.isHttpError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                yield break;
            }

            var xml = XDocument.Parse(request.downloadHandler.text);

            if (xml.Root.Element("code").Value != "1")
            {
                Debug.LogWarning(xml.Root.Element("error").Value);
                yield break;
            }

            var posts = xml.Root.Element("posts");
            Transform content = GetComponentInChildren<ScrollRect>().content;

            foreach (var post in posts.Elements())
            {
                Post instance = Instantiate(postPrefab, content);
                instance.SetupByXml(post);
            }


        }

    }*/
}

    