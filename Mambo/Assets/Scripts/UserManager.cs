using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    private GameObject c1, c2, c22, c3;
    private InputField if1, if2, if22;
    private Text te;
    //private string url = "http://localhost:5000/msgs/api/v1.0/users";
    private string url = "http://pdm-missatges.herokuapp.com/msgs/api/v1.0/users";

    void Start()
    {
        c1 = GameObject.Find("UserInputField");
        if1 = c1.GetComponent<InputField>();

        c2 = GameObject.Find("PasswordInputField");
        if2 = c2.GetComponent<InputField>();

        c3 = GameObject.Find("InfoText");
        te = c3.GetComponent<Text>();
    }

    private IEnumerator EntrarUsuari()
    {
        // www.EscapeURL i www.UnEscapeURL
        WWWForm form = new WWWForm();
        var headers = form.headers;
        headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(if1.text + ":" + if2.text));
        WWW www = new WWW(url + "?nombre=" + if1.text, null, headers);
        yield return www;

        // Print the error to the console
        if (www.error != null)
        {
            Debug.Log("request error: " + www.error);
            te.text = "Error: " + www.error;
            //te.text = "INCORRECT";
        }
        else
        {
            Debug.Log("request ok:" + www.text);
            PlayerPrefs.SetString("usuario", if1.text);
            PlayerPrefs.SetString("password", if2.text);
            SceneManager.LoadScene("ChatScene");
        }
    }

    public void ClicEntrar()
    {
        StartCoroutine(EntrarUsuari());
    }

    public void ClicCambiar()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}
