float4x4 World;
float4x4 View;
float4x4 Projection;

float BloomIntensity = 3.0;			//Lightの明るさ
float OriginalIntensity = 0.8;		//元の明るさ
float BloomSaturation = 1.0;		//灰色調Lerp用
float OriginalSaturation = 1.0;		//灰色調Lerp用

sampler BloomSampler : register(s0);
texture OriginColor;
sampler OriginColorSampler = sampler_state
{
	Texture = <OriginColor>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

//灰色調に調整するか
float4 AdjustSaturation(float4 color, float saturation)
{
	float gray = dot(color, float3(0.3f, 0.59f, 0.11f));
	return lerp(gray, color, saturation);
}

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 bloomColor = tex2D(BloomSampler, texCoord);
	float4 originColor = tex2D(OriginColorSampler, texCoord);

	bloomColor = AdjustSaturation(bloomColor, BloomSaturation) * BloomIntensity;
	originColor = AdjustSaturation(originColor, OriginalSaturation) * OriginalIntensity;

	originColor *= (1 - saturate(bloomColor));

    return originColor + bloomColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
