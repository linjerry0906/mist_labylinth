float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.
texture Texture0;

float BlurRate;

float offsetX;
float offsetY;

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
	float2 TexUV1 : TEXCOORD1;
	float2 TexUV2 : TEXCOORD2;
	float2 TexUV3 : TEXCOORD3;
	float2 TexUV4 : TEXCOORD4;

	// TODO: add vertex shader outputs such as colors and texture
	// coordinates here. These values will automatically be interpolated
	// over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, Projection);

	output.TexUV0 = input.TexUV + float2(BlurRate, 0);
	output.TexUV1 = input.TexUV + float2(-BlurRate, 0);
	output.TexUV2 = input.TexUV + float2(0, BlurRate);
	output.TexUV3 = input.TexUV + float2(0, -BlurRate);
	output.TexUV4 = input.TexUV + float2(0, 0);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 Color = tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY)) / 5.0f;

	Color += tex2D(MainSampler, input.TexUV1 + float2(offsetX, offsetY)) / 5.0f;

	Color += tex2D(MainSampler, input.TexUV2 + float2(offsetX, offsetY)) / 5.0f;

	Color += tex2D(MainSampler, input.TexUV3 + float2(offsetX, offsetY)) / 5.0f;

	Color += tex2D(MainSampler, input.TexUV4) / 5.0f;

	return Color;
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
