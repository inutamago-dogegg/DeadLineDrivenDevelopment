namespace ArtC.Systems {
    public struct ResultData {
        public ResultData(GameStates.ResultReason reason) {
            Reason = reason;
        }

        public GameStates.ResultReason Reason { get; }
    }
}
