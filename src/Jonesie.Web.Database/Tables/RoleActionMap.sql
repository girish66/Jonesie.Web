CREATE TABLE [dbo].[RoleActionMap] (
    [RoleActionMapId] INT identity NOT NULL, 
    [RoleId] INT NOT NULL,
    [Path] VARCHAR (250) NOT NULL,
    [row_version] ROWVERSION NOT NULL, 
    CONSTRAINT [PK_RoleActionMap] PRIMARY KEY CLUSTERED ([RoleActionMapId]), 
    CONSTRAINT [FK_RoleActionMap_ToTable] FOREIGN KEY ([RoleId]) REFERENCES [webpages_Roles]([RoleId])
);