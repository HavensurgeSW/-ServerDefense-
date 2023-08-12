using System.Collections.Generic;

using UnityEngine;

namespace ServerDefense.Gameplay.Gameplay.Modules.Terminal
{
    [CreateAssetMenu(fileName = "response_", menuName = "ScriptableObjects/Terminal/Response")]
    public class TerminalResponseSO : ScriptableObject
    {
        [SerializeField] private List<string> response = null;

        public List<string> RESPONSE { get => response; }

        public static TerminalResponseSO CreateInstance(List<string> response)
        {
            TerminalResponseSO so = CreateInstance<TerminalResponseSO>();
            so.response = response;
            return so;
        }

        public static void Destroy(TerminalResponseSO so)
        {
            ScriptableObject.Destroy(so);
        }
    }
}