using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ImageView : MonoBehaviour {

    public Slider progessBar;
    public RawImage image;
    public string imageName;
    public UnityWebRequest request { get; private set; }


	// Use this for initialization
	void Start () {
        if (string.IsNullOrEmpty(imageName))
        {
            Destroy(gameObject);
            return;
        }

        string uri = "https://wixart.ddns.net/fourm/img/" + imageName;
        request = UnityWebRequestTexture.GetTexture(uri);
        request.SendWebRequest();

	}
	
	// Update is called once per frame
	void Update () {
        if (request == null) return;
        progessBar.value = request.downloadProgress;
        if (request.isDone)
        {
            image.texture =  DownloadHandlerTexture.GetContent(request);
            image.gameObject.SetActive(true);
            progessBar.gameObject.SetActive(false);

            Vector2 imgSize = new Vector2(image.texture.width, image.texture.height);
            Vector2 frame = new Vector2(Screen.width, Screen.height);

            float rateW = imgSize.x / frame.x;
            float rateH = imgSize.y / frame.y;
            float rate = Mathf.Max(rateW, rateH, 1f);
            image.rectTransform.sizeDelta = imgSize / rate;

            //image.SetNativeSize();

            request.Dispose();
            request = null;
        }
        

	}
    public void OnClose() {
        Destroy(gameObject);
    }


}
