using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SampleRoom : MonoBehaviourPunCallbacks
{
    public Button button;       //参加ボタン
    public Text text;           //人数のテキスト
    public Text room_name;      //部屋の名前
    
    public string LoadSceneName;

    public string GroundName;
    public string TowerName;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnLoadedScene;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        RoomOptions options = new RoomOptions();

        options.MaxPlayers = 2;
        options.IsOpen = true;
        options.IsVisible = true;

        //ちょっと無理やり
        if (text.text.Equals("人数:0/2"))
        {
            text.text = "人数:1/2";
        }
        //
        else if (text.text.Equals("人数:1/2"))
        {
            text.text = "人数:2/2";
            options.IsVisible = false;
            button.interactable = false;
        }

        PhotonNetwork.JoinOrCreateRoom(room_name.text, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(LoadSceneName);
        //StartCoroutine(LoadScene(2.0f));
    }

    void SceneChange()
    {
        SceneManager.LoadScene(LoadSceneName);
    }

    private void OnLoadedScene(Scene my_scene,LoadSceneMode my_mode)
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        if (my_scene.name == LoadSceneName)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //床面
                //PhotonNetwork.Instantiate(GroundName, Vector3.zero, Quaternion.identity);
                //Debug.Log("Create_Floor");
                //タワー
                //PhotonNetwork.Instantiate(TowerName, Vector3.zero, Quaternion.identity);
                //Debug.Log("Create_Tower");
            }
        }
    }

    private IEnumerator LoadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(LoadSceneName);
    }
}
