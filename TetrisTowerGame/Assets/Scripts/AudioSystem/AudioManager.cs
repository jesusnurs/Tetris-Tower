using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private const string MusicMutedKey = "IsMuted";
	private const string SoundsMutedKey = "IsMuted";

	public static AudioManager Instance { get; private set; }

	[System.Serializable]
	public class Sound
	{
		public string name;
		public AudioClip clip;
		public bool loop;
		[Range(0f, 1f)] public float volume = 1f;
		public float pitch = 1f;
	}

	public Sound[] sounds;
	private bool isSoundsMuted;
	private bool isMusicMuted;
	private Dictionary<AudioClipEnum, Sound> soundDictionary;
	private Dictionary<AudioClipEnum, List<AudioSource>> activeSources;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
		isSoundsMuted = PlayerPrefs.GetInt(SoundsMutedKey, 0) == 1;
		isMusicMuted = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
		soundDictionary = new Dictionary<AudioClipEnum, Sound>();
		activeSources = new Dictionary<AudioClipEnum, List<AudioSource>>();

		InitializeSounds();
	}

	public void ToggleSoundsMute()
	{
		isSoundsMuted = !isSoundsMuted;
		PlayerPrefs.SetInt(SoundsMutedKey, isSoundsMuted ? 1 : 0);
		foreach (var soundEnum in soundDictionary.Keys)
		{
			foreach (var source in activeSources[soundEnum])
			{
				if (source != null && soundEnum != AudioClipEnum.Background)
					source.volume = isSoundsMuted ? 0f : soundDictionary[soundEnum].volume;
			}
		}
	}

	public void ToggleMusicMute()
	{
		isMusicMuted = !isMusicMuted;
		PlayerPrefs.SetInt(MusicMutedKey, isMusicMuted ? 1 : 0);
		foreach (var soundEnum in soundDictionary.Keys)
		{
			foreach (var source in activeSources[soundEnum])
			{
				if (source != null && soundEnum == AudioClipEnum.Background)
					source.volume = isMusicMuted ? 0f : soundDictionary[soundEnum].volume;
			}
		}
	}

	private void InitializeSounds()
	{
		foreach (var sound in sounds)
		{
			if (string.IsNullOrEmpty(sound.name))
				continue;

			AudioClipEnum soundEnum = GenerateEnumFromName(sound.name);
			soundDictionary[soundEnum] = sound;
			activeSources[soundEnum] = new List<AudioSource>();
		}
	}

	public void Play(AudioClipEnum soundEnum)
	{
		if (!soundDictionary.ContainsKey(soundEnum))
		{
			Debug.LogWarning($"Sound '{soundEnum}' not found!");
			return;
		}

		Sound sound = soundDictionary[soundEnum];
		AudioSource source = CreateAudioSource(sound);
		source.volume = isSoundsMuted ?  0 : source.volume;
		source.Play();
		activeSources[soundEnum].Add(source);

		if (!sound.loop)
		{
			Destroy(source.gameObject, sound.clip.length / sound.pitch);
			activeSources[soundEnum].Remove(source);
		}
	}

	public void Stop(AudioClipEnum soundEnum)
	{
		if (!activeSources.ContainsKey(soundEnum)) return;
		foreach (var source in activeSources[soundEnum])
		{
			if (source != null)
			{
				source.Stop();
				Destroy(source.gameObject);
			}
		}

		activeSources[soundEnum].Clear();
	}

	private AudioSource CreateAudioSource(Sound sound)
	{
		GameObject soundObject = new GameObject(sound.name);
		AudioSource source = soundObject.AddComponent<AudioSource>();
		source.clip = sound.clip;
		source.volume = sound.volume;
		source.pitch = sound.pitch;
		source.loop = sound.loop;
		soundObject.transform.parent = transform;

		return source;
	}

	private AudioClipEnum GenerateEnumFromName(string soundName)
	{
		string validName = soundName.Replace(" ", "_").Replace("-", "_");
		return (AudioClipEnum)System.Enum.Parse(typeof(AudioClipEnum), validName, true);
	}

	public bool IsSoundMuted() => isSoundsMuted;
	public bool IsMusicMuted() => isMusicMuted;
}
