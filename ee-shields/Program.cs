using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NodaTime;
using PlayerIOClient;

namespace Lobbydb
{
    internal static class Program
    {
        private const string GameId = "everybody-edits-su9rn58o40itdbnw69plyw";

        private static Client _globalClient;

        public static List<RoomInfo> roomsDone = new List<RoomInfo>();
        private static readonly int DOWNLOAD_INTERVAL_SECONDS = 5*1000;

        private static CancellationTokenSource cancelToken = new CancellationTokenSource();
        private static void Main()
        {

            PlayerIO.QuickConnect.SimpleConnect(GameId, "guest", "guest", null, delegate (Client client)
            {
                _globalClient = client;
                    Task t = null;
                    if (_globalClient.ConnectUserId != null)
                    {
                        var rooms = DownloadLobby();
                        //Console.WriteLine(JsonConvert.SerializeObject(rooms));
                    Environment.Exit(0);
                    }
                    
                
            });
            Thread.Sleep(Timeout.Infinite);
        }

        // ReSharper disable once InconsistentNaming
        private static void PrintPlayerIOError(PlayerIOError error)
        {
            Console.WriteLine("ERROR: [{0}] {1}", "unknownTime", error.Message);
        }

        private static List<RoomInfo> DownloadLobby()
        {
            ManualResetEvent waitHandle = new ManualResetEvent(false);
            List<RoomInfo> dataToWrite = new List<RoomInfo>();
            _globalClient.Multiplayer.ListRooms(null, null, 0, 0,
                delegate (PlayerIOClient.RoomInfo[] rooms)
                {
                    foreach (var room in rooms)
                    {
                        if (room.RoomType.StartsWith("Lobby"))
                        {
                            if (room.Id.StartsWith("simple") || room.Id.StartsWith("fb") || room.Id.StartsWith("kong"))
                            {
                                Console.WriteLine(room.Id);
                            }
                        }
                    }
                    waitHandle.Set();
                },
                PrintPlayerIOError);
            waitHandle.WaitOne();
            return dataToWrite;
        }
    }
}