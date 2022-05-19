#include <stdafx.h>

/*
	Effect by Last0xygen
*/

static void OnTick()
{
	static const int lagTimeDelay = 1000 / 25;
	int lastUpdateTick			  = GetTickCount64();
	while (lastUpdateTick > GetTickCount64() - lagTimeDelay)
	{
		// Create Lag
	}
}

// clang-format off
static RegisterEffect registerEffect(nullptr, nullptr, OnTick, EffectInfo
	{
		.Name = "Console Experience",
		.Id = "misc_fps_limit",
		.IsTimed = true,
		.IsShortDuration = true
	}
);