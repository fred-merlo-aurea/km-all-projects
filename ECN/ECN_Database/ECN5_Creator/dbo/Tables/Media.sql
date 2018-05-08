CREATE TABLE [dbo].[Media] (
    [MediaID]          INT           IDENTITY (1, 1) NOT NULL,
    [AssociatedID]     INT           NULL,
    [AssociatedField]  VARCHAR (50)  NULL,
    [MediaTypeCode]    VARCHAR (50)  NULL,
    [MediaName]        VARCHAR (50)  NULL,
    [MediaDescription] VARCHAR (255) NULL,
    [FilePointer]      VARCHAR (255) NULL,
    [URLPointer]       VARCHAR (255) NULL,
    [PointerTypeCode]  VARCHAR (50)  NULL,
    CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED ([MediaID] ASC) WITH (FILLFACTOR = 80)
);

