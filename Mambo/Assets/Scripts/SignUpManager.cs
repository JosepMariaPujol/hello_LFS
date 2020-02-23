using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class SignUpManager : MonoBehaviour
{
    private GameObject c1, c2, c22, c3, gameObjectFemaleToggle, gameObjectMaleToggle;
    private GameObject gameObjectDayDropdown, gameObjectMonthDropdown, gameObjectYearDropdown;
    private TMP_InputField if1, if2, if22;
    private TMP_Text te;
    private Toggle femaleToggle, maleToggle;
    private Dropdown dayDropdown, monthDropdown, yearDropdown;
    //private string url = "http://localhost:5000/msgs/api/v1.0/users";
    private string url = "http://pdm-missatges.herokuapp.com/msgs/api/v1.0/users";

    private bool male = false;
    private bool female = false;

    void Start()
    {
        c1 = GameObject.Find("UserInputField");
        if1 = c1.GetComponent<TMP_InputField>();

        c2 = GameObject.Find("PasswordInputField");
        if2 = c2.GetComponent<TMP_InputField>();

        c22 = GameObject.Find("PasswordRepeatInputField");
        if22 = c22.GetComponent<TMP_InputField>();

        c3 = GameObject.Find("InfoText");
        te = c3.GetComponent<TMP_Text>();

        gameObjectFemaleToggle = GameObject.Find("FemaleToggle");
        femaleToggle = gameObjectFemaleToggle.GetComponent<Toggle>();

        gameObjectMaleToggle = GameObject.Find("MaleToggle");
        maleToggle = gameObjectMaleToggle.GetComponent<Toggle>();

        gameObjectDayDropdown = GameObject.Find("DayDropdown");
        dayDropdown = gameObjectDayDropdown.GetComponent<Dropdown>();

        gameObjectMonthDropdown = GameObject.Find("MonthDropdown");
        monthDropdown = gameObjectMonthDropdown.GetComponent<Dropdown>();

        gameObjectYearDropdown = GameObject.Find("YearDropdown");
        yearDropdown = gameObjectYearDropdown.GetComponent<Dropdown>();
    }

    private void Update()
    {
        CheckToggle();

        CheckSignUp();

        /*
        Debug.Log(dayDropdown.options[dayDropdown.value].text);
        Debug.Log(monthDropdown.options[monthDropdown.value].text);
        Debug.Log(yearDropdown.options[yearDropdown.value].text);
        */
    }

    private void CheckToggle()
    {
        if (femaleToggle.isOn && !male)
        {
            female = true;
            male = false;
            if (maleToggle.isOn && female)
            {
                male = true;
                female = false;
                femaleToggle.isOn = false;
            }
        }
        else if (maleToggle.isOn && !female)
        {
            female = false;
            male = true;
            if (femaleToggle.isOn && male)
            {
                male = false;
                female = true;
                maleToggle.isOn = false;
            }
        }
        else if (!femaleToggle.isOn && !male && female)
        {
            femaleToggle.isOn = true;
        }
        else if (!maleToggle.isOn && male && !female)
        {
            maleToggle.isOn = true;
        }
    }

    private bool CheckSignUp()
    {
        if (dayDropdown.value == 1 || monthDropdown.value == 1 || yearDropdown.value == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator CrearUsuari()
    {
        if (if2.text != if22.text)
        {
            Debug.Log("Error: password");
            te.text = "Error: password";
        }
        else if ((dayDropdown.value == 0 || monthDropdown.value == 0 || yearDropdown.value == 0))
        {
            Debug.Log("Error: missing birthday");
            te.text = "Error: missing birthday";
        }
        else if ((!femaleToggle.isOn && !maleToggle.isOn))
        {
            Debug.Log("Error: missing gender");
            te.text = "Error: missing gender";
        }
        else
        {
            WWWForm form = new WWWForm();

            form.AddField("nombre", if1.text);
            form.AddField("password", if2.text);
            form.AddField("day", dayDropdown.value);
            form.AddField("month", monthDropdown.value);
            form.AddField("year", yearDropdown.value);
            form.AddField("female", femaleToggle.isOn.ToString());
            form.AddField("male", maleToggle.isOn.ToString());

            WWW www = new WWW(url, form);

            Debug.Log(form.data);
            Debug.Log(url);
            Debug.Log(www);

            yield return www;

            // Print the error to the console
            if (www.error != null)
            {
                Debug.Log("request error: " + www.error);
                te.text = "Error: " + www.error;
            }
            else
            {
                Debug.Log("request ok:" + www.text);
                PlayerPrefs.SetString("usuario", if1.text);
                PlayerPrefs.SetString("password", if2.text);
                SceneManager.LoadScene("ChatScene");
            }

        }
    }

    public void ClicCrear()
    {
        StartCoroutine(CrearUsuari());
    }

    public void ClicCambiar()
    {
        SceneManager.LoadScene("UserScene");
    }
}
