CREATE TABLE [dbo].[Zona] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (30)  NOT NULL,
    [Descripcion] NVARCHAR (250) NULL,
    [FechaAlta]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Zona] PRIMARY KEY CLUSTERED ([Id] ASC)
);

