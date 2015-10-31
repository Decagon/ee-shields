using System;
using System.Collections.Generic;
using System.Threading;
using PlayerIOClient;

namespace eeshields
{
    internal static class Program
    {
        private const string GameId = "everybody-edits-su9rn58o40itdbnw69plyw";
        private static Client _globalClient;

        private static void Main()
        {
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            PlayerIO.QuickConnect.SimpleConnect(GameId, "guest", "guest", null, delegate (Client client)
            {
                _globalClient = client;
                DownloadLobby();
                waitHandle.Set();
            });

            waitHandle.WaitOne();
        }

        // ReSharper disable once InconsistentNaming
        private static void PrintPlayerIOError(PlayerIOError error)
        {
            Console.WriteLine("ERROR: [{0}] {1}", error.Source, error.Message);
        }

        private static void DownloadLobby()
        {
            ManualResetEvent waitHandle = new ManualResetEvent(false);
            _globalClient.Multiplayer.ListRooms(null, null, 0, 0,
                delegate (PlayerIOClient.RoomInfo[] rooms)
                {
                    foreach (var room in rooms)
                    {
                        if (room.RoomType.StartsWith("Lobby"))
                        {
                            if (room.Id.StartsWith("simple") || room.Id.StartsWith("fb") || room.Id.StartsWith("kong") || room.Id.StartsWith("mouse") || room.Id.StartsWith("armor"))
                            {
                                Console.WriteLine(room.Id);
                            }
                        }
                    }
                    waitHandle.Set();
                },
                PrintPlayerIOError);
            waitHandle.WaitOne();
        }
    }
}