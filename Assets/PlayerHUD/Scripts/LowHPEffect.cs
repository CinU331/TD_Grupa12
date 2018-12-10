using System;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LowHPEffect : MonoBehaviour {
	private Material material;
	private VignetteModel.Settings vignetteSettings;
	public PostProcessingProfile ppProfile;

	private float MaxHealthPoints;
	private float currentHealthPoints;
	
	void Start () {
		material = new Material(Shader.Find("Hidden/BWDiffuse"));

		vignetteSettings = ppProfile.vignette.settings;
		vignetteSettings.color = Color.red;
		vignetteSettings.intensity = 0;
		ppProfile.vignette.settings = vignetteSettings;
	}
		
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (currentHealthPoints > (MaxHealthPoints / 2) || Math.Abs(currentHealthPoints - MaxHealthPoints) < 1f)
		{
			Graphics.Blit (source, destination);
			vignetteSettings.intensity = 0;
			ppProfile.vignette.settings = vignetteSettings;
			return;
		}
 
		material.SetFloat("_bwBlend", 1 - currentHealthPoints / MaxHealthPoints);
		Graphics.Blit(source, destination, material);
		
		vignetteSettings.intensity = 0.8f - currentHealthPoints / MaxHealthPoints;
		ppProfile.vignette.settings = vignetteSettings;
	}

	public void InitState(float maxHP)
	{
		MaxHealthPoints = maxHP;
		currentHealthPoints = maxHP;
	}

	public void SetCurrentHp(float currentHP)
	{
		currentHealthPoints = currentHP;
	}
}
