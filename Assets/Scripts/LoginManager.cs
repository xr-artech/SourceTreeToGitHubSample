using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour {

    public InputField loginEmail,loginPassword;
    public InputField signupEmail, signupPassword, signupPWconfirm, signupName, signupPhone;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLoginClick() {
        StartCoroutine(LoginHandler());
    }

    IEnumerator LoginHandler() {
        /*string uri = "https://wixart.ddns.net/fourm/restful/login";

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            LoginTemplate template = new LoginTemplate
            {
                email = loginEmail.text,
                password = loginPassword.text
            };

             string json = JsonUtility.ToJson(template);

            // C# 6.0 
            string json =
            $"{{\"password\": \"{loginPassword.text}\",\"email\": \"{loginEmail.text}\"}}";
            // C# 4.0
            json = string.Format("{{\"password\":\"{0}\",\"email\":\"{1}\"}}", loginPassword.text, loginEmail.text);

            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();*/

        LoginTemplate template = new LoginTemplate
        {
            email = loginEmail.text,
            password = loginPassword.text
        };
       

        using (var request = API.Login(template))
        {
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                Debug.LogWarning($"{request.responseCode}:{request.downloadHandler.text}");
                yield break;
            }

            var response =
            JsonUtility.FromJson<LoginResponseTemplate>(request.downloadHandler.text);
            Debug.Log("Token: " + response.token);
        }
      
    }

    public void OnSignUpClick() {
        StartCoroutine(SignUpHandle());
    }

    IEnumerator SignUpHandle()
    {
        if (!Regex.IsMatch(signupEmail.text, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$"))
        { 
            Debug.LogWarning("Email 格式錯誤");
            yield break;
        }

        if (!Regex.IsMatch(signupPassword.text, @"^\w(\w|[!@#$%^&*]){3,}$"))
        {
            Debug.LogWarning("密碼格式錯誤");
            yield break;
        }

        if (signupPassword.text != signupPWconfirm.text)
        {
            Debug.LogWarning("密碼不符");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(signupName.text))
        {
            Debug.LogWarning("請輸入正確使用者ID");
            yield break;
        }
        if (!Regex.IsMatch(signupPhone.text, @"^0[\d]{8,9}$"))
        {
            Debug.LogWarning("電話號碼格式錯誤");
            yield break;
        }
        

        UserTemplate template = new UserTemplate
        {
            email = signupEmail.text,
            password = signupPassword.text,
            name = signupName.text,
            phone = signupPhone.text
        };

        using (var request = API.SignUp(template))
        {
            yield return request.SendWebRequest();

            if (request.responseCode != 201)
            {
                switch (request.responseCode)
                {
                    case 409:
                        Debug.LogWarning("Email 已使用");
                        break;
                    default:
                        Debug.LogWarning($"{request.responseCode}:{request.downloadHandler.text}");
                        break;
                }
                yield break;
            }
            loginEmail.text = signupEmail.text;
            loginPassword.text = signupPassword.text;

            StartCoroutine(LoginHandler());
        }

    }

}

[System.Serializable]
public struct LoginResponseTemplate {
    public int user_id;
    public string token;
}

[System.Serializable]
public struct LoginTemplate {
    public string email;
    public string password;
}

[System.Serializable]
public struct UserTemplate {
    public int id;
    public string name;
    public string email;
    public string password;
    public string phone;
    public string created_at;
}
