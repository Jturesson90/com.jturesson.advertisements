namespace Jturesson.Advertisements
{
    public struct RewardAdvertisementFinishedArgs
    {
        public RewardAdvertisementResult ShowResult;
        public string ReferenceId;

        public RewardAdvertisementFinishedArgs(RewardAdvertisementResult showResult, string referenceId)
        {
            ShowResult = showResult;
            ReferenceId = referenceId;
        }
    }
}