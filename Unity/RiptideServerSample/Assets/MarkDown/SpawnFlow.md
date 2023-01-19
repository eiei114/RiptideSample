## Server/NetworkManager
```
        private void NewPlayerConnected(object sender, ServerConnectedEventArgs e)
        {
            foreach (Player player in Player.List.Values)
            {
                if (player.Id != e.Client.Id)
                    player.SendSpawn(e.Client.Id);
            }
        }
```
## Server/SpawnManager
```
        public static void Spawn(ushort id, string username)
        {
            Player player = Instantiate(NetworkManager.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
            player.name = $"Player {id} ({(username == "" ? "Guest" : username)})";
            player.Id = id;
            player.Username = username;

            player.SendSpawn();
            List.Add(player.Id, player);
        }

        public void SendSpawn(ushort toClient)
        {
            NetworkManager.Singleton.Server.Send(GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer)), toClient);
        }
        /// <summary>Sends a player's info to all clients.</summary>
        private void SendSpawn()
        {
            NetworkManager.Singleton.Server.SendToAll(GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer)));
        }

        private Message GetSpawnData(Message message)
        {
            message.AddUShort(Id);
            message.AddString(Username);
            message.AddVector3(transform.position);
            return message;
        }
```
## Client/SpawnManager
```
        public static void Spawn(ushort id, string username, Vector3 position)
        {
            Player player;
            if (id == NetworkManager.Singleton.Client.Id)
                player = Instantiate(NetworkManager.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            else
                player = Instantiate(NetworkManager.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();

            player.name = $"Player {id} ({username})";
            player.id = id;
            player.username = username;
            list.Add(player.id, player);
        }

        #region Messages
        [MessageHandler((ushort)ServerToClientId.SpawnPlayer)]
        private static void SpawnPlayer(Message message)
        {
            Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
        }
```

