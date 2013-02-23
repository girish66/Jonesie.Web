CREATE TABLE [dbo].[UserProfile] (
    [UserId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (MAX) NULL,
    [Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [LastLogin] DATETIMEOFFSET NULL,
    [row_version] ROWVERSION NOT NULL, 
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

