using TMPro;
using UnityEngine;

namespace TestGame
{
    public class PlayerHPWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_textCount;

        private float _hp;

        public void SetHP(float hp)
        {
            if (_hp == hp) return;
            _hp = hp;
            Render();
        }

        private void Render()
        {
            m_textCount.text = $"Жизнь: {(int)(_hp*100f)}%";
        }

    }
}