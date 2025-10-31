using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Feed Cameras")]
    public List<Camera> cameras = new List<Camera>();

    [Header("UI Monitor")]
    public RawImage monitorImage;
    public TMP_Text cameraNumberText;

    [Header("Labels")]
    public List<string> cameraNames = new List<string>();

    private int currentIndex = 0;

    void Start()
    {
        SetActiveCamera(currentIndex);
    }

    public void NextCamera()
    {
        currentIndex = (currentIndex + 1) % cameras.Count;
        SetActiveCamera(currentIndex);
    }

    public void PreviousCamera()
    {
        currentIndex = (currentIndex - 1 + cameras.Count) % cameras.Count;
        SetActiveCamera(currentIndex);
    }

    private void SetActiveCamera(int index)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == index);
        }

        if (monitorImage != null)
            monitorImage.texture = cameras[index].targetTexture;

        if (cameraNumberText != null && cameraNames.Count > index)
            cameraNumberText.text = cameraNames[index];
    }
}
