using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Entity;

namespace FrameworkUAD.BusinessLogic.Transformations
{
    public class TransformationDefinitionsGrouping
    {
        public HashSet<Transformation> DataMapTrans { get; set; }
        public HashSet<Transformation> JoinTrans { get; set; }
        public HashSet<Transformation> SplitTrans { get; set; }
        public HashSet<Transformation> AssignTrans { get; set; }
        public HashSet<Transformation> TransSplit { get; set; }
        public HashSet<Transformation> AllTrans { get; set; }
        public HashSet<TransformAssign> AllTransAssign { get; set; }
        public HashSet<TransformDataMap> AllTransDataMap { get; set; }
        public HashSet<TransformSplit> AllTransSplitMap { get; set; }
        public HashSet<TransformJoin> AllTransJoinMap { get; set; }
        public HashSet<TransformSplitTrans> AllSplitTrans { get; set; }
        public HashSet<TransformDataMap> EveryTransDataMap { get; set; }
        public HashSet<TransformSplit> EveryTransSplitMap { get; set; }

        public IEnumerable<TransformDataMap> GetDataMaps(int transformationId, int pubCodeId)
        {
            return AllTransDataMap
                .Where(x => x.TransformationID == transformationId && x.PubID == pubCodeId)
                .ToList();
        }

        public IList<TransformDataMap> GetDataMaps(int transformationId)
        {
            return AllTransDataMap.Where(x => x.TransformationID == transformationId).ToList();
        }

        public IList<Transformation> GetDataMapsActive()
        {
            return DataMapTrans.Where(x => x.MapsPubCode).ToList();
        }

        public Transformation GetTransformation(int transformationId)
        {
            return AllTrans.FirstOrDefault(x => x.TransformationID == transformationId);
        }

        public IList<Transformation> GetSplitTransformsForPubCode(int pubCode)
        {
            return SplitTrans.Where(t => t.PubMap.Any(x => x.PubID == pubCode)).ToList();
        }

        public IList<Transformation> GetAssignTransformsForPubCode(int pubCode)
        {
            return AssignTrans.Where(d => d.PubMap.Any(x => x.PubID == pubCode)).ToList();
        }

        public IList<Transformation> GetDataMapTransformsForPubCode(int pubCodeId)
        {
            return DataMapTrans.Where(d => d.PubMap.Any(x => x.PubID == pubCodeId)).ToList();
        }

        public IEnumerable<TransformSplit> GetTransformSplits(int transformationId)
        {
            return AllTransSplitMap.Where(x => x.TransformationID == transformationId).ToList();
        }

        public IEnumerable<TransformAssign> GetTransAssigns(int transformationId)
        {
            return AllTransAssign.Where(x => x.TransformationID == transformationId).ToList();
        }

        public void RemoveDataMaps(IEnumerable<Transformation> transformations)
        {
            foreach (var t in transformations)
            {
                DataMapTrans.Remove(t);
            }
        }

        public TransformSplitTrans GetGetSplitTrans(int transformationId)
        {
            return AllSplitTrans.SingleOrDefault(x => x.TransformationID == transformationId);
        }

        public IEnumerable<TransformDataMap> GetEveryTransDataMaps(int dataMapId)
        {
            return EveryTransDataMap.Where(x => x.TransformationID == dataMapId).ToList();
        }

        public TransformSplit GetEveryTransSplitMap(int splitAfterId)
        {
            return EveryTransSplitMap.SingleOrDefault(x => x.TransformationID == splitAfterId);
        }

        public IEnumerable<TransformAssign> GetAllTransAssigns()
        {
            return AssignTrans.SelectMany(t =>
                AllTransAssign.Where(x => x.TransformationID == t.TransformationID)); 
        }

        public IEnumerable<TransformJoin> GetTransJoinMap(int transformationId)
        {
            return AllTransJoinMap.Where(x => x.TransformationID == transformationId);
        }

        public IEnumerable<Transformation> GetJoinTransformsForPubCode(int pubCodeId)
        {
            return JoinTrans
                .Where(x => x.PubMap.Any(m => m.PubID == pubCodeId))
                .ToList();
        }
    }
}