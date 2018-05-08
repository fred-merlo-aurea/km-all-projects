CREATE TABLE [dbo].[SmartSegment] (
    [SmartSegmentID]    INT           IDENTITY (1, 1) NOT NULL,
    [SmartSegmentName]  VARCHAR (100) NOT NULL,
    [SmartSegmentOldID] INT           NOT NULL,
    [CreatedUserID]     INT           NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_SmartSegment_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]     INT           NULL,
    [UpdatedDate]       DATETIME      NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_SmartSegment_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SmartSegments] PRIMARY KEY CLUSTERED ([SmartSegmentID] ASC) WITH (FILLFACTOR = 80)
);

