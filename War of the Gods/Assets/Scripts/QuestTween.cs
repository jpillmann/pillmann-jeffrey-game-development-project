using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class QuestTween : MonoBehaviour
    {
        private void Start()
        {
            transform.localScale = Vector2.zero;
        }

        public void Open()
        {
            StartCoroutine(HandleQuestWindow(3, true));
        }

        public void Close()
        {
            transform.LeanScale(Vector2.zero, 1).setEaseInBack();
            StartCoroutine(HandleQuestWindow(1, false));
        }

        private IEnumerator HandleQuestWindow(float duration, bool flag)
        {
            if (flag)
            {
                yield return new WaitForSeconds(duration);
                transform.LeanScale(Vector2.one, 0.8f);
            }
            else
            {
                yield return new WaitForSeconds(duration);
                gameObject.SetActive(false);
            }
        }
    }
}
