create table FileProcessingStat
(
      ClientId int NOT NULL,
      FileCount int,
      ProfileCount int,
      DemographicCount int,
      ProcessDate date NOT NULL, 
    CONSTRAINT [PK_FileProcessingStat] PRIMARY KEY ([ClientId], [ProcessDate])
)
