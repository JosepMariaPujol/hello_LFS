using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    private GameObject c1, c2;
    private InputField if1;
    private Text t;
    //private string url = "http://localhost:5000/msgs/api/v1.0/messages";
    private string url = "http://pdm-missatges.herokuapp.com/msgs/api/v1.0/messages";
    //private string url2 = "http://localhost:5000/msgs/api/v1.0/clear";
    private string url2 = "http://pdm-missatges.herokuapp.com/msgs/api/v1.0/clear";

    void Start()
    {
        c1 = GameObject.Find("MessageInputField");
        if1 = c1.GetComponent<InputField>();
        c2 = GameObject.Find("ChatText");
        t = c2.GetComponent<Text>();
        StartCoroutine(missatges());
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
                t.text += m.autor + "\t" + m.tiempo + "\n" + m.texto + "\n";
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
        WWW www = new WWW(url2, null, headers);
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

    public void clicNetejar()
    {
        StartCoroutine(netejar());
    }
}