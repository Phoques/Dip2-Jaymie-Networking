using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode; // All the new Unity stuff is Unity. only, UnityEngine is going to become legacy.

//Networked games should all be private, having public accessors is a baaaaaad idea.
public class NetworkUIManager : MonoBehaviour
{

    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    private void Awake()
    {
        //_serverButton.onClick.AddListener(()< the perenthesis here need a method or function to be put in. Instead we are using lambda to say, where we are looking for a function, do this:
        //=> { NetworkManager.Singleton.StartServer(); }); This is INSTEAD of creating 3 methods, and attaching them to the buttons manually through the inspector.
        //The function would look like:

        //private void StartServer()
        //{
        //   NetworkManager.Singleton.StartServer()
        //}

        //This would then be attached to the 'server' button in the inspector. The way we are doing it below, we must Listeners, which is the code equiv of adding an event to the button.

        _serverButton.onClick.AddListener(() => { NetworkManager.Singleton.StartServer(); });
        _hostButton.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        _clientButton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });




    }

}
