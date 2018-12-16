using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {

    //The texture that's going to replace the current cursor 
    [HideInInspector]
    public Texture2D mainCursorTexture;
    [HideInInspector]
    public Texture2D attackCursorTexture;
    [HideInInspector]
    public Texture2D moveCursorTexture;

    //This variable flags whether the custom cursor is active or not 
    public bool ccEnabled = false;

    void Awake()
    {
        mainCursorTexture = Resources.Load<Texture2D>("Textures/UI/MainCursor");
        attackCursorTexture = Resources.Load<Texture2D>("Textures/UI/AttackCursor");
        moveCursorTexture = Resources.Load<Texture2D>("Textures/UI/MoveCursor");
    }

    void Start()
    {
        //Call the 'SetCustomCursor' (see below) with a delay of 2 seconds.  
        SetCustomCursor(mainCursorTexture);
    }

    void OnDisable()
    {
        //Resets the cursor to the default 
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //Set the _ccEnabled variable to false 
        this.ccEnabled = false;
    }

    public void SetCustomCursor(Texture2D cursorTexture)
    {
        //Replace the 'cursorTexture' with the cursor   
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        //Set the ccEnabled variable to true 
        this.ccEnabled = true;
    }
}
