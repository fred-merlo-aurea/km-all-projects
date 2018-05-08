CREATE TABLE [dbo].[ResponseGroups] (
    [ResponseGroupID]   INT           IDENTITY (1, 1) NOT NULL,
    [PubID]             INT           NULL,
    [ResponseGroupName] VARCHAR (100) NOT NULL,
    [DisplayName]       VARCHAR (100) NOT NULL,
	[DateCreated]        DATETIME CONSTRAINT [DF_ResponseGroups_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]		INT           NULL,
    [UpdatedByUserID]		INT           NULL,
	[DisplayOrder]			INT			  NULL,
	[IsMultipleValue]		BIT			  NULL DEFAULT ((0)),
	[IsRequired]			BIT			  NULL DEFAULT ((0)),
	[IsActive]				BIT			  NULL DEFAULT ((1)),
	[WQT_ResponseGroupID]   INT			  NULL,
    [ResponseGroupTypeId]   INT			  NULL, 
    CONSTRAINT [PK_ResponseGroups] PRIMARY KEY CLUSTERED ([ResponseGroupID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ResponseGroups_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_ResponseGroups_ResponseGroupName_PubID]
    ON [dbo].[ResponseGroups]([ResponseGroupName] ASC, [PubID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_ResponseGroups_PubID]
    ON [dbo].[ResponseGroups]([PubID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE TRIGGER [dbo].[TR_ResponseGroups_Insert]
   ON [dbo].[ResponseGroups] 
   AFTER INSERT 
AS  
BEGIN 
   SET NOCOUNT ON;  
 
   create table #ins (id int identity(1,1), PubID int, displayorder int)
   
   insert into #ins (PubID, displayorder)
   Select i.PubID, isnull(MAX(rg.displayorder),0) 
   From 
		inserted i left outer join 
		ResponseGroups rg on i.PubID = rg.PubID
	where 
		isnull(i.displayorder, 0) = 0
   group by 
		i.PubID
   
   UPDATE rg
   SET displayorder = i1.displayorder + ID
   FROM 
		ResponseGroups rg join 
		INSERTED I on rg.ResponseGroupID = i.ResponseGroupID join 
		#ins i1 on i.PubID = i1.PubID
   
   drop table #ins
END
GO