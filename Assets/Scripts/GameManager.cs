using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  bool enableFramerateLimiting = false;
  bool wasLimited = false;

  [SerializeField]
  int targetFramerate = 30;

  int originalTargetFramerate;
  int originalVsyncCount;

  private void Start()
  {
    originalTargetFramerate = Application.targetFrameRate;
    originalVsyncCount = QualitySettings.vSyncCount;
  }

  private void Update()
  {
    if (enableFramerateLimiting && (!wasLimited || Application.targetFrameRate != targetFramerate))
    {
      Application.targetFrameRate = targetFramerate;
      QualitySettings.vSyncCount = 0;

      wasLimited = true;
    }
    else if (!enableFramerateLimiting && wasLimited)
    {
      Application.targetFrameRate = originalTargetFramerate;
      QualitySettings.vSyncCount = originalVsyncCount;
      wasLimited = false;
    }
  }
}
