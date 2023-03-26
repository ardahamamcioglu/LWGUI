using UnityEngine;
using UnityEditor;

namespace LWGUI {
    public class TexKeywordDrawer : SubDrawer {
        private string _extraPropName = "";
        private string _keyWord = "";

        private ChannelDrawer _channelDrawer = new ChannelDrawer("_");

        public TexKeywordDrawer() { }

        public TexKeywordDrawer(string group,string keyWord) : this(group,keyWord,"") { }

        public TexKeywordDrawer(string group,string keyWord, string extraPropName) {
            this.group = group;
            this._keyWord = keyWord;
            this._extraPropName = extraPropName;
        }

        protected override bool IsMatchPropType() { return prop.type == MaterialProperty.PropType.Texture; }

        public override void DrawProp(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            var rect = position; //EditorGUILayout.GetControlRect();
            string k = Helper.GetKeyWord(_keyWord, prop.name);

            bool hasTexture = prop.textureValue;
            Helper.SetShaderKeyWord(editor.targets, k, hasTexture);

            editor.TexturePropertyMiniThumbnail(rect, prop, label.text, label.tooltip);

            MaterialProperty extraProp = null;
            if(_extraPropName != "" && _extraPropName != "_")
                extraProp = LWGUI.FindProp(_extraPropName, props, true);

            if(extraProp != null && extraProp.type != MaterialProperty.PropType.Texture) {
                Rect extraPropRect = Rect.zero;
                if(extraProp.type == MaterialProperty.PropType.Range) {
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUIUtility.fieldWidth = RevertableHelper.fieldWidth - 12f;
                    extraPropRect = MaterialEditor.GetRectAfterLabelWidth(rect);
                }
                else
                    extraPropRect = MaterialEditor.GetRectAfterLabelWidth(rect);


                var i = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                if(extraProp.type == MaterialProperty.PropType.Vector) {
                    _channelDrawer.DrawProp(extraPropRect, extraProp, new GUIContent(""), editor);
                }
                else {
                    editor.ShaderProperty(extraPropRect, extraProp, "");
                }
                EditorGUI.indentLevel = i;


                var revertButtonRect = RevertableHelper.GetRevertButtonRect(extraProp, position, true);
                if(RevertableHelper.RevertButton(revertButtonRect, extraProp, editor)) {
                    RevertableHelper.SetPropertyToDefault(extraProp);
                }
            }

            if(GUIData.keyWord.ContainsKey(k)) {
                GUIData.keyWord[k] = hasTexture;
            }
            else {
                GUIData.keyWord.Add(k, hasTexture);
            }

            EditorGUI.showMixedValue = false;
        }
        /*
        public override void Apply(MaterialProperty prop) {
            base.Apply(prop);
            if(!prop.hasMixedValue && prop.type == MaterialProperty.PropType.Texture)
                Helper.SetShaderKeyWord(prop.targets, Helper.GetKeyWord(_keyWord, prop.name), prop.textureValue);
        }
        */
    }
}
