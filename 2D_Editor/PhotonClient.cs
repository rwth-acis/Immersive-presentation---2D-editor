using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Photon.Realtime;
using System.Windows.Threading;
using System.Windows;
using System.Collections;
using System.Windows.Input;

namespace _2D_Editor
{
    public class PhotonClient : IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        private LoadBalancingClient loadBalancingClient;
        private string roomName;
        private bool joindedInRoom = false;
        public bool disconnectWanted = false;

        public PresentationHandling presHandling;
        private DispatcherTimer dispatcherTimer;

        //The names of the room porperties used in the photon rooms
        const string ANCHORID_PROPERTY_NAME = "azureAnchorId";
        const string STAGE_INDEX_PROPERTY_NAME = "stageIndex";

        public PhotonClient(PresentationHandling pPresHandling)
        {
            presHandling = pPresHandling;
            this.loadBalancingClient = new LoadBalancingClient();
            this.SubscribeToCallbacks();
        }

        ~PhotonClient()
        {
            this.Disconnect();
            this.UnsubscribeFromCallbacks();
        }

        private void SubscribeToCallbacks()
        {
            this.loadBalancingClient.AddCallbackTarget(this);
        }

        private void UnsubscribeFromCallbacks()
        {
            this.loadBalancingClient.RemoveCallbackTarget(this);
        }

        // call this to connect to Photon
        public void Connect(string pRoomName)
        {
            disconnectWanted = false;
            roomName = pRoomName;
            this.loadBalancingClient.AppId = "1f107329-3f6f-48fa-9218-23d6664a64ed";  // set your app id here
            this.loadBalancingClient.AppVersion = "1.0";  // set your app version here

            // "eu" is the European region's token
            if (!this.loadBalancingClient.ConnectToRegionMaster("eu")) // can return false for errors
            {
                Console.WriteLine("Error in connecting");
            }
            Console.WriteLine("No error when ask for connection");
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            presHandling.MakeCursorWait(true);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            loadBalancingClient.Service();
        }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            // client is now connected to Photon Master Server and ready to create or join rooms
            Console.WriteLine("Connected");
            MyCreateRoom(roomName);
        }

        public void OnConnected()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Joins or Creates a room with the given name
        /// </summary>
        /// <param name="roomName">The name of the room that will be joined or created</param>
        void MyCreateRoom(string roomName)
        {
            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomName = roomName;
            enterRoomParams.RoomOptions = new RoomOptions();
            enterRoomParams.RoomOptions.MaxPlayers = 10;
            this.loadBalancingClient.OpCreateRoom(enterRoomParams);
        }

        public void setStageIndex(int pStageIndex)
        {
            if (joindedInRoom)
            {
                loadBalancingClient.CurrentRoom.SetCustomProperties(new System.Collections.Hashtable()
                {
                    { STAGE_INDEX_PROPERTY_NAME, pStageIndex}
                 });
            }
        }

        /// <summary>
        /// Disconects the client from the photon application
        /// </summary>
        public void Disconnect()
        {
            disconnectWanted = true;
            if (this.loadBalancingClient.IsConnected)
            {
                this.loadBalancingClient.Disconnect();
            }
        }



        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            joindedInRoom = true;
            presHandling.MakeCursorWait(false);
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            if (!disconnectWanted)
            {
                //Reconnect
                Connect(roomName);
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            throw new NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new NotImplementedException();
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
        {
            MessageBox.Show(cause.ToString());
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            throw new NotImplementedException();
        }

        public void OnCreatedRoom()
        {
            //throw new NotImplementedException();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        public void OnLeftRoom()
        {
            //throw new NotImplementedException();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            //throw new NotImplementedException();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            //throw new NotImplementedException();
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(STAGE_INDEX_PROPERTY_NAME))
            {
                if (loadBalancingClient.CurrentRoom.CustomProperties.ContainsKey(STAGE_INDEX_PROPERTY_NAME))
                {
                    int stageIndex = (int) loadBalancingClient.CurrentRoom.CustomProperties[STAGE_INDEX_PROPERTY_NAME];
                    presHandling.updatePresStage(stageIndex);
                }
            }
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            throw new NotImplementedException();
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            throw new NotImplementedException();
        }
    }
}
