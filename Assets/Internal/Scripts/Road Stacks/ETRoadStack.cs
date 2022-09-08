namespace ScrollingEnvironment
{
    public class ETRoadStack : System.IDisposable
    {
        private readonly SORoadStack _roadStackData = null;

        private readonly ETDistributor _distributorEntity = null;
        private readonly ETRoad[] _roadEntities = null;

        public ETRoadStack(SORoadStack roadStackData, ETDistributor distributorEntity)
        {
            _distributorEntity = distributorEntity;
            _roadStackData = roadStackData;
            
            _roadEntities = new ETRoad[_roadStackData.RoadsAmount];
            for (var i = 0; i < _roadStackData.RoadsAmount; ++i)
            {
                _roadEntities[i] = new ETRoad(_roadStackData.RoadsData[i], this);
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < _roadStackData.RoadsAmount; ++i)
            {
                _roadEntities[i].Dispose();
            }
        }

        public bool CheckHasAnyRoadCrossedStartPoint(bool isForDistributor = false)
        {
            if (isForDistributor)
            {
                return _distributorEntity.CheckHasAnyRoadStackCrossedStartPoint();
            }

            for (var i = 0; i < _roadStackData.RoadsAmount; ++i)
            {
                if (!_roadEntities[i].CheckHasCrossedStartPoint())
                {
                    return false;
                }
            }

            return true;
        }

        public ETRoad Distribute() 
            => _roadEntities[_roadStackData.GetRandomDistributionIndex()];
    }
}