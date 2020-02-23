using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Msgs
{
    public Msg[] data;
}

[Serializable]
public class Msg
{
    public string autor, texto, tiempo;
}

public class ChatManager : MonoBehaviour
{
    private GameObject c1, c2, c3, c4, c5, c6;
    private InputField if4, if5, if6;

    private TMP_Text t, userName;
    public TMP_InputField if1;



    //private string url = "http://localhost:5000/msgs/api/v1.0/messages";
    private string url = "http://pdm-missatges.herokuapp.com/msgs/api/v1.0/messages";

    void Start()
    {
        c1 = GameObject.Find("MessageInputField");
        if1 = c1.GetComponent<TMP_InputField>();
        c2 = GameObject.Find("ChatText");
        t = c2.GetComponent<TMP_Text>();
        c3 = GameObject.Find("UserText");
        userName = c3.GetComponent<TMP_Text>();

        c4 = GameObject.Find("CurrentPasswordInputField");
        if4 = c4.GetComponent<InputField>();
        c5 = GameObject.Find("NewPasswordInputField");
        if5 = c5.GetComponent<InputField>();
        c6 = GameObject.Find("RepeatPasswordInputField");
        if6 = c6.GetComponent<InputField>();

        ProfileName();
        StartCoroutine(missatges());
    }
    
    private void ProfileName()
    {       
        string user = PlayerPrefs.GetString("usuario");
        userName.text = user;
    }

    public void LogOut()
    {
        SceneManager.LoadScene("UserScene");
    }

    private IEnumerator missatges()
    {
        WWWForm form = new WWWForm();
        var headers = form.headers;
        string credencials = PlayerPrefs.GetString("usuario") + ":" + PlayerPrefs.GetString("password");
        headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credencials));
        WWW www = new WWW(url, null, headers);
        yield return www;

        // Print the error to the console
        if (www.error != null)
        {
            Debug.Log("request error:  " + www.error);
        }
        else
        {
            // Debug.Log("request ok:" + www.text);
            Msgs msgs = JsonUtility.FromJson<Msgs>(www.text);
            t.text = "";
            foreach (Msg m in msgs.data)
            {
                t.text += "<b>" + m.autor + "</b>" + "\t" + "<i>" + "<size=25>" + m.tiempo + "</size>" + "</i>" + "\n" + m.texto + "\n" + "\n";
            }
        }
    }

    private IEnumerator crearMissatge()
    {
        WWWForm form = new WWWForm();
        form.AddField("texto", if1.text);
        var headers = form.headers;
        var rawData = form.data;
        string credencials = PlayerPrefs.GetString("usuario") + ":" + PlayerPrefs.GetString("password");
        headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credencials));
        WWW www = new WWW(url, rawData, headers);
        yield return www;

        // Print the error to the console
        if (www.error != null)
        {
            Debug.Log("request error: " + www.error);
        }
        else
        {
            Debug.Log("request ok:" + www.text);
            if1.text = "";
            StartCoroutine(missatges());
        }
    }

    public void clicAfegir()
    {
        StartCoroutine(crearMissatge());
    }

    private IEnumerator netejar()
    {
        WWWForm form = new WWWForm();
        var headers = form.headers;
        string credencials = PlayerPrefs.GetString("usuario") + ":" + PlayerPrefs.GetString("password");
        headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credencials));
        WWW www = new WWW(url, null, headers);
        yield return www;

        /// Print the error to the console
        if (www.error != null)
        {
            Debug.Log("request error: " + www.error);
        }
        else
        {
            // Debug.Log("request ok:" + www.text);
            t.text = "";
        }
    }

    private IEnumerator ContrasenyaUsuari()
    {
        yield return null;

        /*

        if(if4.text == PlayerPrefs.GetString("password"))
        {
            if(if5.text == if6.text)
            {
                if(if4.text != if5.text)
                {
                    Debug.Log("OLD PASSWORD" + PlayerPrefs.GetString("password"));

                    /*
                        PlayerPrefs.SetString("password", if5.text);
                        Debug.Log("NEW PASSWORD" + PlayerPrefs.GetString("password"));

                        if4.text = "";
                        if5.text = "";
                        if6.text = "";
                    

                    WWWForm form = new WWWForm();
                    Debug.Log(userName.text);
                    form.AddField("nombre", userName.text + "a");
                    form.AddField("password", if5.text);


                    WWW www = new WWW(url, form);

                    yield return www;

                    // Print the error to the console
                    if (www.error != null)
                    {
                        Debug.Log("request error: " + www.error);
                    }
                    else
                    {
                        PlayerPrefs.SetString("password", if5.text);
                        Debug.Log("NEW PASSWORD" + PlayerPrefs.GetString("password"));

                        if4.text = "";
                        if5.text = "";
                        if6.text = "";
                    }

                }
                else
                {
                    Debug.Log("ERROR Same Password");
                }
            }
            else
            {
                Debug.Log("ERROR Wrong New password");
            }
        }
        else
        {
            Debug.Log("ERROR Wrong password");
        }
    */
    }

    public void clicNetejar()
    {
        StartCoroutine(netejar());
    }

    public void ClicContrasenya()
    {
        StartCoroutine(ContrasenyaUsuari());
    }
}