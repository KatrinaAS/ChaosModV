#pragma once

#include "Component.h"
#include "EffectDispatcher.h"

#include <memory>
#include <vector>

using DWORD64 = unsigned long long;
using BYTE    = unsigned char;

using HANDLE  = void *;

enum class EChatVoteOverlayMode : int
{
	ChatMessages,
	OverlayIngame,
	OverlayOBS
};

class ChatVoting : public Component
{
  private:
	struct ChoosableEffect
	{
		ChoosableEffect(const EffectIdentifier &effectIdentifier, const std::string &szName, int iMatch)
		    : m_EffectIdentifier(effectIdentifier), m_szEffectName(szName), m_iMatch(iMatch)
		{
		}

		EffectIdentifier m_EffectIdentifier;
		std::string m_szEffectName;
		int m_iMatch;
		int m_iChanceVotes = 0;
	};

	bool m_bEnableChatVoting;

	bool m_bReceivedHello     = false;
	bool m_bReceivedFirstPing = false;
	bool m_bHasReceivedResult = false;

	int m_iChatSecsBeforeVoting;

	HANDLE m_hPipeHandle            = INVALID_HANDLE_VALUE;

	DWORD64 m_ullLastPing           = GetTickCount64();
	DWORD64 m_ullLastVotesFetchTime = GetTickCount64();

	int m_iNoPingRuns               = 0;

	bool m_bIsVotingRoundDone       = true;
	bool m_bAlternatedVotingRound   = false;

	EChatVoteOverlayMode m_eChatOverlayMode;

	bool m_bEnableChatChanceSystem;
	bool m_bEnableVotingChanceSystemRetainChance;
	bool m_bEnableChatRandomEffectVoteable;

	std::array<BYTE, 3> m_rgTextColor;

	bool m_bIsVotingRunning = false;

	std::vector<std::unique_ptr<ChoosableEffect>> m_rgEffectChoices;

	std::unique_ptr<EffectIdentifier> m_pChosenEffectIdentifier;
	std::string GetPipeJson(std::string identifier, std::vector<std::string> params);

  protected:
	ChatVoting(const std::array<BYTE, 3> &rgTextColor);
	virtual ~ChatVoting() override;

  public:
	virtual void OnModPauseCleanup() override;
	virtual void OnRun() override;

	bool IsEnabled() const;

	bool HandleMsg(const std::string &szMsg);

	void SendToPipe(std::string identifier, std::vector<std::string> params = {});

	void ErrorOutWithMsg(const std::string &&szMsg);

	template <class T>
	requires std::is_base_of_v<Component, T>
	friend struct ComponentHolder;
};