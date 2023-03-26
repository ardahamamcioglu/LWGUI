using UnityEngine;
using UnityEditor;

namespace LWGUI {
    public class EmissionDrawer : SubDrawer {

        public EmissionDrawer() { }

        public EmissionDrawer(string group) : this(group, "") { }

        public EmissionDrawer(string group, string extraPropName) {
            this.group = group;
        }

        protected override bool IsMatchPropType() { return prop.type == MaterialProperty.PropType.Texture; }

        public override void DrawProp(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            var rect = position; //EditorGUILayout.GetControlRect();

            bool hasTexture = prop.textureValue;
            Helper.SetShaderKeyWord(editor.targets, "_EMISSION", hasTexture);

            editor.TexturePropertyMiniThumbnail(rect, prop, label.text, label.tooltip);

            MaterialProperty extraProp = null;
            extraProp = LWGUI.FindProp("_EmissionColor", props, true);

            Rect extraPropRect = Rect.zero;
            extraPropRect = MaterialEditor.GetRectAfterLabelWidth(rect);


            var i = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            editor.ColorProperty(extraPropRect, extraProp, "");
            EditorGUI.indentLevel = i;

            var target = editor.target as Material;
            //MaterialGlobalIlluminationFlags GIFlag = (MaterialGlobalIlluminationFlags)EditorGUILayout.EnumFlagsField(target.globalIlluminationFlags);
            target.globalIlluminationFlags = (MaterialGlobalIlluminationFlags)EditorGUILayout.EnumPopup(target.globalIlluminationFlags);

            var revertButtonRect = RevertableHelper.GetRevertButtonRect(extraProp, position, true);
            if(RevertableHelper.RevertButton(revertButtonRect, extraProp, editor)) {
                RevertableHelper.SetPropertyToDefault(extraProp);
            }
            EditorGUI.showMixedValue = false;
        }
    }
}
