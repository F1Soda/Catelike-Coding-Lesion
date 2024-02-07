using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Packs.Scene1.UI
{
    public class ButtonHandler : MonoBehaviour
    {
        
        [SerializeField] private Color clickedColor; // Цвет для кнопки после нажатия
        [SerializeField] private Color initialColor; // Начальный цвет кнопки
        private Button previousButton; // Предыдущая нажатая кнопка
        [SerializeField] private List<Button> buttons;

        private void Start()
        {
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => SelectButton(button));
                button.image.color = initialColor;
            }
        }

        private void SelectButton(Button clickedButton)
        {
            if (previousButton is not null)
                previousButton.image.color = initialColor;
            clickedButton.image.color = clickedColor;
            previousButton = clickedButton;
        }

        public void SelectButton(int index)
        {
            if(buttons is null)
                return;
            previousButton = buttons[index];
            previousButton.image.color = clickedColor;
        }
    }
}