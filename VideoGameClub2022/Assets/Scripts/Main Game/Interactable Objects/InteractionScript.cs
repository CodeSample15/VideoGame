using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    /* plan to make this class a child in the future, so that way all interactable objects can inherit
     * the interactOption() function, and just override it
     */
    public void interactOption()
    {
        //Switches scenes when interacting with computer
        SceneManager.LoadScene("PongGame");
    }
}
