export type VoteOption = {
    Label: string,
    Votes: number,
    Matches: string[]
}
export type WebsocketMessage = {
    percentage: boolean,
    options: VoteOption[]
}