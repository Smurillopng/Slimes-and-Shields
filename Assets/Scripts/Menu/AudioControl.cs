using System;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Menu
{
	public class AudioControl : MonoBehaviour
	{
		public static AudioControl Instance;

		[SerializeField] private AudioMixerGroup musicMixerGroup;
		[SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
		[SerializeField] private Sound[] sounds;

		private void Awake()
		{
			Instance = this;

			foreach (var s in sounds)
			{
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.audioClip;
				s.source.loop = s.isLoop;
				s.source.volume = s.volume;

				s.source.outputAudioMixerGroup = s.audioType switch
				{
					Sound.AudioTypes.SoundEffect => soundEffectsMixerGroup,
					Sound.AudioTypes.Music => musicMixerGroup,
					_ => s.source.outputAudioMixerGroup
				};

				if (s.playOnAwake)
					s.source.Play();
			}
		}

		public void Play(string clipName)
		{
			var s = Array.Find(sounds, theSound => theSound.clipName == clipName);
			if (s == null)
			{
				this.LogError("Sound: " + clipName + " does NOT exist!");
				return;
			}
			s.source.Play();
		}

		public void Stop(string clipName)
		{
			var s = Array.Find(sounds, dummySound => dummySound.clipName == clipName);
			if (s == null)
			{
				this.LogError("Sound: " + clipName + " does NOT exist!");
				return;
			}
			s.source.Stop();
		}
	}
}
