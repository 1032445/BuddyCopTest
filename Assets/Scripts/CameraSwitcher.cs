using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.UI;
using TMPro;
using System;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Room Cameras")]
    public List<Camera> cameras = new List<Camera>();

    [Header("UI")]
    public RawImage monitorImage;
    public TMP_Text cameraLabel;

    [Header("Ping System")]
    public RectTransform monitorRectTransform;
    public bool pingModeActive = false;

    [Header("Agent")]
    public AgentController agent;

    [HideInInspector]
    public int currentIndex = 0;

    void Start()
    {
        SetActiveCamera(currentIndex);
    }

    void Update()
    {
        if (pingModeActive && Input.GetMouseButtonDown(0))
        {
            PingFromMonitor();
        }
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
            cameras[i].gameObject.SetActive(i == index);

        if (monitorImage != null)
            monitorImage.texture = cameras[index].targetTexture;

        if (cameraLabel != null)
            cameraLabel.text = cameras[index].name;

        // Update RawImage aspect
        ImageAspect aspectFitter = monitorImage.GetComponent<ImageAspect>();
        if (aspectFitter != null)
            aspectFitter.UpdateAspect(cameras[index]);
    }

    private void PingFromMonitor()
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(monitorRectTransform, Input.mousePosition))
            return;

        // Get the local point in the RawImage
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            monitorRectTransform, Input.mousePosition, null, out localPoint);

        Rect rect = monitorRectTransform.rect;

        // Convert local point to 0-1 UV (account for pivot at center)
        Vector2 uv = new Vector2(
            (localPoint.x - rect.x) / rect.width,
            (localPoint.y - rect.y) / rect.height
        );

        Camera activeCam = cameras[currentIndex];

        // Convert UV (0-1) to viewport coordinates (0-1)
        Vector3 viewportPoint = new Vector3(uv.x, uv.y, activeCam.nearClipPlane);

        // Map viewport point to world position
        Vector3 worldPos = activeCam.ViewportToWorldPoint(viewportPoint);

        // Use agent
        if (agent != null)
            agent.MoveTo(worldPos);

        pingModeActive = false;
    }

    public void TogglePingMode()
    {
        pingModeActive = !pingModeActive;
        Debug.Log("Ping Mode: " + (pingModeActive ? "ON" : "OFF"));
    }
}
