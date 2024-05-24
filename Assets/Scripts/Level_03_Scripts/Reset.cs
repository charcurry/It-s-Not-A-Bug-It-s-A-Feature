using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;


public class Reset : MonoBehaviour
{
    private List<GetPosition> objectsToResetList = new List<GetPosition>();
    private List<Vector3> objectsToResetPos = new List<Vector3>();
    private List<Quaternion> objectsToResetRot = new List<Quaternion>();
    private List<string> objectsToResetName = new List<string>();
    private GameObject heldObject;
    [HideInInspector] public bool hasDuplicated;

    [Header("References")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject heartBoxPrefab;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject keycardPrefab;
    [SerializeField] private GameObject keyCartoonPrefab;
    [SerializeField] private GameObject keyOldPrefab;
    [SerializeField] private GameObject keyEyePrefab;
    [SerializeField] private GameObject keySimplePrefab;

    void Start()
    {
        GetPosition[] objectsToResetArray = FindObjectsOfType<GetPosition>();
        objectsToResetList.AddRange(objectsToResetArray);

        foreach (GetPosition obj in objectsToResetList)
        {
            objectsToResetPos.Add(obj.initialPosition);
            objectsToResetRot.Add(obj.initialRotation);
            objectsToResetName.Add(obj.prefabName);
        }
    }

    public void ResetLevel()
    {
        int index = 0;
        List<GetPosition> tempObjectsToResetList = new List<GetPosition>();

        foreach (GetPosition obj in objectsToResetList)
        {
            // Reset with duplication
            if (obj != null && obj.GetComponent<Interactable>().isPickedUp)
            {
                hasDuplicated = true;
                heldObject = obj.gameObject;
                GameObject newObject = Instantiate(heldObject, objectsToResetPos[index], objectsToResetRot[index]);
                newObject.GetComponent<Rigidbody>().useGravity = true;
                newObject.GetComponent<Interactable>().isPickedUp = false;
                tempObjectsToResetList.Add(newObject.GetComponent<GetPosition>());
            }

            // Reset without duplication
            else 
            {
                if (obj != null)
                    Destroy(obj.gameObject);

                GameObject tempObj = null;

                switch (objectsToResetName[index])
                {
                    case "box":
                        tempObj = Instantiate(boxPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "heartBox":
                        tempObj = Instantiate(heartBoxPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "barrel":
                        tempObj = Instantiate(barrelPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "keycard":
                        tempObj = Instantiate(keycardPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "keyCartoon":
                        tempObj = Instantiate(keyCartoonPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "keyOld":
                        tempObj = Instantiate(keyOldPrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "keyEye":
                        tempObj = Instantiate(keyEyePrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;

                    case "keySimple":
                        tempObj = Instantiate(keySimplePrefab, objectsToResetPos[index], objectsToResetRot[index]);
                        break;
                }

                
                tempObjectsToResetList.Add(tempObj.GetComponent<GetPosition>());
            }

            index++;
        }

        objectsToResetList = tempObjectsToResetList;
    }
}
