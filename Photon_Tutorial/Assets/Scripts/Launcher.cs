using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{


    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion


        #region Private Fields

        string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // 이것은 마스터 클라이언트에서 PhotonNetwork.LoadLevel()을 사용할 수 있고 같은 방에 있는 모든 클라이언트가 자동으로 레벨을 동기화할 수 있도록 합니다.
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        ///초기화 단계에서 Unity에 의해 GameObject에서 호출되는 MonoBehaviour 메서드입니다.
        /// </summary>
        void Start()
        {
            Connect();
        }


        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        #endregion

        #region Public Methods



        /// <summary>
        /// 연결 프로세스를 시작합니다.
        /// - 이미 연결되어 있는 경우 임의의 방에 참여를 시도합니다.
        /// - 아직 연결되지 않은 경우 이 애플리케이션 인스턴스를 Photon Cloud Network에 연결합니다.
        /// </summary>
        public void Connect()
        {
            // 연결되었는지 여부를 확인하고 연결되어 있으면 가입하고 그렇지 않으면 서버에 연결을 시작합니다.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical 이 시점에서 랜덤 룸에 참여를 시도해야 합니다.실패하면 OnJoinRandomFailed()에서 알림을 받고 하나를 만듭니다.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, 우리는 가장 먼저 Photon Online Server에 연결해야 합니다.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

    }

}