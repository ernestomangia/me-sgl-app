CREATE TABLE [dbo].[PlanPago] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Nombre]         NVARCHAR (100)  NOT NULL,
    [Descripcion]    NVARCHAR (150)  NULL,
    [CantidadCuotas] INT             NOT NULL,
    [Monto]          DECIMAL (18, 2) NOT NULL,
    [Tipo]           INT             NOT NULL,
    [FechaAlta]      DATETIME        NOT NULL,
    CONSTRAINT [PK_dbo.PlanPago] PRIMARY KEY CLUSTERED ([Id] ASC)
);

