using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedCharacter : MonoBehaviour
{
    [SerializeField]
    private string narrativeName;
    public string NarrativeName { get { return narrativeName; } }

    public float cameraWeight = 0;
    public bool targeted = false;

    private void Start()
    {
        CameraController.Instance.AddNamedCharacter(this);
    }

    private void OnDestroy()
    {
        CameraController.Instance.RemoveNamedCharacter(this);
    }
}
