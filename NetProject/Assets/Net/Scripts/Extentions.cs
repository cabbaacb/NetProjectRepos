using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
    }
}
