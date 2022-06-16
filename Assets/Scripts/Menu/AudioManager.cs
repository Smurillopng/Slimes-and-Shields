using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
	[RequireComponent(typeof(Slider))]
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance;
		public AudioMixer mixer;
		[SerializeField] private string volumeName;
		[SerializeField] private TMP_Text label; // TextMeshPro Text
		private Slider slider => GetComponent<Slider>();

		private void Start()
		{
			GetFloat(volumeName);
			slider.value = GetFloat(volumeName);
			slider.onValueChanged.AddListener(delegate{OnValueChange(GetFloat(volumeName));});
		}

		private void Update()
		{
			slider.value = GetFloat(volumeName);
		
			if(label != null)
			{
				label.text = Mathf.Round(GetFloat(volumeName) * 100.0f) + "%";
			}
		}

		private static void SetFloat(string keyName, float value)
		{
			PlayerPrefs.SetFloat(keyName, value);
		}

		private static float GetFloat(string keyName)
		{
			return PlayerPrefs.GetFloat(keyName);//ele tem o default, se ele n existir, ele usa o default
		}

		private void OnValueChange(float value)
		{
			if (mixer == null) return;
			mixer.SetFloat(volumeName, Mathf.Log(value) * 20);
			SetFloat(volumeName, slider.value);
		}
	}
}
