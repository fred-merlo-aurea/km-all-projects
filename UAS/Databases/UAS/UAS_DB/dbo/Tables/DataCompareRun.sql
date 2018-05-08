CREATE TABLE [dbo].[DataCompareRun]
(
	    DcRunId int identity(1,1),
        ClientId int not null,
        SourceFileId int not null,
        FileRecordCount int default((0)) null,
        MatchedRecordCount int default((0)) null,
        UadConsensusCount int default((0)) null,
        ProcessCode varchar(50) not null, 
		DateCreated datetime default(getdate()) not null,
		IsBillable bit default('true') not null,
		Notes varchar(max) null,
    CONSTRAINT [PK_DataCompareRun] PRIMARY KEY ([DcRunId])
)
