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
            // �̰��� ������ Ŭ���̾�Ʈ���� PhotonNetwork.LoadLevel()�� ����� �� �ְ� ���� �濡 �ִ� ��� Ŭ���̾�Ʈ�� �ڵ����� ������ ����ȭ�� �� �ֵ��� �մϴ�.
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        ///�ʱ�ȭ �ܰ迡�� Unity�� ���� GameObject���� ȣ��Ǵ� MonoBehaviour �޼����Դϴ�.
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
        /// ���� ���μ����� �����մϴ�.
        /// - �̹� ����Ǿ� �ִ� ��� ������ �濡 ������ �õ��մϴ�.
        /// - ���� ������� ���� ��� �� ���ø����̼� �ν��Ͻ��� Photon Cloud Network�� �����մϴ�.
        /// </summary>
        public void Connect()
        {
            // ����Ǿ����� ���θ� Ȯ���ϰ� ����Ǿ� ������ �����ϰ� �׷��� ������ ������ ������ �����մϴ�.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical �� �������� ���� �뿡 ������ �õ��ؾ� �մϴ�.�����ϸ� OnJoinRandomFailed()���� �˸��� �ް� �ϳ��� ����ϴ�.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, �츮�� ���� ���� Photon Online Server�� �����ؾ� �մϴ�.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

    }

}