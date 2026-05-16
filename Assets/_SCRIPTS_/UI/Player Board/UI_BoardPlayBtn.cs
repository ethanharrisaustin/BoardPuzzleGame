using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class UI_BoardPlayBtn : MonoBehaviour
    {
        [SerializeField] Image playBtnOn, playBtnOff;

        // Update is called once per frame
        void Update()
        {
            playBtnOn.enabled = !Board.inPlayMode;
            playBtnOff.enabled = Board.inPlayMode;
        }
    }
}