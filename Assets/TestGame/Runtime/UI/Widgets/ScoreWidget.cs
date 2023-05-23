using TMPro;
using UnityEngine;

namespace TestGame
{
    public class ScoreWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_textCount;

        private int _count;
        private float _hp;
        private float _killed;

        public void SetCount(int count)
        {
            if (_count == count) return;
            _count = count;
            Render();
        }

        public void SetAllHP(float hp)
        {
            if (_hp == hp) return;
            _hp = hp;
            Render();
        }

        public void SetKill()
        {
            _killed++;
            Render();
        }

        private void Render()
        {
            m_textCount.text = $"Живых монстров: {_count}\r\n"
                +$"Всего у них HP: {(int)(_hp*100.0f)}\r\n"
                +$"Всего Убито: {_killed}";
        }
    }
}