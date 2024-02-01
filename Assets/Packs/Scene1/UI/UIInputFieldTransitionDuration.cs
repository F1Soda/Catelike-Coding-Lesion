using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Packs.Scene1.UI
{
    public class UIInputFieldTransitionDuration : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private TMP_InputField inputField;

        [SerializeField] private UIHandler uiHandler;
        public event Action<int> OnSettingInputField;

        private void Start()
        {
            inputField.SetTextWithoutNotify(uiHandler.GetCurrentTransitionDuration()
                .ToString(CultureInfo.InvariantCulture));
        }

        public void OnSetValue(string value)
        {
            if (int.TryParse(value, out var result))
                OnSettingInputField?.Invoke(result);
        }
    }
}