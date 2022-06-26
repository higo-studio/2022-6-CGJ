using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Menu : MonoBehaviour
{
    public ScriptableRendererFeature feature;

    private void Start()
    {
        feature.SetActive(true);
    }

    public void onClickStart()
    {
        feature.SetActive(false);
        SceneManager.LoadScene("Game");       
    }
}