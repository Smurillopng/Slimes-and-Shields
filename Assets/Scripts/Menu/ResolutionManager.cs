using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
	public class ResolutionManager : MonoBehaviour
	{
		private const string ResolutionWidthPlayerPrefKey = "ResolutionWidth";
		private const string ResolutionHeightPlayerPrefKey = "ResolutionHeight";
		private const string ResolutionRefreshRatePlayerPrefKey = "RefreshRate";
		private const string FullScreenPlayerPrefKey = "FullScreen";
		public Toggle fullScreenToggle;
		public TMPro.TMP_Dropdown resolutionDropdown;
		private Resolution[] _resolutions;
		private Resolution _selectedResolution;

		private void Start()
		{
			_resolutions = Screen.resolutions;
			LoadSettings();
			CreateResolutionDropdown();
 
			fullScreenToggle.onValueChanged.AddListener(SetFullscreen);
			resolutionDropdown.onValueChanged.AddListener(SetResolution);
		}
 
		private void LoadSettings()
		{
			_selectedResolution = new Resolution
			{
				width = PlayerPrefs.GetInt(ResolutionWidthPlayerPrefKey, Screen.currentResolution.width),
				height = PlayerPrefs.GetInt(ResolutionHeightPlayerPrefKey, Screen.currentResolution.height),
				refreshRate = PlayerPrefs.GetInt(ResolutionRefreshRatePlayerPrefKey, Screen.currentResolution.refreshRate)
			};

			fullScreenToggle.isOn = PlayerPrefs.GetInt(FullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) > 0;
 
			Screen.SetResolution(
				_selectedResolution.width,
				_selectedResolution.height,
				fullScreenToggle.isOn
			);
		}
 
		private void CreateResolutionDropdown()
		{
			resolutionDropdown.ClearOptions();
			var options = new List<string>();
			var currentResolutionIndex = 0;
			for (var i = 0; i < _resolutions.Length; i++)
			{
				if (Math.Abs(_resolutions[i].width / (float)_resolutions[i].height - Screen.width / (float)Screen.height) > 0.1f) { continue; }
				var option = _resolutions[i].width + "x" + _resolutions[i].height;
				options.Add(option);
				if (Mathf.Approximately(_resolutions[i].width, _selectedResolution.width) && Mathf.Approximately(_resolutions[i].height, _selectedResolution.height))
				{
					currentResolutionIndex = i;
				}
			}
			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();
		}

		private static void SetFullscreen(bool isFullscreen)
		{
			Screen.fullScreen = isFullscreen;
			PlayerPrefs.SetInt(FullScreenPlayerPrefKey, isFullscreen ? 1 : 0);
		}

		private void SetResolution(int resolutionIndex)
		{
			_selectedResolution = _resolutions[resolutionIndex];
			Screen.SetResolution(_selectedResolution.width, _selectedResolution.height, Screen.fullScreen);
			PlayerPrefs.SetInt(ResolutionWidthPlayerPrefKey, _selectedResolution.width);
			PlayerPrefs.SetInt(ResolutionHeightPlayerPrefKey, _selectedResolution.height);
			PlayerPrefs.SetInt(ResolutionRefreshRatePlayerPrefKey, _selectedResolution.refreshRate);
		}
	}
}