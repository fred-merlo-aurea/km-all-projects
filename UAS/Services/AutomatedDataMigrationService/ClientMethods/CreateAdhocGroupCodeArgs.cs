namespace ADMS.ClientMethods
{
    internal class CreateAdhocGroupCodeArgs
    {
        public CreateAdhocGroupCodeArgs(
            int clientId,
            int sourceFileId,
            int evtSourceFileId,
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup agWorker,
            string adHocDimensionGroupName,
            int orderOfOperation,
            string standardField,
            string createdDimension)
        {
            ClientId = clientId;
            SourceFileId = sourceFileId;
            EvtSourceFileId = evtSourceFileId;
            AgWorker = agWorker;
            AdHocDimensionGroupName = adHocDimensionGroupName;
            OrderOfOperation = orderOfOperation;
            StandardField = standardField;
            CreatedDimension = createdDimension;
        }

        public int ClientId { get; private set; }

        public int SourceFileId { get; private set; }

        public int EvtSourceFileId { get; private set; }

        public FrameworkUAS.BusinessLogic.AdHocDimensionGroup AgWorker { get; private set; }

        public string AdHocDimensionGroupName { get; private set; }

        public int OrderOfOperation { get; private set; }

        public string StandardField { get; private set; }

        public string CreatedDimension { get; private set; }
    }
}
