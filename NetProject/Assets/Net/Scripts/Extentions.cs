using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

namespace Net
{
    public static class Debugger
    {
        private static Text _console;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnStart()
        {
            _console = GameObject.FindObjectsOfType<Text>().FirstOrDefault(t => t.name == "Console");

#if UNITY_EDITOR
            Debug.Log("console not found!");
#endif
        }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR            
            _console.text += message;
#endif
        }

        public static byte[] SerializePlayerData(object data)
        {
            var player = (PlayerData)data;

            var array = new List<byte>();

            array.AddRange(BitConverter.GetBytes(player.posX));
            array.AddRange(BitConverter.GetBytes(player.posZ));
            array.AddRange(BitConverter.GetBytes(player.rotY));
            array.AddRange(BitConverter.GetBytes(player.hp));
            foreach(var bul in player.bullets)
            {
                var trans = BitConverter.GetBytes(bul[0]);
                array.AddRange(trans);
                trans = BitConverter.GetBytes(bul[1]);
                array.AddRange(trans);
                trans = BitConverter.GetBytes(bul[2]);
                array.AddRange(trans);
            }

            return array.ToArray();
        }

        public static object DeserializePlayerData(byte[] data)
        {
            List<float[]> buls = new List<float[]>();

            for (int i = 16; i < data.Length; i += 12)
            {
                buls.Add(new float[] {BitConverter.ToSingle(data, i), BitConverter.ToSingle(data, i+4),
                    BitConverter.ToSingle(data, i+8)});
            }
            return new PlayerData
            {
                posX = BitConverter.ToSingle(data, 0),
                posZ = BitConverter.ToSingle(data, 4),
                rotY = BitConverter.ToSingle(data, 8),
                hp = BitConverter.ToSingle(data, 12),
                bullets = buls
            };
        }

        /*
        public static byte[] SerializeBullet(object data)
        {
            var bullet = (GameObject)data;

            var array = new List<byte>();

            array.AddRange(BitConverter.GetBytes(bullet.transform.position.x));
            array.AddRange(BitConverter.GetBytes(bullet.transform.position.y));
        }
        */

    }

    public struct PlayerData
    {
        public float posX;
        public float posZ;
        public float rotY;
        public float hp;
        public List<float[]> bullets;

        public static PlayerData Create(PlayerController player)
        {
            return new PlayerData
            {
                posX = player.transform.position.x,
                posZ = player.transform.position.z,
                rotY = player.transform.eulerAngles.y,
                hp = player.Health,
                bullets = player.GetBulletsPool()
            };
        }
            
        public void Set(PlayerController player)
        {
            var vector = player.transform.position;
            vector.x = posX;
            vector.z = posZ;
            player.transform.position = vector;

            vector = player.transform.eulerAngles;
            vector.y = rotY;
            player.transform.eulerAngles = vector;

            player.SetHealth(hp);
            player.SetBullets(bullets);

        }
    }

}
