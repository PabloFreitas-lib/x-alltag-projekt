using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    /*
    * Used to save general GameState.
    * (Player position, how many and which files cubes are there?, user(?))
    */
    public void saveComplexUserPrefs()
    {

    }

    /*
     * Used to load general GameState.
     * (Player position, how many and which files cubes are there?, user(?))
     */
    public void loadComplexUserPrefs()
    {

    }

    /*
     * Called to save Mindmap when clicking button or on socket interaction.
     */
    public void saveMindmap(Mindmap mindmap)
    {
       // string json = JsonUtility.toJSON(new MindmapPersistentObject(mindmap));
    }


    /*
     * Called to load Mindmap when clicking button or on socket interaction.
     */
    public void loadMindmap()
    {

    }

    /*
     * Called to save Whiteboard when clicking button or on whiteboard interaction.
     */
    public void saveWhiteboard()
    {

    }

    /*
     * Called to load Whiteboard when clicking button or on whiteboard interaction..
     */
    public void loadWhitebaord()
    {

    }

    /*
     * Objects of this class have the purpose of collecting serializable data of complex user preferences to be saved from or respectively be loaded to.
     */
    [System.Serializable]
    public class ComplexPlayerPrefsPersistentObject
    {
        public ComplexPlayerPrefsPersistentObject()
        {

        }
    }

    /*
    * Objects of this class have the purpose of collecting serializable data of a mindmap to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class MindmapPersistentObject
    {
        public MindmapPersistentObject(Mindmap mindmap) 
        {
            
        }
    }

    /*
    * Objects of this class have the purpose of collecting serializable data of a whiteboard to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class WhiteboardPersistentObject
    {
        public WhiteboardPersistentObject()
        {

        }
    }



}
