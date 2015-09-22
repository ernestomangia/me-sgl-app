CREATE TABLE [dbo].[Login] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [Usuario]    NVARCHAR (30) NOT NULL,
    [Contrasena] NVARCHAR (30) NULL,
    [FechaAlta]  DATETIME      NOT NULL,
    CONSTRAINT [PK_dbo.Login] PRIMARY KEY CLUSTERED ([Id] ASC)
);

