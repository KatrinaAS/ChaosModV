#pragma once

#include "Component.h"
#include "EffectDispatcher.h"

#include <vector>
#include <memory>

#define _NODISCARD [[nodiscard]]

using DWORD64 = unsigned long long;
using BYTE = unsigned char;

using HANDLE = void*;

enum class EStreamOverlayMode : int
{
	ChatMessages,
	OverlayIngame,
	OverlayOBS
};

class StreamVoting : public Component
{
private:
	struct ChoosableEffect
	{
		ChoosableEffect(const EffectIdentifier& effectIdentifier, const std::string& szName, int iMatch)
			: m_EffectIdentifier(effectIdentifier), m_szEffectName(szName), m_iMatch(iMatch)
		{

		}

		EffectIdentifier m_EffectIdentifier;
		std::string m_szEffectName;
		int m_iMatch;
		int m_iChanceVotes = 0;
	};

	bool m_bEnableVoting;

	bool m_bReceivedHello = false;
	bool m_bReceivedFirstPing = false;
	bool m_bHasReceivedResult = false;

	int m_iStreamSecsBeforeVoting;

	bool m_bEnableStreamPollVoting = false;

	HANDLE m_hPipeHandle = INVALID_HANDLE_VALUE;

	DWORD64 m_ullLastPing = GetTickCount64();
	DWORD64 m_ullLastVotesFetchTime = GetTickCount64();

	int m_iNoPingRuns = 0;

	bool m_bIsVotingRoundDone = true;
	bool m_bNoVoteRound = false;
	bool m_bAlternatedVotingRound = false;

	EStreamOverlayMode m_eStreamOverlayMode;

	bool m_bEnableStreamChanceSystem;
	bool m_bEnableVotingChanceSystemRetainChance;
	bool m_bEnableStreamRandomEffectVoteable;
	
	std::array<BYTE, 3> m_rgTextColor;

	bool m_bIsVotingRunning = false;

	std::vector<std::unique_ptr<ChoosableEffect>> m_rgEffectChoices;

	std::unique_ptr<EffectIdentifier> m_pChosenEffectIdentifier;

public:
	StreamVoting(const std::array<BYTE, 3>& rgTextColor);
	~StreamVoting();

	virtual void Run() override;

	_NODISCARD bool IsEnabled() const;

	bool HandleMsg(const std::string& szMsg);

	void SendToPipe(std::string&& szMsg);

	void ErrorOutWithMsg(const std::string&& szMsg);
};