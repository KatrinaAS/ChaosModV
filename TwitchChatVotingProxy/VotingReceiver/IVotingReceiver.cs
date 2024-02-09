namespace TwitchChatVotingProxy.VotingReceiver
{
    /// <summary>
    /// Defines the interface that a voting receiver needs to satisfy
    /// </summary>
    interface IVotingReceiver
    {
        /// <summary>
        /// Events which get invoked when the voting receiver receives a message
        /// </summary>
        event EventHandler<OnMessageArgs> OnMessage;
        /// <summary>
        /// Initializes the voting receiver
        /// </summary>
        /// <returns></returns>
        Task<bool> Init();
        /// <summary>
        /// Sends a message to the connected service
        /// </summary>
        /// <param name="message">Message that should be sent</param>
        Task SendMessage(string message);

        /// <summary>
        /// Does the voting receiver need things as data rather then text
        /// </summary>
        bool IsDataBased() => false;

        /// <summary>
        /// Send the voting data to a receiver
        /// </summary>
        /// <param name="options">Voting options</param>
        /// <param name="percentage">Is voting proportional?</param>
        /// <returns>true if in use/successful</returns>
        async Task<bool> SendData(List<IVoteOption> options, bool percentage) { return false; }


        /// <summary>
        /// Informs the Voting Receiver that the voting has ended
        /// </summary>
        async Task EndVoting() { }

        /// <summary>
        /// Informs the receiver about a no voting round
        /// </summary>
        async Task NoVotingRound() { }
        /// <summary>
        /// Informs the receiver about possible updates
        /// </summary>
        /// <param name="votes"></param>
        async Task UpdateVoting(List<IVoteOption> votes) { }
    }
}
