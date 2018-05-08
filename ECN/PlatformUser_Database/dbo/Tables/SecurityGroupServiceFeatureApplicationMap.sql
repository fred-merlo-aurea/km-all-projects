CREATE TABLE [dbo].[SecurityGroupServiceFeatureApplicationMap] (
    [SecurityGroupServiceFeatureApplicationMapID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]                             INT      NOT NULL,
    [ServiceFeatureID]                            INT      NOT NULL,
    [IsActive]                                    BIT      NOT NULL,
    [CreatedDate]                                 DATETIME NULL,
    [CreatedByUserID]                             INT      NULL,
    [UpdatedDate]                                 DATETIME NULL,
    [UpdatedByUserID]                             INT      NULL
);

