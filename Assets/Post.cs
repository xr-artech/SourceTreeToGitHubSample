using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;

public class Post : MonoBehaviour {

    public Text headerlable, contentlable, timelabel;
    public Button imageButton;
    string imageName;

    [SerializeField]
    ImageView imageViewPrefab;

    public void SetupByXml(XElement xml)
    {
        string title = xml.Element("title").Value;
        string autor = xml.Element("author").Value;
        string id = xml.Attribute("id").Value;

        headerlable.text = $"{title}-{autor}<color=#00F>#{id}</color>";
        contentlable.text = xml.Element("content").Value;
        timelabel.text = xml.Element("created_at").Value;

        imageName = xml.Element("img_name").Value;
        imageButton.gameObject.SetActive(!string.IsNullOrEmpty(imageName));
    }

	public void SetupByJson(PostTemplate json){
		string title = json.title;
		string autor = json.author;
		string id = json.id.ToString();

		headerlable.text = $"{title}-{autor}<color=#00F>#{id}</color>";
		contentlable.text = json.content;
		timelabel.text = json.created_at;

		imageName = json.img_name;
		imageButton.gameObject.SetActive(!string.IsNullOrEmpty(imageName));
	}

    public void OnImageClick()
    {
        ImageView instance = Instantiate(imageViewPrefab);
        instance.imageName = this.imageName;
    }

}
