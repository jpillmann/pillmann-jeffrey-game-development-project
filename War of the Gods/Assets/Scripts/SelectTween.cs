using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class SelectTween : MonoBehaviour
    {
        private void Start()
        {
            transform.localScale = Vector2.zero;
        }

        public void Open()
        {
            StartCoroutine(HandleSelectWindow(0.5f, true));
        }

        public void Close()
        {
            StartCoroutine(HandleSelectWindow(0.5f, false));
        }

        private IEnumerator HandleSelectWindow(float duration, bool flag)
        {
            if (flag)
            {
                transform.LeanScale(Vector2.one, 0.5f).setEaseOutQuart();
            }
            else
            {
                transform.LeanScale(Vector2.zero, 0.5f).setEaseOutQuart();
                yield return new WaitForSeconds(duration);
                gameObject.SetActive(false);
            }
        }
    }
}
