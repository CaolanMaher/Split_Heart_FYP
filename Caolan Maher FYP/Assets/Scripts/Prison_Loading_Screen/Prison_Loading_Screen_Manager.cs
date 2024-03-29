using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison_Loading_Screen_Manager : MonoBehaviour
{

    public GameObject player;

    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(true);
        Invoke("HidePlayer", 0.25f);
        //player.SetActive(false);
    }

    void HidePlayer()
    {
        player.SetActive(false);
    }

    public void FinishedLoading()
    {
        loadingScreen.SetActive(false);
        player.SetActive(true);
    }
}
