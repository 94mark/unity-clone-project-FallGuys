using UnityEngine;

public class EffectShaderPropertyStr {
	public const string ColorStr = "_TintColor";
	public const string MainTexStr = "_MainTex";
	public const string CutTexStr = "_CutTex";
	public const string CutOffStr = "_Cutoff";
	public const string MainRotationStr = "_MainRotation";
	public const string CutRotationStr = "_CutRotation";
	public const string UVScrollX = "_UVScrollX";
	public const string UVScrollY = "_UVScrollY";
	public const string UVCutScrollX = "_UVCutScrollX";
	public const string UVCutScrollY = "_UVCutScrollY";
    public const string CutParticleSoftValue = "_InvFade";
	public const string UVMirrorX = "_UVMirrorX";
	public const string UVMirrorY = "_UVMirrorY";
	public const string DissolveSrc = "_DissolveSrc";
	public const string SpecColor = "_SpecColor";
	public const string Shininess = "_Shininess";
	public const string Amount = "_Amount";
	public const string StartAmount = "_StartAmount";
	public const string DissColor = "_DissColor";
	public const string Illuminate = "_Illuminate";
	public const string EmissionGain = "_EmissionGain";
	public const string ShadowColor = "_ShadowColor";
	public const string SpecularPower = "_SpecularPower";
	public const string EdgeThickness = "_EdgeThickness";
	public const string EdgeSaturtion = "_EdgeSaturtion";
	public const string EdgeBrightness = "_EdgeBrightness";
	public const string FalloffSampler = "_FalloffSampler";
	public const string RimLightSampler = "_RimLightSampler";
	public const string ColorFactor = "_ColorFactor";
	//特效在用的
	public const string EnableAlphaMaskStr = "Enable_AlphaMask";
	public const string EnableUVRotationStr = "Enable_UVRotation";
	public const string EnableUVScrollStr = "Enable_UVScroll";
	public const string EnableUVMirror = "Enable_UVMirror";
	public const string EnableBloom = "Enable_Bloom";
	
	public static readonly int Material_Color = Shader.PropertyToID (EffectShaderPropertyStr.ColorStr);
	public static readonly int Material_Color_Factor = Shader.PropertyToID (EffectShaderPropertyStr.ColorFactor);
}
