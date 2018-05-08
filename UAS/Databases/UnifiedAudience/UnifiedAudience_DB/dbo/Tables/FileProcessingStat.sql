create table FileProcessingStat
(
	SourceFileId int NOT NULL,
    ProfileCount int,
    DemographicCount int,
    ProcessDate date NOT NULL, 
    CONSTRAINT [PK_FileProcessingStat] PRIMARY KEY ([SourceFileId], [ProcessDate])
)
