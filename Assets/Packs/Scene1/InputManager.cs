using UnityEngine;

namespace Packs.Scene1
{
    public class InputManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UIHandler uiHandler;
         
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                uiHandler.TurnOffOnMenu();
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
    }
}