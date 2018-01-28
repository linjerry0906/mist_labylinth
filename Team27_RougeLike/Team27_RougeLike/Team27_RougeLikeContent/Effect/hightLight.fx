sampler TextureSampler : register(s0);
float Threshold = 0.4f;		//�傫���ق�Range���Ⴂ

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 Color = tex2D(TextureSampler, texCoord);


	return saturate((Color - Threshold) / (1 - Threshold));
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
