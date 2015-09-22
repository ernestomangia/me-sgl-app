CREATE TABLE [dbo].[Iva] (
    [Id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Codigo]            INT             NOT NULL,
    [Nombre]            NVARCHAR (80)   NOT NULL,
    [Alicuota]          DECIMAL (18, 2) NULL,
    [HabilitarEliminar] BIT             NOT NULL,
    [FechaAlta]         DATETIME        NOT NULL,
    CONSTRAINT [PK_dbo.Iva] PRIMARY KEY CLUSTERED ([Id] ASC)
);



