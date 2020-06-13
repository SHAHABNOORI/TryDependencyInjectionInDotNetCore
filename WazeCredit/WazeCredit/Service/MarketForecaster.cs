using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecaster : IMarketForecaster
    {
        public MarketResult GetMarketPrediction()
        {
            return new MarketResult()
            {
                MarketCondition = MarketCondition.StableUp
            };
        }
    }
}