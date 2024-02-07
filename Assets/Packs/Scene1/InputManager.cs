using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.instance.LoadNextScene();
            }
        }
        
    }
}