CREATE TABLE [dbo].[Provincia] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Nombre]    NVARCHAR (100) NOT NULL,
    [FechaAlta] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Provincia] PRIMARY KEY CLUSTERED ([Id] ASC)
);



