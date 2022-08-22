using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static class UiUtils
    {
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), Quaternion rotation = default(Quaternion), int fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0) {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, rotation, fontSize, color, textAnchor, textAlignment, sortingOrder);
        }
    
        public static TextMesh CreateWorldText (Transform parent, string text, Vector3 localPosition, Quaternion rotation, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            var gameObject = new GameObject("World_Text", typeof(TextMesh));
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.rotation = rotation;

            var textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }

        public static GameObject CreateEmptyGameObject(string name, Transform parent, Vector3 localPosition, Quaternion rotation)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.rotation = rotation;

            return gameObject;
        }
    }
}
