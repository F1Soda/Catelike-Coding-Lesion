using System;
using System.Linq;
using Packs.Scene1;
using TMPro;
using UnityEngine;


[ExecuteAlways]
public class UIDropDownMenu : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private TMP_Dropdown _dropdown;
    [SerializeField] private UIHandler uiHandler;
    public event Action<ETypeFunction> OnSelectedDropdownOption;
    
    
    private void Start()
    {
        if (_dropdown.options.Count != FunctionLibrary.CountFunctions)
        {
            for (int i = 0; i < FunctionLibrary.CountFunctions; i++)
            {
                var nameOfFunc = ((ETypeFunction)i).ToString();
                if(_dropdown.options.All(item => item.text != nameOfFunc))
                    _dropdown.options.Add(new TMP_Dropdown.OptionData(nameOfFunc));
            }
        }
        _dropdown.SetValueWithoutNotify(uiHandler.GetCurrentIdFunction());

    }

    public void OnButtonPressed(int value)
    {
        OnSelectedDropdownOption?.Invoke((ETypeFunction)value);
    }
}
