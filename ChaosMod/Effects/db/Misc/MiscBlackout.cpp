#include <stdafx.h>

static void OnStart()
{
	SET_CLOCK_TIME(0, 0, 0);
}

static void OnStop()
{
	SET_ARTIFICIAL_LIGHTS_STATE(false);
}

static void OnTick()
{
	SET_ARTIFICIAL_LIGHTS_STATE(true);
}

// clang-format off
static RegisterEffect registerEffect(OnStart, OnStop, OnTick, EffectInfo
	{
		.Name = "Blackout",
		.Id = "world_blackout",
		.IsTimed = true
	}
);