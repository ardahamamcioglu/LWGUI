using UnityEditor;
using UnityEngine;
using System.Linq;

namespace LWGUI {
    public class ScaleOffsetDrawer : SubDrawer {

        private string _basePropName = "";

        public ScaleOffsetDrawer() { }

        public ScaleOffsetDrawer(string group) : this(group,"") { }

        public ScaleOffsetDrawer(string group, string basePropName) {
            this.group = group;
            this._basePropName = basePropName;
        }

        //protected override float GetVisibleHeight() { return 64f; }

        protected override bool IsMatchPropType() { return prop.type == MaterialProperty.PropType.Vector; }

        public override void DrawProp(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            var rect = position; //EditorGUILayout.GetControlRect();
            /*
            if(prop.name == "_BaseMap") {
                materialEditor.TextureScaleOffsetProperty(prop);
                Vector4 scaleOffset = prop.textureScaleAndOffset;
                target.mainTextureScale = new Vector2(scaleOffset.x, scaleOffset.y);
                target.mainTextureOffset = new Vector2(scaleOffset.z, scaleOffset.w);
            }
            */
            Material target = editor.target as Material;
            MaterialProperty[] props = Helper.GetProperties(editor);
            MaterialProperty mainTexProp = props.Where(p => p.name== _basePropName).First();
            MaterialProperty bumpMapProp = props.Where(p => p.flags == MaterialProperty.PropFlags.Normal).First();
            
            if(mainTexProp != null) {
                editor.TextureScaleOffsetProperty(mainTexProp);
                //editor.VectorProperty(prop, prop.displayName);
                //Vector4 scaleOffset = mainTexProp.textureScaleAndOffset;
                Vector4 scaleOffset = mainTexProp.textureScaleAndOffset;
                target.SetTextureScale(mainTexProp.name,new Vector2(scaleOffset.x, scaleOffset.y));
                target.SetTextureOffset(mainTexProp.name, new Vector2(scaleOffset.z, scaleOffset.w));
                
                target.SetTextureScale("_MainTex", new Vector2(scaleOffset.x, scaleOffset.y));
                target.SetTextureOffset("_MainTex", new Vector2(scaleOffset.z, scaleOffset.w));

                //Debug.Log(target.GetTextureScale("_BaseColorScale"));
                if(bumpMapProp != null) {
                    target.SetTextureScale(bumpMapProp.name, new Vector2(scaleOffset.x, scaleOffset.y));
                    target.SetTextureOffset(bumpMapProp.name, new Vector2(scaleOffset.z, scaleOffset.w));
                    bumpMapProp.textureScaleAndOffset = scaleOffset;
                }
            }

            
            var revertButtonRect = RevertableHelper.GetRevertButtonRect(prop, position, true);
            if(RevertableHelper.RevertButton(revertButtonRect, prop, editor)) {
                RevertableHelper.SetPropertyToDefault(prop);
            }
            
            EditorGUI.showMixedValue = false;
        }
    }
}
