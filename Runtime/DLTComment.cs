// Assets/DolceTools/Runtime/DLTComment.cs
using UnityEngine;

namespace DolceTools
{
    [DisallowMultipleComponent]
    [AddComponentMenu("DolceTools/DLT Comment")]
    public class DLTComment : MonoBehaviour
    {
        #pragma warning disable 414
        [SerializeField, TextArea(2, 10)]
        private string commentText = "";
        #pragma warning restore 414
    }
}