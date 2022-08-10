namespace JTuresson.Advertisements
{
    public struct RewardAdvertisementFinishedArgs
    {
        public readonly RewardAdvertisementResult ShowResult;

        public RewardAdvertisementFinishedArgs(RewardAdvertisementResult showResult)
        {
            ShowResult = showResult;
        }
    }
}