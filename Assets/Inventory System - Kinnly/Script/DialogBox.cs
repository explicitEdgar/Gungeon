using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kinnly
{
    public class DialogBox : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] RectTransform rectTransform;
        [SerializeField] TMP_Text dialogBoxText;

        public static DialogBox instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(string _text, float time)
        {
            CancelInvoke();
            gameObject.SetActive(true);
            dialogBoxText.text = _text;
            Invoke("Close", time);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
