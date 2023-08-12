#if UNITY_EDITOR
using System.Reflection;

using UnityEditor;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CustomEditor(typeof(CommandSO), true)]
    public class CommandEditor : Editor
    {
        // a futuro para modificar el help response
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CommandSO command = target as CommandSO;

            MethodInfo info = command.GetType().GetMethod(nameof(command.TriggerHelpResponse));
            bool isOverriden = IsOverridden(info);
        }

        public bool IsOverridden(MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }
    }
}
#endif