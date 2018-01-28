float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.
texture Texture0;
float alpha;
float4 color;

float3 cameraPos;

bool fogEnable;
float4 fogColor;
float fogNear;
float fogFar;

sampler MainSampler : register(s0) = sampler_state
{
	Texture = <Texture0>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexUV : TEXCOORD0;

	// TODO: add input channels such as texture
	// coordinates and vertex colors here.
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexUV0 : TEXCOORD0;
	float Distance : TEXCOORD1;

	// TODO: add vertex shader outputs such as colors and texture
	// coordinates here. These values will automatically be interpolated
	// over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.TexUV0 = input.TexUV;

	output.Distance = length(worldPosition - cameraPos);
	// TODO: add your vertex shader code here.

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// TODO: add your pixel shader code here.
	float4 tex = tex2D(MainSampler, input.TexUV0) * alpha;

	if (tex.a < 0.11f)		//“§–¾“x‚ª0.11ˆÈ‰º‚Ìê‡‚Í•úŠü
		discard;

	tex *= color;
	if (fogEnable)
	{
		float l = saturate((input.Distance - fogNear) / (fogFar - fogNear));

		return lerp(tex, fogColor, l);
	}

	return tex;
}

technique Technique1
{
	pass Pass1
	{
		// TODO: set renderstates here.

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
