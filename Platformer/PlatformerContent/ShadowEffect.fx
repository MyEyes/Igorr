float4x4 View;
float4x4 Projection;

float2   TextureSize     : register(c1);
texture ScreenTexture;    
sampler TexSampler:register(s0);
sampler TextureSampler:register(s1)
= sampler_state 
{
    Texture = <ScreenTexture>;    
};
float viewDistance = 1.0f;
float shadowDarkness = 10.0f;

// TODO: Fügen Sie Effektparameter hier hinzu.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TexCoord:TEXCOORD0;
    // TODO: Fügen Sie Eingabekanäle, wie Texturkoordinaten
    // und Vertex-Farben, hier hinzu.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 ScreenPos : TEXCOORD0;
	float4 Color : COLOR0;
    // TODO: Fügen Sie Vertex-Shader-Ausgaben, wie Farben und Texturkoordinaten,
    // hier hinzu. Diese Werte werden automatisch über das Dreieck
    // interpoliert und als Eingabe für Ihren Pixel-Shader geliefert.
};

struct ClearPSInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

ClearPSInput ClearVSFunction(ClearPSInput input)
{
	return input;
}

struct TileVSInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct TilePSInput
{
	float4 Position : POSITION0;
	float4 ScreenPos : TEXCOORD0;
};

struct ForegroundTilePSInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 ScreenPos : TEXCOORD1;
};

struct SpritePSInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

TilePSInput TileVSFunction(TileVSInput input)
{
	TilePSInput result;
	//result.Position=input.Position;
	float4 viewPosition = mul(input.Position, View);
    result.Position = mul(viewPosition, Projection);
	result.ScreenPos=result.Position;
	return result;
}

float4 TilePSFunction(TilePSInput input) : COLOR
{
	return float4(float3(1,1,1),1);//*(1-length(input.ScreenPos.xy)/viewDistance),1);
}

ForegroundTilePSInput ForegroundTileVSFunction(TileVSInput input)
{
	ForegroundTilePSInput result;
	//result.Position=input.Position;
	float4 viewPosition = mul(input.Position, View);
    result.Position = mul(viewPosition, Projection);
	result.TexCoord = input.TexCoord;
	result.ScreenPos = result.Position;
	return result;
}

float4 ForegroundTilePSFunction(ForegroundTilePSInput input) : COLOR
{
	clip(tex2D(TextureSampler, input.TexCoord).a-0.5f);
	return float4(1,1,1,1)*saturate(1-length(input.ScreenPos.xy)/viewDistance);
}

float4 SpritePSFunction(SpritePSInput input) : COLOR
{
	float4 color = tex2D(TexSampler, input.TexCoord);
	clip(color.a-0.5f);
	return color*input.Color;
}

VertexShaderOutput GlowVSFunction(VertexShaderInput input)
{
	VertexShaderOutput result = (VertexShaderOutput)0;
	float4 viewPosition = mul(input.Position, View);
    result.Position = mul(viewPosition, Projection);
	result.ScreenPos.xy=2*input.TexCoord.xy-1;
	result.Color = input.Color;
	return result;
}

float4 GlowPSFunction(VertexShaderOutput input) : COLOR
{
	return float4(input.Color.rgb*(1-length(input.ScreenPos.xy)),1);
}

float4 ClearPSFunction(ClearPSInput input) : COLOR
{
	return float4(float3(shadowDarkness,shadowDarkness,shadowDarkness),1);//*(1-length(2*input.Color.xy-1)/viewDistance),1);
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	input.Position.w=1;
    float4 viewPosition = mul(input.Position, View);
    output.Position = mul(viewPosition, Projection);
	output.Position.w=1;
	output.ScreenPos=output.Position;
	output.Color = input.Color;
    // TODO: Fügen Sie Ihren Vertex-Shader-Code hier hinzu.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: Fügen Sie Ihren Pixel-Shader-Code hier hinzu.
	return float4(1,1,1,1)*saturate(1-length(input.ScreenPos.xy)/viewDistance)/shadowDarkness;
    return float4(1,1,1,1)*(input.Color.rgba);//*(1-length(input.ScreenPos.xy));
}

technique Shadow
{
    pass Pass1
    {
        // TODO: Stellen Sie Renderstates hier ein.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Clear
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 ClearVSFunction();
		PixelShader = compile ps_2_0 ClearPSFunction();
	}
}

technique Tile
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 TileVSFunction();
		PixelShader = compile ps_2_0 TilePSFunction();
	}
}

technique ForegroundTile
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 ForegroundTileVSFunction();
		PixelShader = compile ps_2_0 ForegroundTilePSFunction();
	}
}

technique Sprite
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 SpritePSFunction();
	}
}

technique Glow
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 GlowVSFunction();
		PixelShader = compile ps_2_0 GlowPSFunction();
	}
}