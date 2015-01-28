CREATE TABLE [dbo].[Provincia] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]     NVARCHAR (100) NOT NULL,
    [Fecha_Alta] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Provincia] PRIMARY KEY CLUSTERED ([Id] ASC)
);

