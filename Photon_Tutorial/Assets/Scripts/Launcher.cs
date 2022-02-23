using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{


    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("방당 최대 플레이어 수.방이 가득 차면 새로운 플레이어가 참여할 수 없으므로 새 방이 생성됩니다")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

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
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN 기초 튜토리얼 / 런처: PUN에서 OnConnectedToMaster()를 호출했습니다.");
            PhotonNetwork.JoinRandomRoom();
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN 기본 튜토리얼 / 런처: OnDisconnected()가 이유 {0}와 함께 PUN에서 호출되었습니다.", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN 기초 튜토리얼 / 런처:OnJoinRandomFailed()가 PUN에 의해 ​​호출되었습니다.사용 가능한 임의의 방이 없으므로 하나 만듭니다.\n호출: PhotonNetwork.CreateRoom");

            // #Critical: 우리는 임의의 방에 참여하는 데 실패했습니다. 아무도 없거나 모두 찼을 수 있습니다. 걱정 마세요. 새 방을 만듭니다.
                        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN 기초 튜토리얼 / 런처: PUN에 의해 ​​호출된 OnJoinedRoom().이제 이 클라이언트는 방에 있습니다.");
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
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
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