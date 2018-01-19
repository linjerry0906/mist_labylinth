//Å@ToDoÅFê[ìxÇì«Ç›éÊÇÈ
float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.
texture Texture0;

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

	//float4 Depth : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(input.Position, Projection);

    // TODO: add your vertex shader code here.
	//output.Depth = output.Position;
	output.TexUV0 = input.TexUV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	//float depth = (input.Depth.z / input.Depth.w);

	float4 Color;

	Color = tex2D(MainSampler, input.TexUV0);

	Color += tex2D(MainSampler, input.TexUV0 + float2(-0.001f, -0.001f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(-0.002f, 0.0f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(-0.001f, 0.001f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(0.0f, -0.002f));
	Color += tex2D(MainSampler, input.TexUV0) * 2;
	Color += tex2D(MainSampler, input.TexUV0 + float2(0.0f, 0.002f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(0.001f, -0.001f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(0.002f, 0.0f));
	Color += tex2D(MainSampler, input.TexUV0 + float2(0.001f, 0.001f));

    return Color / 10.0f;
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
