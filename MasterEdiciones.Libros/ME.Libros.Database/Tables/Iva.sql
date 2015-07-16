CREATE TABLE [dbo].[Iva] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]     NVARCHAR (100) NOT NULL,
    [Fecha_Alta] DATETIME       NOT NULL,
    [Alicuota] DECIMAL NOT NULL, 
    CONSTRAINT [PK_dbo.Iva] PRIMARY KEY CLUSTERED ([Id] ASC)
);

