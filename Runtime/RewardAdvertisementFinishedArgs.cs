namespace JTuresson.Advertisements
{
    public struct RewardAdvertisementFinishedArgs
    {
        public readonly RewardAdvertisementResult ShowResult;
        public readonly string ReferenceId;

        public RewardAdvertisementFinishedArgs(RewardAdvertisementResult showResult, string referenceId)
        {
            ShowResult = showResult;
            ReferenceId = referenceId;
        }
    }
}