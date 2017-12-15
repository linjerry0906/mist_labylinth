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

	// TODO: add vertex shader outputs such as colors and texture
	// coordinates here. These values will automatically be interpolated
	// over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, Projection);

	output.TexUV0 = input.TexUV;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 Color = 0;

	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(-BlurRate, -BlurRate));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(0, -BlurRate));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(BlurRate, -BlurRate));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(-BlurRate, 0));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(0, 0));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(BlurRate, 0));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(-BlurRate, BlurRate));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(0, BlurRate));
	Color += tex2D(MainSampler, input.TexUV0 + float2(offsetX, offsetY) + float2(BlurRate, BlurRate));

	return Color / 9;
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
