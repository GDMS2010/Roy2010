using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    GameObject ventureOutBox = null;
    [SerializeField]
    GameObject ventureButton = null;

    // Start is called before the first frame update
    void Start()
    {
        ventureOutBox.SetActive(false);
        ventureButton.SetActive(true);
    }

    public void ButtonManager(UnityEngine.UI.Button _button)
    {
        switch (_button.name)
        {
            default:
                break;

            case "Refugee Camp Button":
                LoadSceneString("RefugeeCamp");
                break;

            case "SuperMarket Button":
                //LoadSceneString("");
                break;

            case "Downtown Button":
                //LoadSceneString("");
                break;

            case "BackButton":
                ventureOutBox.SetActive(false);
                ventureButton.SetActive(true);
                break;

            case "Venture Button":
                ventureOutBox.SetActive(true);
                ventureButton.SetActive(false);
                break;
        }
    }

    void LoadSceneString(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}
