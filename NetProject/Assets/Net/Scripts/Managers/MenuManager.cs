using UnityEngine;
using Photon.Pun;

namespace Net.Managers
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        

        public void OnCreateRoom_UnityEditor()
        {
            PhotonNetwork.CreateRoom("MyRoom", new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        }

        public void OnJoinRoom_UnityEditor()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void OnQuit_UnityEditor()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Application.Quit();
#endif

        }

        private void Start()
        {
#if UNITY_EDITOR
            PhotonNetwork.NickName = "playerNick1";
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            PhotonNetwork.NickName = "playerNick2";
#endif

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debugger.Log("Ready for connection");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("NetGameScene");
            Debugger.Log("Joined room");
        }


    }


    
}
